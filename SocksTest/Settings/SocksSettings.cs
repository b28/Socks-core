using System;
using System.Runtime.Serialization;

namespace SocksTest.Settings
{
    public interface ISocksSettingsDeserializer
    {
        SocksSettings GetConfig();
    }

    public interface ISocksSettingsSerializer
    {
        byte[] SerializeData(SocksSettings dataToSerialize);
    }

    [Serializable]
    public enum ConfigType : byte
    {
        [DataMember]
        SocksServer = 1, // port listener 
        [DataMember]
        DirectBackConnector, // back connect to direct 
        [DataMember]
        ProxyBackConnector, // back connect through proxy
        [DataMember]
        ProxyBackConnectorWithAuth, // back connect through proxy with authentication 
        [DataMember]
        ProxyBackConnectorWithDomainAuth // back connect through proxy with NTLM-authentication 
    }

    [Serializable]
    public class SocksSettings
    {
        [DataMember]
        public string UserName;//{ get; set; }  
        [DataMember]
        public string UserPassword;// { get; set; }
        [DataMember]
        public string DomainName;//{ get; set; }
        [DataMember]
        public string ProxyIp;// { get; set; }
        [DataMember]
        public int ProxyPort;// { get; set; }
        [DataMember]
        public string BackConnectServerIp;// { get; set; }
        [DataMember]
        public int BackConnectServerPort;// { get; set; }
        [DataMember]
        public ushort PortToListen;
        [DataMember]
        public ConfigType ConfiguredAs;
        public override string ToString()
        {
            return $"UserName: {UserName}, UserPassword: {UserPassword}, DomainName: {DomainName}, ProxyIp: {ProxyIp}, ProxyPort: {ProxyPort}, BackConnectServerIp: {BackConnectServerIp}, BackConnectServerPort: {BackConnectServerPort}, PortToListen: {PortToListen}, ConfiguredAs: {ConfiguredAs}";
        }
    }
}