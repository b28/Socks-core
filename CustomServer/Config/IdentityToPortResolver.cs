using CustomServer.ConnectionAcceptor.Identities;
using System.IO;
using System.Linq;
using Tiny.EntityDb;

namespace CustomServer.Config
{

    public interface IStoredIdentity
    {
        IConnectBackConnectionIdentity Identity { get; set; }
        ushort Port { get; set; }
    }

    public struct StoredIdentity : IStoredIdentity
    {
        public IConnectBackConnectionIdentity Identity { get; set; }
        public ushort Port { get; set; }
    }


    public class IdentityToPortResolver : IIdentityToPortResolver
    {
        private readonly DataStream dataStream;
        private readonly Stream identityFile;
        private readonly ushort startPortNumber;
        private JsonBaseEntityDatabase<StoredIdentity> savedIdentities;
        public IdentityToPortResolver(string pathToIdentityFile, ushort startPortNumber = 2000)
        {
            this.startPortNumber = startPortNumber;
            identityFile = File.Open(pathToIdentityFile, FileMode.OpenOrCreate);
            dataStream = new DataStream(identityFile);

            savedIdentities = new JsonBaseEntityDatabase<StoredIdentity>(dataStream);
        }
        public ushort GetPortFromIdentity(IConnectBackConnectionIdentity identityToCompare)
        {
            ushort port = 0;
            if (savedIdentities.Count == 0)
            {
                port = startPortNumber;
                savedIdentities.Add(new StoredIdentity { Identity = identityToCompare, Port = port });
            }
            else
            {
                var identity = savedIdentities.FirstOrDefault(a => a.Identity.Equals(identityToCompare));
                if (identity.Equals(null))
                {
                    port = (ushort)(savedIdentities.Last().Port + 1);
                    savedIdentities.Add(new StoredIdentity { Identity = identityToCompare, Port = port });
                }
                port = identity.Port;
            }
            return port;
        }



    }
}
