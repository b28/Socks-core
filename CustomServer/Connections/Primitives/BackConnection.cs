using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace CustomServer.Connections.Primitives
{
    public class ActiveConnection
    {
        public TcpClient Connection;

        protected Task ReportRoutine;

        protected bool reportWhenDisconnected;


        public bool ReportOnDisconnect
        {
            get { return reportWhenDisconnected; }
            set
            {
                reportWhenDisconnected = value;
                if (!reportWhenDisconnected) return;

                if (Connection.Connected)
                {
                    ReportRoutine = new Task((() =>
                    {
                        Thread.Sleep(1000); // checking rate
                        if (reportWhenDisconnected && Connection.Connected == false)
                        {
                            reportWhenDisconnected = false; // disable reporting
                            OnClosed(this);
                        }
                    }));
                    ReportRoutine.Start();
                }

            }
        }


        public event EventHandler<ActiveConnection> Closed;


        #region Connect overloads

        public void Connect(string host, int port)
        {
            Connection.Connect(host, port);
            ReportOnDisconnect = true;
        }

        public void Connect(IPEndPoint connectTo)
        {
            Connection.Connect(connectTo);
            ReportOnDisconnect = true;
        }

        #endregion
        protected virtual void OnClosed(ActiveConnection e)
        {
            Closed?.Invoke(this, e);
        }
    }
    public class BackConnection
    {

    }
}