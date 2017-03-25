﻿using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using SocksCore;
using SocksCore.Primitives;
using SocksCore.Utils;
using SocksTest.Connectors;
using SocksTest.Connectors.Connections;

namespace SocksTest.TlvClientSources
{
    public sealed class DirectConnectionEstablisher : TlvClientSourceBase
    {
        private readonly IIdentityFactory identityFactory;
        private const int FreeConnectionsLimit = 5;
        private const int SleepInterval = 250;
        private bool MustCreateBackConnection => FreeConnectionsPool?.Count() < FreeConnectionsLimit;
        private IPEndPoint backconnectorEndPoint;

        public DirectConnectionEstablisher(IPEndPoint backconnectorEndPoint, IIdentityFactory identityFactory)
        {
            this.identityFactory = identityFactory;
            this.backconnectorEndPoint = backconnectorEndPoint;
            //listener = new TcpListener(backconnectorEndPoint);
        }


        ConcurrentList<IBackConnection> FreeConnectionsPool = new ConcurrentList<IBackConnection>();

        private void RegisterConnection(IBackConnection connectionToRegister)
        {
            connectionToRegister.HasDataToRead += ConnectionToRegisterOnHasDataToRead;
            connectionToRegister.Disconnected += ConnectionToRegisterDisconnected;
            FreeConnectionsPool.Add(connectionToRegister);
        }

        private void ConnectionToRegisterDisconnected(object sender, BackConnection backConnection)
        {
            var client = sender as IBackConnection;

            ConnectionDisconnected(client);
        }

        private void ConnectionDisconnected(IBackConnection connection)
        {
            connection.HasDataToRead -= ConnectionToRegisterOnHasDataToRead;
            connection.Disconnected -= ConnectionToRegisterDisconnected;
            FreeConnectionsPool.Remove(connection);
        }
        private void ConnectionToRegisterOnHasDataToRead(object sender, EventArgs eventArgs)
        {
            var client = sender as IBackConnection;
            NotifyClientArrived(client);
        }

        private void NotifyClientArrived(IBackConnection connectionWithClient)
        {
            OnNewTlvClientConnected(new TcpClientEx(connectionWithClient.Connection.Client));
            ConnectionDisconnected(connectionWithClient);
        }


        public override async void StartConnections()
        {


            try
            {
                while (true)
                {
                    while (MustCreateBackConnection)
                    {
                        var newConnection = new TcpClient();
                        newConnection.Connect(backconnectorEndPoint);
                        var clientInfo = RemoteClientInfo.Get().ToByteArray();
                        var sendStream = newConnection.GetStream();
                        newConnection.Client.Send(clientInfo); // send identity to back server
                        var backConnection = BackConnection.From(newConnection);
                        RegisterConnection(backConnection);
                        await backConnection.BeginPollAsync();

                    }
                    Thread.Sleep(SleepInterval);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }


    }
}
