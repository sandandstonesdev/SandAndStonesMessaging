namespace SandAndStonesMessaging.Messages
{
    public static class MessageConstants
    {
        public const int MaxPayloadInBytes = 256 * (1024 / 8);
        public const int MaxHeadersCount = 63;
        public const int MaxHeaderNameLength = 1023;
        public const int MaxHeaderValueLength = 1023;
    }
}