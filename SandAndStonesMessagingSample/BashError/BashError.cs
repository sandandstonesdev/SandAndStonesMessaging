namespace SandAndStonesMessaging.BashError
{
    public enum BashErrorCodeType : int
    {
        NotSet = -1,
        NoError = 0,
        FatalError = 1,
    };

    public abstract class BashErrorBase
    {
        protected BashErrorCodeType BashErrorCode { get; init; } = BashErrorCodeType.NotSet;
        public abstract int Get();
    }
    public class BashError : BashErrorBase
    {
        public BashError() : base()
        {

        }
        private BashError(BashErrorCodeType bashErrorCodeType) : base()
        {
            BashErrorCode = bashErrorCodeType;
        }
        public static BashErrorBase GetNoError()
        {
            return new BashError(BashErrorCodeType.NoError);
        }
        public static BashErrorBase GetFatalError()
        {
            return new BashError(BashErrorCodeType.FatalError);
        }

        public override int Get()
        {
            return (int)BashErrorCode;
        }
    }
}
