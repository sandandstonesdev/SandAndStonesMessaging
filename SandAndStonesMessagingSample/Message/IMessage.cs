namespace SandAndStonesMessaging.Messages
{
    public interface IMessage
    {
        Dictionary<string, string> Headers { get; }
        byte[] Payload { get; }
    }
}
