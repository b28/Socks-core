using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using SocksCore;
using SocksCore.SocksHandlers.Socks4;
using System.IO;

namespace CoreTests.Socks4RequestTests
{

    public static class Socks4RequestExamples
    {


        /// <summary>
        /// Standart IPv4 Request with NO user Name
        /// </summary>
        private static readonly byte[] Socks4DummyUserNameConnectRequest = { 0x4, 1, 0, 80, 192, 168, 0, 168, (byte)'d', (byte)'u', (byte)'m', (byte)'m', (byte)'y', 0 };

        private static readonly string UserName = "dummy";
        public static readonly string IpForParsing = "192.168.0.168";
        private static Socks4Request tempObject;
        public static Socks4Request GetObjectToTestWithdummyUserName
        {
            get
            {
                if (tempObject.Header.ProtocolVersion != 0) return tempObject;
                var receiver = Substitute.For<IByteReceiver>();

                var buffer = Socks4RequestExamples.Socks4DummyUserNameConnectRequest;
                var ms = new MemoryStream(buffer);

                receiver.Receive(Arg.Any<int>()).Returns(x =>
                {
                    var count = (int)x[0];
                    byte[] result = new byte[count];
                    ms.Read(result, 0, count);
                    return result;
                });

                tempObject = Socks4Request.From(receiver);
                return tempObject;
            }
        }
    }




    [TestClass]
    public class Socks4RequestTests
    {

        [TestMethod]
        public void AssertThatThisIsNoUserNameSocks4Request()
        {
            var request = Socks4RequestExamples.GetObjectToTestWithdummyUserName;

            Assert.IsTrue(request.Header.ProtocolVersion == (int)SocksVersion.Socks4);
            Assert.IsFalse(string.IsNullOrEmpty(request.UserName));
        }


        [TestMethod]
        public void AssertIfPortFromRequestNotEqual80IpParsingError()
        {
            var request = Socks4RequestExamples.GetObjectToTestWithdummyUserName;

            Assert.IsTrue(string.CompareOrdinal(request.IpAddress.ToString(), Socks4RequestExamples.IpForParsing) == 0);
            Assert.IsFalse(request.Header.Port != 80);
        }
    }
}
