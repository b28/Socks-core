using CustomServer.ConnectionAcceptor;
using CustomServer.ConnectionAcceptor.Identities;

namespace CustomServer.Ui.DataContainers
{
    public struct ConnectedClientInfoRecord
    {
        //public string None => "zalupa";
        public string UserName { get; set; }
        public string OsVersion { get; set; }
        //public string ExternalIpAddress { get; set; }
        public string InternalIpAddress { get; set; }
        public int PortToConnect { get; set; }

        public static ConnectedClientInfoRecord FromContext(ConnectBackContext context)
        {
            var record = new ConnectedClientInfoRecord
            {
                InternalIpAddress = context.ConnectionIdentity.InternalIp.ToString(),
                OsVersion = context.ConnectionIdentity.WindowsVersion,
                UserName = context.ConnectionIdentity.WindowsUserName,
                PortToConnect = context.PortToConnect
            };

            return record;
        }
        //public static ConnectedClientInfoRecord FromIdentity(ConnectBackConnectionIdentity id)
        //{

        //    var c = new ConnectedClientInfoRecord
        //    {



        //        PortToConnect = id.RemoteEndPoint.Port,
        //        InternalIpAddress = id.InternalIp.ToString(),
        //        UserName = id.WindowsUserName,
        //        OsVersion = id.WindowsVersion
        //    };

        //    return c;
        //}
    }
}