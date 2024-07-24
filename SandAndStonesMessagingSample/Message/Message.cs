namespace SandAndStonesMessaging.Messages
{
    public class Message : IMessage
    {

        public Dictionary<string, string> Headers { get; }
        public byte[] Payload { get; }

        public Message(Dictionary<string, string> headers, byte[] payload)
        {
            ValidateHeaders(headers);
            this.Headers = headers;
            ValidatePayload(payload);
            this.Payload = payload;
        }

        private static void ValidateHeaders(Dictionary<string, string> headers)
        {
            if (headers.Count <= 0 || headers.Count > MessageConstants.MaxHeadersCount)
                throw new ArgumentException("Incorrect format: too many headers");

            foreach (var header in headers)
            {
                if (header.Key.Length <= 0 || header.Key.Length > MessageConstants.MaxHeaderNameLength)
                {
                    throw new ArgumentException($"Incorrect format: some header name exceeded allowed length. Header Name: {header.Key}");
                }
                if (header.Value.Length <= 0 || header.Value.Length > MessageConstants.MaxHeaderValueLength)
                {
                    throw new ArgumentException($"Incorrect format: some header value exceeded allowed length. Header Value: {header.Value}");
                }
            }
        }

        private static void ValidatePayload(byte[] payload)
        {
            if (payload.Length <= 0 || payload.Length > MessageConstants.MaxPayloadInBytes)
            {
                string hexPayloadContent = BitConverter.ToString(payload);
                throw new ArgumentException($"Incorrect format: too large payload: {hexPayloadContent}");
            }
        }
    }
}