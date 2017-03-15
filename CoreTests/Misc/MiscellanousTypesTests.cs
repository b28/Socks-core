using Microsoft.VisualStudio.TestTools.UnitTesting;
using SocksCore.SocksHandlers;
using System.Linq;
using Socks4Response = SocksCore.SocksHandlers.Socks4.Socks4Response;

namespace CoreTests.Misc
{

    [TestClass]
    public class MiscellanousTypesTests
    {
        [TestCategory("Autonomy tests")]
        [TestMethod]
        public void AssertIfSocks4ResponseStructIsNotEqualToGivenData()
        {
            var rawData = new byte[] { 0x00, 0x5a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }; // request allowed

            var testedData = new Socks4Response(Socks4ErrorCodes.Success);
            var testedDataAsBytes = testedData.GetBytes();
            var equal = rawData.SequenceEqual(testedDataAsBytes);
            Assert.IsTrue(equal);
        }

    }
}