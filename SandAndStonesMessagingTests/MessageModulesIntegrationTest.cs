using Message = SandAndStonesMessaging.Messages.Message;
using SandAndStonesMessaging.Codec;
using SandAndStonesMessaging.Messages;
using SandAndStonesMessaging.Utilities;

namespace SandAndStonesMessagingTests
{
    [TestClass]
    public class MessageModulesIntegrationTest
    {
        private static Dictionary<string, string> GenerateHeaders(int headersToGenerate)
        {
            Dictionary<string, string> headers = new();
            for (int i = 0; i< headersToGenerate; i++)
            {
                headers.Add($"header{i}", $"value{i}");
            }

            return headers;
        }

        private static byte[] GeneratePayload(int payloadLength)
        {
            const int maxByteValue = 255;
            byte[] payload = new byte[payloadLength];
            var getSecondsSinceEpoch = () =>
            {
                TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
                return (int)t.TotalSeconds;
            };

            Random rnd = new(getSecondsSinceEpoch());
                
            for (int i = 0; i < payload.Length; i++)
            {
                payload[i] = (byte)(rnd.Next() % maxByteValue);
            }

            return payload;
        }

        [TestMethod]
        public void AreInputOutputMessageEqual()
        {
            var headers = GenerateHeaders(MessageConstants.MaxHeadersCount);
            var payload = GeneratePayload(MessageConstants.MaxPayloadInBytes);

            IMessage inputMessage = new Message(headers, payload);
            IMessageCodec codec = new MessageCodec();
            byte[] encodedMessageBytes = codec.Encode(inputMessage);
            IMessage outputMessage = codec.Decode(encodedMessageBytes);

            Assert.AreEqual(inputMessage.Headers.Count, MessageConstants.MaxHeadersCount);
            Assert.AreEqual(inputMessage.Headers.Count, outputMessage.Headers.Count);
            foreach (var header in headers)
            {
                Assert.AreEqual(inputMessage.Headers[header.Key], outputMessage.Headers[header.Key]);
            }

            Assert.AreEqual(inputMessage.Payload.Length, outputMessage.Payload.Length);
            Assert.IsTrue(Utils.AreBytesSequencesEqual(inputMessage.Payload, outputMessage.Payload));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IsArgumentExceptionThrownCorrectlyOnZeroHeadersError()
        {
            var headers = GenerateHeaders(0);
            var payload = GeneratePayload(63);

            IMessage inputMessage = new Message(headers, payload);
            if (inputMessage.Headers == null) // Suppress IDE0059 - Unnecessary assgnment of a value to input message
            {
                ;
            }
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IsArgumentExceptionThrownCorrectlyOnZeroPayloadError()
        {
            var headers = GenerateHeaders(MessageConstants.MaxHeadersCount);
            var payload = GeneratePayload(0);

            IMessage inputMessage = new Message(headers, payload);
            if (inputMessage.Headers == null) // Suppress IDE0059 - Unnecessary assignment of a value to input message
            {
                ;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IsArgumentExceptionThrownCorrectlyOnExceededHeadersCountError()
        {
            var headers = GenerateHeaders(MessageConstants.MaxHeadersCount + 1);
            var payload = GeneratePayload(MessageConstants.MaxPayloadInBytes);

            IMessage inputMessage = new Message(headers, payload);
            if (inputMessage.Headers == null) // Suppress IDE0059 - Unnecessary assgnment of a value to input message
            {
                ;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IsArgumentExceptionThrownCorrectlyOnExceededPayloadSizeError()
        {
            var headers = GenerateHeaders(MessageConstants.MaxHeadersCount);
            var payload = GeneratePayload(MessageConstants.MaxPayloadInBytes + 1);

            IMessage inputMessage = new Message(headers, payload);
            if (inputMessage.Headers == null) // Suppress IDE0059 - Unnecessary assgnment of a value to input message
            {
                ;
            }
        }


        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void IsNullExceptionThrownCorrectlyOnError()
        {
            #pragma warning disable CS8604
            #pragma warning disable CS8600
            Dictionary<string, string> nullDictionary = null;
            byte[] nullPayload = null;
            IMessage inputMessage = new SandAndStonesMessaging.Messages.Message(nullDictionary, nullPayload);
            if (inputMessage.Headers == null) // Suppress IDE0059 - Unnecessary assgnment of a value to input message
            {
                ;
            }
            #pragma warning restore CS8604
            #pragma warning restore CS8600
        }
    }
}