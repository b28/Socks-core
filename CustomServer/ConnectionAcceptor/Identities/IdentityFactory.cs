using CustomServer.Connections.Primitives;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CustomServer.ConnectionAcceptor.Identities
{
    internal class IdentityFactory : IIdentityFactory
    {
        public ConnectBackContextIdentity ContextIdentity(ConnectBackConnectionIdentity identity, ConnectBackContext backContext)
        {
            var port = ((IPEndPoint)backContext.listener.LocalEndpoint).Port;
            var connectBackContextIdentity = new ConnectBackContextIdentity
            {
                InternalIp = identity.InternalIp,
                WindowsUserName = identity.WindowsUserName,
                WindowsVersion = identity.WindowsVersion,
                PortToConnect = port
            };

            return connectBackContextIdentity;
        }

        public async Task<ConnectBackConnectionIdentity> ConnectionIdentity(TcpClientEx tcpClient)
        {
            try
            {
                var receiveTimeout = tcpClient.Client.ReceiveTimeout;
                tcpClient.Client.ReceiveTimeout = 5000;


                var identityLine = ""; // IPADDRESS |username|sraka|drova| 

                var clientStream = tcpClient.GetStream();

                var ipBuffer = new byte[4];

                var rbc = await clientStream.ReadAsync(ipBuffer, 0, 4); // 4 bytes - is ipaddress
                if (rbc < 1)
                    throw new SocketException();

                var clientAddress = new IPAddress(ipBuffer);


                while (identityLine.Count(x => x == '|') < 3)
                {
                    var identityBuffer = new byte[ConnectBackConnectionIdentity.IdentityBufferSize];
                    // ConnectionIdentity: 192.168.255.255 | username | winxp | (1 arg is an internalIpAddress)

                    var readedIdentitySize = clientStream.Read(identityBuffer, 0, identityBuffer.Length);
                    if (readedIdentitySize < 1)
                        throw new SocketException();
                    //tcpClient.Client.Close();

                    identityLine += Encoding.ASCII.GetString(identityBuffer.ToArray()).TrimEnd('\0');
                }

                var identityParts = identityLine.Split(
                new[]{
                    '|','\r','\n'
                }, StringSplitOptions.RemoveEmptyEntries);


                var id = new ConnectBackConnectionIdentity()
                {
                    //RemoteEndPoint = (IPEndPoint)tcpClient.Client.RemoteEndPoint,
                    InternalIp = clientAddress, //  IPAddress.TryParse() (identityParts[0]),
                    WindowsUserName = identityParts[0],
                    WindowsVersion = identityParts[1].TrimEnd('\0'),
                };
                //parse buffer there

                tcpClient.Client.ReceiveTimeout = receiveTimeout;

                return id;
            }
            catch (Exception)
            {

                return null;
            }

        }
    }
}
