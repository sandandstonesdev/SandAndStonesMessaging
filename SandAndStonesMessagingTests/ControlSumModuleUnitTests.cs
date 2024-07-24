using SandAndStonesMessaging.ControlSum;
using SandAndStonesMessaging.Utilities;

namespace SandAndStonesMessagingTests
{
    [TestClass]
    public class ControlSumModuleUnitTests
    {
        [TestMethod]
        public void AreControlSumCalculatorInputBytesEqualToOutputBytes()
        {
            byte[] bytesForCrc = new byte[4];
            IControlSumCalculator bytesControlSum = new SimplifiedControlSumCalculator(bytesForCrc);
            bool sequencesEqual = Utils.AreBytesSequencesEqual(bytesForCrc, bytesControlSum.ControlSumBytes);
            Assert.IsTrue(sequencesEqual);
        }

        [TestMethod]
        public void IsControlSumValueTheSameAsControlSumBytesConvertedToUint()
        {
            byte[] bytesForCrc = new byte[4];
            IControlSumCalculator bytesControlSum = new SimplifiedControlSumCalculator(bytesForCrc);
            uint crcUint = BitConverter.ToUInt32(bytesControlSum.ControlSumBytes);
            bool crcValueAndCrcValueFromBytesEqual = bytesControlSum.Value == crcUint;
            Assert.IsTrue(crcValueAndCrcValueFromBytesEqual);
        }
    }
}
