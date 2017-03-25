using CommandLine;
using CustomServer.ConnectionAcceptor;
using CustomServer.ConnectionAcceptor.Server;
using CustomServer.Ui;
using CustomServer.Ui.DataContainers;
using log4net;
using log4net.Config;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows;

namespace CustomServer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {



        ILog logger;
        private MainWindowViewModel vm;
        private void ApplicationStartupMethod(object sender, StartupEventArgs e)
        {
            XmlConfigurator.Configure();
            logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            logger.Info("Application started.");

            var options = new Options();

            try
            {

                if (!Parser.Default.ParseArguments(e.Args, options))
                {

                    try
                    {
                        options = JsonConvert.DeserializeObject<Options>(File.ReadAllText(Path.Combine(
                            Directory.GetCurrentDirectory(), "Config", "Settings.json")));
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Can't read command line options.");
                        Environment.Exit(1);
                    }
                }

            }
            catch (Exception)
            {
                MessageBox.Show($"Can't read settings{Environment.NewLine}not from command line options.{Environment.NewLine}Not from configuration file.");
                Environment.Exit(1);
            }




            var mainWindow = new MainWindow();
            mainWindow.Show();

            vm = mainWindow.DataContext as MainWindowViewModel;
            vm.ListenIp = options.InternalIp;
            vm.ListenPort = options.ExternalPort;

            try
            {


                var internalEndPoint = new IPEndPoint(IPAddress.Parse(options.InternalIp), 0);
                var externalEndPoint = new IPEndPoint(IPAddress.Parse(options.ExternalIp), options.ExternalPort);

                var serverContext = new ServerContext(externalEndPoint, internalEndPoint);



                //Debug.WriteLine(Thread.CurrentThread.ManagedThreadId);


                var socketListener = new AsynchronousSocketListener(logger);

                socketListener.RemoteClientConnected += RemoteClientConnected;
                socketListener.RemoteClientDisconnected += SocketListenerOnRemoteClientDisconnected;
                socketListener.DebugAction += SocketListenerOnDebugAction;

                AddLogMsgToVm($"Starting listener on {options.ExternalIp}:{options.ExternalPort}");


                socketListener.StartListening(serverContext.ExternalEndpoint);
            }
            catch (Exception exc)
            {

                MessageBox.Show(exc.Message);
            }

            //Console.ReadLine();

            //Console.WriteLine("Exiting.");
        }

        private void SocketListenerOnRemoteClientDisconnected(object sender, ConnectBackContext connectBackContext)
        {
            vm.RemoveFromClientList(ConnectedClientInfoRecord.FromContext(connectBackContext));
            AddLogMsgToVm($"Remote user \"{connectBackContext.ConnectionIdentity.WindowsUserName}\" has disconnected all his connections.");
        }

        private void OnDisconnected(object sender, IPEndPoint ipEndPoint)
        {
            MessageBox.Show($"{ipEndPoint.Address}:{ipEndPoint.Port} disconnected.");
        }

        private void OnDataReceived(object sender, byte[] bytes)
        {
            var s = Encoding.ASCII.GetString(bytes);
            MessageBox.Show(s);
        }

        private void SocketListenerOnDebugAction(object sender, string s)
        {
            AddLogMsgToVm(s);
        }

        private void AddLogMsgToVm(string message)
        {
            vm.AddToLog(message);
        }

        //private void RemoteClientDisconnected(object sender, ConnectedClientInfoRecord connectBackContext)
        //{
        //    vm.ClientsList.Remove(connectBackContext);
        //    AddLogMsgToVm(connectBackContext.ToString());
        //}

        private void RemoteClientConnected(object sender, ConnectBackContext connectBackContext)
        {
            vm.AddToClientList(ConnectedClientInfoRecord.FromContext(connectBackContext));





            //vm.ClientsList.

        }


    }
}
