using SocksCore;
using SocksCore.Primitives;
using SocksCore.SocksHandlers;
using SocksCore.SocksHandlers.Socks4;
using SocksCore.Utils.Log;
using SocksTest.ConnectionEstablisher;
using SocksTest.Settings;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows;
using SocksTest.ConnectionEstablishers;

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

            var compositionRoot = new CompositionRoot();
            compositionRoot.StartComposition();
        }
    }



    public class CompositionRoot
    {
        private readonly IConfigProvider configProvider;
        private IConnectorFactory connectorFactory;
        private IConnectionEstablisher connectionEstablisher;

        private ITlvClientSource clientSource;


        private readonly ILogger logger = new Logger(
            Path.Combine(Directory.GetCurrentDirectory(), "log.txt"));

        private readonly UniversalTlvCore core;




        public CompositionRoot()
        {
            core = new UniversalTlvCore(logger, new Socks4ClientHandler(logger));
            configProvider = new EmbeddedBytesConfigLoader();
        }

        public void StartComposition()
        {
            
            SocksSettings socksSettings;

            using (var embeddedFile = File.OpenRead(Assembly.GetEntryAssembly().Location))
            {
                socksSettings = configProvider.GetConfig(embeddedFile);
            }

            connectionEstablisher = connectorFactory.GetConnectorByConfig(socksSettings); // get proper connector




            var connection =
                connectionEstablisher.Connect(new IPEndPoint(IPAddress.Parse(socksSettings.BackConnectServerIp),
                    socksSettings.BackConnectServerPort));
            var tlvClient = new TcpClientEx(connection.Client);


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
                logger.Fatal(e.Message);
            }

        }

        private void CoreOnDisconnected(object sender, string s)
        {
            logger.Notice(s);
        }

        private void CoreOnConnectionEstablished(object sender, string message)
        {
            logger.Notice(message);
        }

        private void ClientSourceOnNewTlvClientConnected(object sender, ITlvClient tlvClient)
        {
            core.AcceptClientConnection(tlvClient);
        }
    }


}
