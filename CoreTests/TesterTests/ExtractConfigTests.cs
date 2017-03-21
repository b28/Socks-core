using Microsoft.VisualStudio.TestTools.UnitTesting;
using SocksTest.Settings;
using System.IO;

namespace CoreTests.TesterTests
{
    [TestClass]
    public class ExtractConfigTests
    {
        [TestMethod]
        public void AssertIfCannotReadConfigFromItself()
        {
            IConfigProvider provider = new EmbeddedBytesConfigLoader();
            var peToCheckPath = Path.Combine(@"f:\Asynchronus transfer\software\Socks\CoreTests\bin\Debug\", "configured_SocksTest.exe");
            Assert.IsTrue(File.Exists(peToCheckPath));

            SocksSettings currentSettings;

            using (var fileWithInjectedConfig = File.OpenRead(peToCheckPath))
            {
                currentSettings = provider.GetConfig(fileWithInjectedConfig);
            }

            Assert.IsNotNull(currentSettings);


        }
    }
}
