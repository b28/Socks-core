using System.Net;

namespace SocksCore.Primitives
{
    public interface ISocksConnection
    {
        void EstablishConnection(IPEndPoint connectTo);
    }
}