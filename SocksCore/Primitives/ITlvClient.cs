using System;
using System.Net;
using System.Net.Sockets;

namespace SocksCore.Primitives
{
    public interface ITlvClient : IBytePeeker, IByteReceiver, IDestinationEndPointHolder
    {
        event EventHandler Disconnected;
        event EventHandler<PacketData> DataReceived;
        Socket Client { get; }
        void Connect(IPEndPoint connectTo);
        void Close();
        void Send(byte[] arrayToSend);
    }


    public interface IDestinationEndPointHolder
    {
        IPEndPoint ConnectedToEndPoint { get; }
    }
}