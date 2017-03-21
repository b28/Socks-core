using System;
using System.Linq;

namespace SocksTest.ConnectionEstablisher.Messages.Ntlmv1
{
    public class NtlmV1ServerChallenge
    {
        public byte[] Content;
        public NtlmV1ServerChallenge(byte[] src)
        {
            if (src.Length != 8 || src == null)
                throw new IncorrectServerChallenge("Invalid Challenge size");
            var test = new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 };
            Content = src;//test; //src;
        }
    }

    public static class NtlmV1Message2
    {
        public static NtlmV1ServerChallenge NonceFromChallenge(string base64EncodedData)
        {
            var decodeData = Convert.FromBase64String(base64EncodedData);
            return new NtlmV1ServerChallenge(Convert.FromBase64String(base64EncodedData).Skip(24).Take(8).ToArray());
        }
    }
}