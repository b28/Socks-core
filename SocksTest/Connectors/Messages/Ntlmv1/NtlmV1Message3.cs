using System;
using System.Security.Cryptography;
using System.Text;
using SocksTest.Cryptography;
using SocksTest.Extensions;

namespace SocksTest.Connectors.Messages.Ntlmv1
{
    public class NtlmV1Message3
    {
        public string Header => "NTLMSSP";
        public byte ProtocolType => 0x03;
        public byte[] Align1 => new byte[] { 0, 0, 0 };

        public ushort LmResponseLength1 => 0x18;
        public ushort LmResponseLength2 => 0x18;
        public ushort LmResponseOffset => Convert.ToUInt16(HostOffset + HostName.Length * 2);
        public byte[] Align2 => new byte[] { 0, 0 };

        public ushort NtResponseLength1 => 0x18;
        public ushort NtResponseLength2 => 0x18;
        public ushort NtResponseOffset => Convert.ToUInt16(LmResponseOffset + LmResponse.Length);
        public byte[] Align3 => new byte[] { 0, 0 };

        public ushort DomainLength1 => Convert.ToUInt16(DomainName.Length * 2);
        public ushort DomainLength2 => Convert.ToUInt16(DomainName.Length * 2);
        public ushort DomainOffset => 0x40;
        public byte[] Align4 => new byte[] { 0, 0 };

        public ushort UserLength1 => Convert.ToUInt16(UserName.Length * 2);
        public ushort UserLength2 => Convert.ToUInt16(UserName.Length * 2);
        public ushort UserOffset => Convert.ToUInt16(DomainOffset + DomainName.Length * 2);
        public byte[] Align5 => new byte[] { 0, 0 };

        public ushort HostLength1 => Convert.ToUInt16(HostName.Length * 2);
        public ushort HostLength2 => Convert.ToUInt16(HostName.Length * 2);
        public ushort HostOffset => Convert.ToUInt16(UserOffset + UserName.Length * 2);
        public byte[] Align6 => new byte[] { 0, 0, 0, 0, 0, 0 };

        public ushort MessageLength => Convert.ToUInt16(64 + 2 * (DomainName.Length + UserName.Length + HostName.Length) + LmResponse.Length + NtResponse.Length);
        public byte[] Align7 => new byte[] { 0, 0 };

        public ushort Flags => 0x8201;
        public byte[] Align8 => new byte[] { 0, 0 };

        public string DomainName;
        public string UserName;
        public string HostName;
        public byte[] LmResponse = new byte[24];
        public byte[] NtResponse = new byte[24];

        public NtlmV1Message3(string userName, string userPassword, string hostName, string domainName, byte[] challengeMassage2)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentException("Username invalid");
            if (string.IsNullOrEmpty(userPassword))
                throw new ArgumentException("Password invalid");
            if (string.IsNullOrEmpty(hostName))
                throw new ArgumentException("Hostname invalid");
            if (string.IsNullOrEmpty(domainName))
                throw new ArgumentException("Domain name invalid");
            UserName = userName;
            HostName = hostName.ToUpper();
            DomainName = domainName.ToUpper();
            CreateLmResponse(userPassword, challengeMassage2).CopyTo(LmResponse, 0);
            CreateNtResponse(userPassword, challengeMassage2).CopyTo(NtResponse, 0);
        }

        public byte[] DesEncrypt(byte[] key, byte[] inputData)
        {
            var iv = new byte[8];
            var desKey = new byte[8];
            key.CopyTo(desKey, 0);
            var binkey = new bool[8 * 8];
            var isByteEven = 0;
            var bitNumber = 0;

            for (var i = 0; i < 7; i++)
            {
                for (byte indexBit = 0x80; indexBit > 0; indexBit /= 2)
                {
                    binkey[bitNumber] = ((desKey[i] & indexBit) > 0);
                    if (binkey[bitNumber])
                        isByteEven++;
                    bitNumber++;
                    if ((bitNumber % 8 == 7) & (bitNumber > 0))
                    {
                        binkey[bitNumber] = (isByteEven % 2 == 0);
                        bitNumber++;
                        isByteEven = 0;
                    }
                }
            }

            for (var i = 0; i < 8; i++)
            {
                desKey[i] = 0;
                for (var j = 0; j < 8; j++)
                    desKey[i] += (byte)((binkey[8 * i + j] ? 1 : 0) * Math.Pow(2, 7 - j));
            }

            using (var des = DES.Create())
            {
                des.Mode = CipherMode.ECB;
                des.Padding = PaddingMode.None;
                des.BlockSize = 64;

                using (var encryptor = des.CreateEncryptor(desKey, iv))
                {
                    return encryptor.TransformFinalBlock(inputData, 0, inputData.Length);
                }
            }
        }

        private string GetModifyUserPassword(string userPassword)
        {
            const int indexPasswordLength = 14;
            if (userPassword.Length > indexPasswordLength)
                userPassword = UserName.Substring(0, indexPasswordLength);
            return userPassword;
        }

