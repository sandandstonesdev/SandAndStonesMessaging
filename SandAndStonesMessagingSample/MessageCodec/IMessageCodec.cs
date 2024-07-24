using SandAndStonesMessaging.Messages;

namespace SandAndStonesMessaging.Codec
{
    public interface IMessageCodec
    {
        byte[] Encode(IMessage message);
        IMessage Decode(byte[] data);
    }
}