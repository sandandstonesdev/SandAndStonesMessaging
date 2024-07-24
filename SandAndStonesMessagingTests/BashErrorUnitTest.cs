using SandAndStonesMessaging.BashError;

namespace SandAndStonesMessagingTests
{
    [TestClass]
    public class BashErrorUnitTest
    {
        public void DoesBashErrorNotSetWorkAsExpected()
        {
            BashErrorBase bashErrorNotSet = new BashError();
            int errorCodeNotSet = bashErrorNotSet.Get();
            Assert.AreEqual(errorCodeNotSet, BashErrorCodeType.NotSet);
        }
        public void DoesBashNoErrorWorkAsExpected()
        {
            BashErrorBase bashErrorNoError = BashError.GetNoError();
            int noErrorCode = bashErrorNoError.Get();
            Assert.AreEqual(noErrorCode, BashErrorCodeType.NoError);
        }
        public void DoesBashFatalErrorWorkAsExpected()
        {
            BashErrorBase bashErrorFatalError = BashError.GetFatalError();
            int fatalErrorCode = bashErrorFatalError.Get();
            Assert.AreEqual(fatalErrorCode, BashErrorCodeType.FatalError);
        }
    }
}
