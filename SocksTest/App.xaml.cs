using SocksCore;
using SocksCore.Primitives;
using SocksCore.SocksHandlers.Socks4;
using SocksCore.Utils.Log;
using System;
using System.IO;
using System.Net;
using System.Windows;

namespace SocksTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var compisitionRoot = new CompositionRoot();
            compisitionRoot.StartComposition();
        }
    }



    public class CompositionRoot
    {
        private Logger logger = new Logger(
            Path.Combine(Directory.GetCurrentDirectory(), "log.txt"));
        UniversalTlvCore core;
        public CompositionRoot()
        {
            core = new UniversalTlvCore(logger, new Socks4ClientHandler(logger));
        }

        public void StartComposition()
        {
            var innerEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.168"), 1112);
            var clientSource = new TlvClientSourceFromListener(logger);

            logger.CurrentLogLevel = Logger.LogLevel.Debug;

            core.ConnectionEstablished += CoreOnConnectionEstablished;
            core.Disconnected += CoreOnDisconnected;
            clientSource.NewTlvClientConnected += ClientSourceOnNewTlvClientConnected;
            try
            {
                clientSource.BeginAcceptClients(innerEndPoint);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        private void CoreOnDisconnected(object sender, string s)
        {
            logger.Notice(s);
        }

        private void CoreOnConnectionEstablished(object sender, string s)
        {
            logger.Notice(s);
        }

        private void ClientSourceOnNewTlvClientConnected(object sender, ITlvClient tlvClient)
        {
            core.AcceptClientConnection(tlvClient);
        }
    }
}
