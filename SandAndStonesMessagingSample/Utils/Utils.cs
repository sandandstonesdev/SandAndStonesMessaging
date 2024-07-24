namespace SandAndStonesMessaging.Utilities
{
    public static class Utils
    {
        public const string ConsoleSeparator = "------------------------------";
        public static bool AreBytesSequencesEqual(byte[] sig1, byte[] sig2)
        {
            if (sig1.Length != sig2.Length)
            {
                return false;
            }

            for (int i = 0; i < sig1.Length; i++)
            {
                if (sig1[i] != sig2[i])
                    return false;
            }

            return true;
        }
    }
}
