namespace SocksCore.Primitives
{
    public class SocketSettings
    {
        public uint NetworkClientKeepAliveInterval = 1000;

        public uint NetworkClientKeepAliveTimeout = 1000;

        public int NetworkClientReceiveBufferSize = 1024 * 1024 * 3;

        public int NetworkClientReceiveTimeout = 1000 * 60 * 180;

        public int NetworkClientSendBufferSize = 1024 * 1024 * 3;

        public int NetworkClientSendTimeout = 1000 * 60 * 180;

        public bool UseNoDelay = true;


        public SocketSettings()
        {

        }

        public static SocketSettings Default => new SocketSettings();


        public static SocketSettings DefaultHigh
        {
            get
            {
                const int timeOut = 1000 * 60 * 30;
                return new SocketSettings()
                {
                    NetworkClientReceiveTimeout = timeOut,
                    NetworkClientSendTimeout = timeOut,
                    NetworkClientKeepAliveInterval = 1000,
                    UseNoDelay = true,
                    NetworkClientKeepAliveTimeout = 1000,
                };
            }
        }

    }
}