namespace SandAndStonesMessaging.ControlSum
{
    public class SimplifiedControlSumCalculator : IControlSumCalculator
    {
        private readonly byte[] inputData;
        private readonly uint controlSum = 0;

        public uint Value
        {
            get { return controlSum; }
        }

        public byte[] ControlSumBytes
        {
            get
            {
                byte[] bytesOfSum = BitConverter.GetBytes(controlSum);
                return bytesOfSum;
            }
        }

        public SimplifiedControlSumCalculator(byte[] data)
        {
            this.inputData = data;
            this.controlSum = CalculateControlSum(this.inputData);
        }

        private static uint CalculateControlSum(byte[] inputData)
        {
            uint sum = 0;
            unchecked
            {
                foreach (var b in inputData)
                {
                    sum += b;
                }
            }

            return sum;
        }
    }
}