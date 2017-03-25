using CustomServer.ConnectionAcceptor.Identities;

namespace CustomServer.Config
{
    public interface IIdentityToPortResolver
    {
        ushort GetPortFromIdentity(IConnectBackConnectionIdentity identityToCompare);
    }
}