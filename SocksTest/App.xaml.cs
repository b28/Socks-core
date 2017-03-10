using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using SocksCore;
using SocksCore.Primitives;
using SocksCore.SocksHandlers.Socks4;
using SocksCore.Utils.Log;

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
        UniversalSocksCore core;
        public CompositionRoot()
        {
            core = new UniversalSocksCore(new Socks4ClientHandler(logger), null);
        }

        public void StartComposition()
        {
            var innerEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.168"), 1112);
            var clientSource = new SocksClientSourceFromListener(logger);

            logger.CurrentLogLevel = Logger.LogLevel.Debug;

            core.ClientConnected += CoreOnClientConnected;
            core.ClientRequestReceived += CoreOnClientRequestReceived;
            clientSource.NewSocksClientConnected += ClientSourceOnNewSocksClientConnected;
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

        private void CoreOnClientRequestReceived(object sender, string s)
        {
            logger.Notice(s);
        }

        private void CoreOnClientConnected(object sender, string s)
        {
            logger.Notice(s);
        }

        private void ClientSourceOnNewSocksClientConnected(object sender, ISocksClient socksClient)
        {
            core.AcceptClientConnection(socksClient);
        }
    }
}
