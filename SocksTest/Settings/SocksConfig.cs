using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace SocksTest.Settings
{
    public interface IConfigProvider
    {
        SocksSettings GetConfig(Stream readFromStream);
    }
    public class EmbeddedBytesConfigLoader : IConfigProvider
    {
        private const int BinaryConfigLength = 2;



        public SocksSettings GetConfig(byte[] serializedDataBuffer)
        {
            var formatter = new BinaryFormatter();
            var ms = new MemoryStream(serializedDataBuffer);
#if DEBUG
            var result = new SocksSettings {ConfiguredAs = ConfigType.DirectBackConnector, BackConnectServerIp = "192.168.0.168", BackConnectServerPort = 1080,PortToListen = 1515};
#else
            var result = (SocksSettings)formatter.Deserialize(ms);
#endif
            return result;
        }

        public SocksSettings GetConfig(Stream embeddedFile)
        {
            using (var memStr = new MemoryStream())
            {

                var r = embeddedFile.Seek(BinaryConfigLength * -1, SeekOrigin.End);
                var lengthArray = new byte[BinaryConfigLength];
                //var serializedLength = 
                var r2 = embeddedFile.Read(lengthArray, 0, lengthArray.Length);

                int serializedDataLength = BitConverter.ToUInt16(lengthArray, 0);//lengthArray, 0);

                embeddedFile.Seek((BinaryConfigLength + serializedDataLength) * -1, SeekOrigin.End);
                var serializedObjectBuffer = new byte[serializedDataLength];
                embeddedFile.Read(serializedObjectBuffer, 0, serializedObjectBuffer.Length);

                return GetConfig(serializedObjectBuffer);
            }
        }

        public SocksSettings GetConfig()
        {
            using (var embeddedFile = File.OpenRead(Assembly.GetEntryAssembly().Location))
            {
                return GetConfig(embeddedFile);
            }
        }
    }
}
