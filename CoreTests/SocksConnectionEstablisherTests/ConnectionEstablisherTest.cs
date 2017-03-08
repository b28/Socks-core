using Microsoft.VisualStudio.TestTools.UnitTesting;
using SocksCore;
using SocksCore.SocksHandlers.Socks4;
using System.Net;

namespace CoreTests
{

    public static class DataToConnectionEstablishersTests
    {
        private static string ValidIp = "127.0.0.1";
        private static ushort ValidPort = 1455;

        public static IPEndPoint ValidEndPoint = new IPEndPoint(IPAddress.Parse(ValidIp), ValidPort);

        private static string InvalidIp = "0.2.1.1";
        private static ushort InvalidPort = 9999;
        public static IPEndPoint InvalidEndPoint = new IPEndPoint(IPAddress.Parse(InvalidIp), InvalidPort);
    }

    [TestClass]
    public class ConnectionEstablisherTest
    {



        [TestMethod]
        public void TestIfConnectionToNonExistantEndpointThrowsAnException()
        {
            var exceptionHappened = false;

            ISocksConnectionEstablisher connector = new ConnectionEstablisher();
            try
            {
                connector.ConnectTo(DataToConnectionEstablishersTests.ValidEndPoint);
            }
            catch (ConnectionEstablisherException)
            {
                exceptionHappened = true;
            }

            Assert.IsTrue(exceptionHappened);
        }
    }
}