using SandAndStonesMessaging.ControlSum;
using SandAndStonesMessaging.Messages;
using SandAndStonesMessaging.Utilities;
using System.Text;

namespace SandAndStonesMessaging.Codec
{
    public class MessageCodec : IMessageCodec
    {
        private readonly byte[] packetSignature = { 0x53, 0x45, 0x4e, 0x44 }; // 'SAND'
        private readonly byte[] headSignature = { 0x48, 0x45, 0x41, 0x44 }; // 'HEAD'
        private readonly byte[] payloadSignature = { 0x50, 0x41, 0x59, 0x4c }; // 'PAYL' 
        private readonly byte[] endOfPacketSignature = { 0x45, 0x4e, 0x44, 0x50 }; // 'ENDP' 

        private readonly byte crByte = 0x0d; // Header separator
        private readonly byte spaceByte = 0x20; // Header name/value separator

        private readonly char spaceSeparator = ' ';
        private readonly char newLineSeparator = '\r';

        private static byte[] GetBytesFromPacketData(byte[] packetData, int offset, int size)
        {
            byte[] retrievedData = new byte[size];
            Buffer.BlockCopy(packetData, offset, retrievedData, 0, size);
            return retrievedData;
        }

        private byte[] GetHeaderBytes(byte[] data, int offset, int headersCount)
        {
            using MemoryStream headerBytesStream = new();
            using BinaryWriter headerWriter = new(headerBytesStream);
            int charsWritten = 0;
            int headersCollected = 0;
            while (headersCollected < headersCount)
            {

                byte b = data[offset++];
                headerWriter.Write(b);
                charsWritten++;
                if (b == crByte)
                {
                    headersCollected++;
                    continue;
                }
            }

            byte[] headerBytes = headerBytesStream.ToArray();
            return headerBytes;
        }

        private Dictionary<string, string> BuildHeaderDictionary(byte[] headerBytes)
        {
            Dictionary<string, string> headerDictionary = new();
            var headerDataString = ASCIIEncoding.ASCII.GetString(headerBytes);
            headerDataString = headerDataString.TrimEnd();
            var tokenizedHeaders = headerDataString.Split(newLineSeparator);

            foreach (var singleHeaderData in tokenizedHeaders)
            {
                var headerStringTokens = singleHeaderData.Split(spaceSeparator);
                if (headerStringTokens.Length == 2)
                {
                    headerDictionary.Add(headerStringTokens[0], headerStringTokens[1]);
                }
            }

            return headerDictionary;
        }

        public IMessage Decode(byte[] data)
        {
            byte[] packetSignatureToCheck = GetBytesFromPacketData(data, 0, 4);
            if (!Utils.AreBytesSequencesEqual(packetSignatureToCheck, packetSignature))
                throw new Exception("Invalid packet signature");

            byte[] headSectionSignatureToCheck = GetBytesFromPacketData(data, 4, 4);
            if (!Utils.AreBytesSequencesEqual(headSectionSignatureToCheck, headSignature))
                throw new Exception("Invalid packet header signature");

            byte[] headerCrcBytes = GetBytesFromPacketData(data, 8, 4);
            uint headerCrc = BitConverter.ToUInt32(headerCrcBytes);

            byte[] headersCountBytes = GetBytesFromPacketData(data, 12, 4);
            uint headersCount = BitConverter.ToUInt32(headersCountBytes);

            int dataIndex = 16;
            byte[] headerBytes = GetHeaderBytes(data, dataIndex, (int)headersCount);
            dataIndex += (int)headerBytes.Length;

            IControlSumCalculator headerControlSum = new SimplifiedControlSumCalculator(headerBytes.ToArray());
            if (headerCrc != headerControlSum.Value)
            {
                throw new Exception("Invalid header CRC");
            }

            Dictionary<string, string> headerDictionary = BuildHeaderDictionary(headerBytes);

            byte[] payloadSectionSignatureToCheck = GetBytesFromPacketData(data, dataIndex, 4);
            if (!Utils.AreBytesSequencesEqual(payloadSectionSignatureToCheck, payloadSignature))
                throw new Exception("Invalid packet payload signature");

            byte[] payloadCrcBytes = GetBytesFromPacketData(data, dataIndex + 4, 4);
            uint payloadCrc = BitConverter.ToUInt32(payloadCrcBytes);

            byte[] payloadCountBytes = GetBytesFromPacketData(data, dataIndex + 8, 4);
            uint payloadBytesCount = BitConverter.ToUInt32(payloadCountBytes);

            byte[] payloadBytes = GetBytesFromPacketData(data, dataIndex + 12, (int)payloadBytesCount);

            IControlSumCalculator payloadControlSum = new SimplifiedControlSumCalculator(payloadBytes.ToArray());
            if (payloadCrc != payloadControlSum.Value)
            {
                throw new Exception("Invalid header CRC");
            }

            byte[] endOfPacketSignatureBytes = GetBytesFromPacketData(data, dataIndex + 12 + (int)payloadBytesCount, 4);
            if (!Utils.AreBytesSequencesEqual(endOfPacketSignatureBytes, endOfPacketSignature))
            {
                throw new Exception("End of packet signature isn't present");
            }

            IMessage msg = new Message(headerDictionary, payloadBytes);
            return msg;
        }

        private byte[] BuildHeaderBytes(Dictionary<string, string> headersDictionary)
        {
            using MemoryStream headerDataStream = new();
            foreach (var header in headersDictionary)
            {
                byte[] keyBytes = ASCIIEncoding.ASCII.GetBytes(header.Key);
                headerDataStream.Write(keyBytes);
                byte[] headerSeparator = { spaceByte };
                headerDataStream.Write(headerSeparator);
                byte[] valueBytes = ASCIIEncoding.ASCII.GetBytes(header.Value);
                headerDataStream.Write(valueBytes);
                byte[] headerPostfix = { crByte };
                headerDataStream.Write(headerPostfix);
            }

            return headerDataStream.ToArray();
        }
        public byte[] Encode(IMessage message)
        {
            byte[] headerDataBytes = BuildHeaderBytes(message.Headers);

            IControlSumCalculator headerControlSum = new SimplifiedControlSumCalculator(headerDataBytes);
            byte[] headerControlSumBytes = headerControlSum.ControlSumBytes;

            byte[] payloadBytes = message.Payload;
            IControlSumCalculator payloadControlSum = new SimplifiedControlSumCalculator(payloadBytes);
            byte[] payloadControlSumBytes = payloadControlSum.ControlSumBytes;

            byte[] headersCountBytes = BitConverter.GetBytes(message.Headers.Count);
            byte[] payloadCountBytes = BitConverter.GetBytes(payloadBytes.Length);

            using MemoryStream packetBytesStream = new();
            packetBytesStream.Write(packetSignature);
            packetBytesStream.Write(headSignature);
            packetBytesStream.Write(headerControlSumBytes);
            packetBytesStream.Write(headersCountBytes);
            packetBytesStream.Write(headerDataBytes);

            packetBytesStream.Write(payloadSignature);
            packetBytesStream.Write(payloadControlSumBytes);
            packetBytesStream.Write(payloadCountBytes);
            packetBytesStream.Write(payloadBytes);
            packetBytesStream.Write(endOfPacketSignature);

            return packetBytesStream.ToArray();
        }
    }
}