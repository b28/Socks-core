using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using SocksCore;
using SocksCore.Primitives;
using SocksCore.SocksHandlers.Socks4;
using SocksCore.Utils.Log;

namespace CoreTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestCategory("Autonomy tests")]
        [TestMethod]
        public void AssertThatInvalidSocksVersionWillCloseConnection()
        {

            //Arrange

            var huySocket = Substitute.For<ITlvClient>();
            huySocket.PeekBytes(1).Returns(new byte[] { 0 });
            var universalHandler = Substitute.For<ISocksClientHandler>();


            //Act
            var logger = new DebugLogger();
            var core = new SocksCore.UniversalTlvCore(logger, new Socks4ClientHandler(logger));
            //core.AcceptClientConnection(huySocket);


            //Assert
            //huySocket.Received().Close();

        }



        [TestCategory("Integration Socket Testing")]
        public void AssertThatInvalidSocksVersionWillCloseConnection2()
        {


        }


    }
}
