using System;
using System.Net;
using System.Net.Sockets;

namespace SocksCore.Primitives
{
    public interface ISocksClient : IBytePeeker, IByteReceiver
    {
        event EventHandler Disconnected;
        event EventHandler<DataStruct> DataReceived;
        Socket Client { get; }
        void Connect(IPEndPoint connectTo);
        void Close();
        void Send(byte[] arrayToSend);
    }
}