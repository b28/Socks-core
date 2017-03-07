using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using SocksCore;
using SocksCore.Primitives;

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

            var huySocket = Substitute.For<ISocksClient>();
            huySocket.PeekBytes(1).Returns(new byte[] { 0 });
            var universalHandler = Substitute.For<ISocksClientHandler>();


            //Act

            var core = new SocksCore.SocksCore(universalHandler, universalHandler);
            core.AcceptClientConnection(huySocket);


            //Assert
            huySocket.Received().Close();

        }



        [TestCategory("Integration Socket Testing")]
        public void AssertThatInvalidSocksVersionWillCloseConnection2()
        {


        }


    }
}
