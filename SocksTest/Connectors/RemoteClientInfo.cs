using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SocksTest.Connectors
{
    public class RemoteClientInfo
    {
        private IPAddress ipAddress;
        private string username;
        private string osVersion;

        public static RemoteClientInfo Get()
        {
            return new RemoteClientInfo();
        }

        protected RemoteClientInfo()
        {
            var sHostName = Dns.GetHostName();

            var ipE = Dns.GetHostByName(sHostName);

            foreach (var address in ipE.AddressList)
            {
                var addr = address.ToString();
                if (addr.StartsWith("10.") || addr.StartsWith("192.168") || addr.StartsWith("172."))
                {
                    ipAddress = IPAddress.Parse(addr);
                }
            }

            username = Environment.UserName;
            var registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion");
            if (registryKey != null) osVersion = (string)registryKey.GetValue("productName");
        }

        public byte[] ToByteArray()
        {
            var n1 = Encoding.ASCII.GetBytes($"|{username}|");
            var n2 = Encoding.ASCII.GetBytes($"{osVersion}|");

            var list = new List<byte>();
            list.AddRange(ipAddress.GetAddressBytes());
            list.AddRange(n1);
            list.AddRange(n2);

            return list.ToArray();
        }


    }
}