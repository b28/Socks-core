namespace CustomServer.ConnectionAcceptor.Identities
{
    public class ConnectBackContextIdentity : ConnectBackConnectionIdentity
    {
        public ConnectBackContextIdentity() : base()
        {

        }
        public int PortToConnect;
        public bool Equals(ConnectBackContextIdentity other)
        {
            return base.Equals(other);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }


        public override string ToString()
        {
            return $"{base.ToString()} PortToConnect: {PortToConnect}";
        }
    }
}
