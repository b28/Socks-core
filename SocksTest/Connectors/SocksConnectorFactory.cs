using SocksCore;
using SocksCore.SocksHandlers;
using SocksCore.Utils.Log;
using SocksTest.Settings;
using System;
using System.Net;

namespace SocksTest.Connectors
{

    public interface IConnectorFactory
    {
        TlvClientSourceBase GetClientSourceByConfig(SocksSettings settings);
    }

    //class TlvClientSourceFromProxyWithNtlmAuth : TlvClientSourceBase
    //{
    //    private ITlvClientSource connectionEstablisher;
    //    private IPEndPoint backConnectServerEndPoint;
    //    public TlvClientSourceFromProxyWithNtlmAuth(
    //        ProxyEndPoint proxyEndPoint,
    //        ProxyAuthInfo proxyAuth,
    //        IPEndPoint backConnectServerEndPoint)
    //    {
    //        this.backConnectServerEndPoint = backConnectServerEndPoint;
    //        connectionEstablisher = new ThroughProxyConnectionEstablisher(proxyEndPoint, proxyAuth);
    //    }


    //    public override void StartConnection(IPEndPoint getClientsFrom)
    //    {


    //        Task.Run(
    //            () =>
    //            {

    //                while (true)
    //                {
    //                    var client = connectionEstablisher.Connect(getClientsFrom);
    //                    var tlvClient = new TcpClientEx(client.Client);
    //                    OnNewTlvClientConnected(tlvClient);
    //                }
    //            }).ConfigureAwait(false);


    //    }
    //}

    public class SocksConnectorFactory : IConnectorFactory
    {
        private readonly ICanLog canLog;
        public SocksConnectorFactory(ICanLog log)
        {
            canLog = log;
        }
        public TlvClientSourceBase GetClientSourceByConfig(SocksSettings settings)
        {

            TlvClientSourceBase clientSource = null;
            switch (settings.ConfiguredAs)
            {
                case ConfigType.SocksServer:
                    clientSource = new TlvClientSourceFromListener(canLog,
                        new IPEndPoint(IPAddress.Any, settings.PortToListen));
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