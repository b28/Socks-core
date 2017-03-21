using SocksCore;
using SocksCore.Primitives;
using SocksCore.SocksHandlers;
using SocksCore.Utils.Log;
using SocksTest.ConnectionEstablisher;
using SocksTest.Settings;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SocksTest.ConnectionEstablishers
{

    public interface IConnectionEstablisher
    {
        TcpClient Connect(IPEndPoint ipEndPoint, Socket socketToConnect);
        event EventHandler<string> DebugMessage;
    }
    public interface IConnectorFactory
    {
        ITlvClientSource GetConnectorByConfig(SocksSettings settings);
    }

    class TlvClientSourceFromProxyWithNtlmAuth : TlvClientSourceBase
    {
        private IConnectionEstablisher connectionEstablisher;
        private IPEndPoint backConnectServerEndPoint;
        public TlvClientSourceFromProxyWithNtlmAuth(
            ProxyEndPoint proxyEndPoint,
            ProxyAuthInfo proxyAuth,
            IPEndPoint backConnectServerEndPoint)
        {
            this.backConnectServerEndPoint = backConnectServerEndPoint;
            connectionEstablisher = new ThroughProxyConnectionEstablisher(proxyEndPoint, proxyAuth);
        }

        public override void GetClients(IPEndPoint getClientsFrom)
        {

            Task.Run(
                () =>
                {

                    while (true)
                    {
                        var client = connectionEstablisher.Connect(getClientsFrom);
                        var tlvClient = new TcpClientEx(client.Client);
                        OnNewTlvClientConnected(tlvClient);
                    }
                }).ConfigureAwait(false);


        }
    }

    public class SocksConnectorFactory : IConnectorFactory
    {
        private ICanLog canLog;
        public SocksConnectorFactory(ICanLog log)
        {
            canLog = log;
        }
        public ITlvClientSource GetConnectorByConfig(SocksSettings settings)
        {
            ITlvClientSource clientSource = null;
            switch (settings.ConfiguredAs)
            {
                case ConfigType.SocksServer:
                    clientSource = new TlvClientSourceFromListener(canLog, new IPEndPoint(IPAddress.Any, settings.PortToListen));
                    break;
                case ConfigType.DirectBackConnector:
                    break;
                case ConfigType.ProxyBackConnector:
                    break;
                case ConfigType.ProxyBackConnectorWithAuth:
                    break;
                case ConfigType.ProxyBackConnectorWithDomainAuth:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return clientSource;
        }
    }

}