        private byte[] CreateLmResponse(string userPassword, byte[] challengeServer)
        {
            var prepareUserPassword = new byte[14];
            Encoding.ASCII.GetBytes(GetModifyUserPassword(userPassword).ToUpper()).CopyTo(prepareUserPassword, 0);
            var magicChallenge = Encoding.ASCII.GetBytes("KGS!@#$%");
            var buff = new byte[7];
            var lmHashedPassword = new byte[21];
            for (var i = 0; i < prepareUserPassword.Length / 7; i++)
            {
                Buffer.BlockCopy(prepareUserPassword, i * 7, buff, 0, 7);
                DesEncrypt(buff, magicChallenge).CopyTo(lmHashedPassword, i * 8);
                //                var tttt = Encoding.ASCII.GetChars(lmHashedPassword);
            }
            var buffResponse = new byte[24];
            for (var i = 0; i < 3; i++)
            {
                Buffer.BlockCopy(lmHashedPassword, i * 7, buff, 0, 7);
                DesEncrypt(buff, challengeServer).CopyTo(buffResponse, i * 8);
            }
            return buffResponse;
        }

        private byte[] CreateNtResponse(string userPassword, byte[] challengeServer)
        {
            var ntHashedPassword = new byte[21];
            new Md4().GetByteHashFromBytes(Encoding.Unicode.GetBytes(GetModifyUserPassword(userPassword))).CopyTo(ntHashedPassword, 0);
            var buff = new byte[7];
            var buffResponse = new byte[24];
            for (var i = 0; i < 3; i++)
            {
                Buffer.BlockCopy(ntHashedPassword, i * 7, buff, 0, 7);
                DesEncrypt(buff, challengeServer).CopyTo(buffResponse, i * 8);
            }
            return buffResponse;
        }

        private static int SetBytesToMessage(byte[] message, int offset, byte[] sourceData, int sizeData)
        {
            Array.Copy(sourceData, 0, message, offset, sizeData);
            offset += sizeData;
            return offset;
        }

        public byte[] GetBytes()
        {
            var offset = 0;
            var msg = new byte[MessageLength];

            offset = SetBytesToMessage(msg, offset, Encoding.ASCII.GetBytes(Header), Header.Length) + 1; // + endline symbol
            offset = SetBytesToMessage(msg, offset, new byte[] { ProtocolType }, sizeof(byte)); //ProtocolType size 1 byte
            offset = SetBytesToMessage(msg, offset, Align1, Align1.Length);

            offset = SetBytesToMessage(msg, offset, LmResponseLength1.ToArray(), LmResponseLength1.GetSizeType());
            offset = SetBytesToMessage(msg, offset, LmResponseLength2.ToArray(), LmResponseLength2.GetSizeType());
            offset = SetBytesToMessage(msg, offset, LmResponseOffset.ToArray(), LmResponseOffset.GetSizeType());
            offset = SetBytesToMessage(msg, offset, Align2, Align2.Length);

            offset = SetBytesToMessage(msg, offset, NtResponseLength1.ToArray(), NtResponseLength1.GetSizeType());
            offset = SetBytesToMessage(msg, offset, NtResponseLength2.ToArray(), NtResponseLength2.GetSizeType());
            offset = SetBytesToMessage(msg, offset, NtResponseOffset.ToArray(), NtResponseOffset.GetSizeType());
            offset = SetBytesToMessage(msg, offset, Align3, Align3.Length);

            offset = SetBytesToMessage(msg, offset, DomainLength1.ToArray(), DomainLength1.GetSizeType());
            offset = SetBytesToMessage(msg, offset, DomainLength2.ToArray(), DomainLength2.GetSizeType());
            offset = SetBytesToMessage(msg, offset, DomainOffset.ToArray(), DomainOffset.GetSizeType());
            offset = SetBytesToMessage(msg, offset, Align4, Align4.Length);

            offset = SetBytesToMessage(msg, offset, UserLength1.ToArray(), UserLength1.GetSizeType());
            offset = SetBytesToMessage(msg, offset, UserLength2.ToArray(), UserLength2.GetSizeType());
            offset = SetBytesToMessage(msg, offset, UserOffset.ToArray(), UserOffset.GetSizeType());
            offset = SetBytesToMessage(msg, offset, Align5, Align5.Length);

            offset = SetBytesToMessage(msg, offset, HostLength1.ToArray(), HostLength1.GetSizeType());
            offset = SetBytesToMessage(msg, offset, HostLength2.ToArray(), HostLength2.GetSizeType());
            offset = SetBytesToMessage(msg, offset, HostOffset.ToArray(), HostOffset.GetSizeType());
            offset = SetBytesToMessage(msg, offset, Align6, Align6.Length);

            offset = SetBytesToMessage(msg, offset, MessageLength.ToArray(), MessageLength.GetSizeType());
            offset = SetBytesToMessage(msg, offset, Align7, Align7.Length);

            offset = SetBytesToMessage(msg, offset, Flags.ToArray(), Flags.GetSizeType());
            offset = SetBytesToMessage(msg, offset, Align8, Align8.Length);

            offset = SetBytesToMessage(msg, offset, Encoding.Unicode.GetBytes(DomainName), DomainLength1);
            offset = SetBytesToMessage(msg, offset, Encoding.Unicode.GetBytes(UserName), UserLength1);
            offset = SetBytesToMessage(msg, offset, Encoding.Unicode.GetBytes(HostName), HostLength1);
            offset = SetBytesToMessage(msg, offset, LmResponse, LmResponse.Length);
            SetBytesToMessage(msg, offset, NtResponse, NtResponse.Length);

            return msg;
        }
    }
}