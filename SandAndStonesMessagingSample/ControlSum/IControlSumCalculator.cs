namespace SandAndStonesMessaging.ControlSum
{
    public interface IControlSumCalculator
    {
        uint Value { get; }
        byte[] ControlSumBytes { get; }
    }
}