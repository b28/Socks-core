using SocksTest.Extensions;
using System;
using System.Text;

namespace SocksTest.ConnectionEstablisher.Messages.Ntlmv1
{
    public class NtlmV1Message1
    {
        public string Header => "NTLMSSP";
        public byte ProtocolType => 0x01;
        public byte[] Align1 => new byte[] { 0, 0, 0 };
        public ushort Flags => 0xB203;
        public byte[] Align2 => new byte[] { 0, 0 };
        public ushort DomainNameLength1 => Convert.ToUInt16(DomainName.Length * 2);
        public ushort DomainNameLength2 => Convert.ToUInt16(DomainName.Length * 2);
        public ushort DomainNameOffset => Convert.ToUInt16(HostNameOffset + HostNameLength1);
        public byte[] Align3 => new byte[] { 0, 0 };
        public ushort HostNameLength1 => Convert.ToUInt16(HostName.Length * 2);
        public ushort HostNameLength2 => Convert.ToUInt16(HostName.Length * 2);
        public ushort HostNameOffset => 0x0020;
        public byte[] Align4 => new byte[] { 0, 0 };
        public string HostName;
        public string DomainName;

        public NtlmV1Message1(string hostName, string domainName)
        {
            if (string.IsNullOrEmpty(hostName))
                throw new ArgumentException("Hostname invalid");
            if (string.IsNullOrEmpty(domainName))
                throw new ArgumentException("Domain name invalid");

            HostName = hostName;
            DomainName = domainName;
        }
        public byte[] GetBytes()
        {
            int offset = 0;
            byte[] msg = new byte[HostNameOffset + HostNameLength1 + DomainNameLength1];
            Array.Copy(Encoding.ASCII.GetBytes(Header), msg, Header.Length);
            offset += Header.Length + 1;
            Array.Copy(new byte[] { ProtocolType }, 0, msg, offset, sizeof(byte));
            offset += sizeof(byte) + 3 * sizeof(byte);
            Array.Copy(Flags.ToArray(), 0, msg, offset, sizeof(ushort));
            offset += sizeof(ushort) + 2 * sizeof(byte);
            Array.Copy(DomainNameLength1.ToArray(), 0, msg, offset, sizeof(ushort));
            offset += sizeof(ushort);
            Array.Copy(DomainNameLength2.ToArray(), 0, msg, offset, sizeof(ushort));
            offset += sizeof(ushort);
            Array.Copy(DomainNameOffset.ToArray(), 0, msg, offset, sizeof(ushort));
            offset += sizeof(ushort) + 2 * sizeof(byte);
            Array.Copy(HostNameLength1.ToArray(), 0, msg, offset, sizeof(ushort));
            offset += sizeof(ushort);
            Array.Copy(HostNameLength2.ToArray(), 0, msg, offset, sizeof(ushort));
            offset += sizeof(ushort);
            Array.Copy(HostNameOffset.ToArray(), 0, msg, offset, sizeof(ushort));
            offset += sizeof(ushort) + 2 * sizeof(byte);
            Array.Copy(Encoding.Unicode.GetBytes(HostName), 0, msg, offset, HostNameLength1);
            offset += HostNameLength1;
            Array.Copy(Encoding.Unicode.GetBytes(DomainName), 0, msg, offset, DomainNameLength1);
            return msg;
        }
    }
}