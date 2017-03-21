using SocksTest.ConnectionEstablisher;
using SocksTest.Settings;
using System;
using System.IO;
using SocksTest.ConnectionEstablishers;

namespace SocksTest
{

    public class ConnectorsFactory : IConnectorFactory
    {
        public IConnectionEstablisher GetConnectorByConfig(SocksSettings settings)
        {
            if (settings == null)
                throw new InvalidDataException("Connector settings is inconsistent or empty");

            if (FieldNotSet(settings.UserName) |
                FieldNotSet(settings.DomainName) |
                FieldNotSet(settings.UserPassword) |
                FieldNotSet(settings.ProxyIp) |
                FieldNotSet(settings.ProxyPort))
                return new DirectConnectionEstablisher();

            var proxyInfo = new ProxyEndPoint
            {
                IpAddress = settings.ProxyIp,
                Port = settings.ProxyPort
            };

            var authInfo = new ProxyAuthInfo
            {
                UserName = settings.UserName,
                UserPassword = settings.UserPassword,
                DomainName = settings.DomainName,
                WorkstationName = Environment.MachineName
            };

            return new ThroughProxyConnectionEstablisher(proxyInfo, authInfo);
        }

        //private bool FieldNotSet(object field)
        //{
        //    var type = field.GetType();
        //    if (type == typeof (string))
        //        return IsStringValueSet(field as string);
        //    if (type == typeof (int))
        //    {
        //        return IsIntValueSet(field as int);
        //    }
        //}

        private static bool FieldNotSet(string value)
        {
            return string.IsNullOrEmpty(value);
        }

        private bool FieldNotSet(int value)
        {
            return value != 0;
        }


    }
}
