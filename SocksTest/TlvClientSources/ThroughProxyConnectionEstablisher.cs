using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SocksTest.Connectors;
using SocksTest.Connectors.Messages.Ntlmv1;

namespace SocksTest.TlvClientSources
{
    public class ThroughProxyConnectionEstablisher
    {
        public event EventHandler<string> DebugMessage;
        //private TcpClient connection = new TcpClient();
        private ProxyAuthInfo proxyAuthInfo;
        private ProxyEndPoint proxyEndPoint;

        public ThroughProxyConnectionEstablisher(ProxyEndPoint proxyConnect, ProxyAuthInfo proxyAuth)
        {
            proxyEndPoint = proxyConnect;
            proxyAuthInfo = proxyAuth;
        }

        private static string PrepareRequest(string connectTo, string ntlmBase64Data)
        {
            if (string.IsNullOrEmpty(connectTo))
                throw new ArgumentNullException(nameof(connectTo));

            var res = $"CONNECT {connectTo} HTTP/1.1\r\n" +
                        "User-Agent: Mozilla / 5.0(compatible; MSIE 9.0; Windows NT 6.1; Trident / 5.0)\r\n" +
                        "Pragma: no-cache\r\n" +
                        (string.IsNullOrEmpty(ntlmBase64Data) ? string.Empty : $"Proxy-Authorization: NTLM {ntlmBase64Data}\r\n") +
                        "\r\n";
            return res;
        }

        private static byte[] GetServerChallenge(string serverAnswer)
        {
            var ntlmChallengeMarker = "Proxy-Authenticate: NTLM ";
            if (!serverAnswer.Contains(ntlmChallengeMarker))
                throw new IncorrectMessage2Responce("Server response is wrong (Message type-2)");

            var base64Challenge = "";
            foreach (var line in serverAnswer.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (line.Contains(ntlmChallengeMarker))
                {
                    var ss = line.ToArray();
                    base64Challenge = new string(
                        ss.Skip(ntlmChallengeMarker.Length).TakeWhile(a => a != 0).ToArray());
                }
            }
            if (string.IsNullOrEmpty(base64Challenge))
                throw new IncorrectServerChallenge("\nBase64 string from server is null\n");

            return NtlmV1Message2.NonceFromChallenge(base64Challenge).Content;
        }

        private static void CheckAuthorizationType(string response)
        {
            if (!(response.ToLower().Contains("ntlm\r\n")))
            {
                throw new IncorrectProxyAuthorization("\nProxy server doesn't support NTLMv1 authorization\n");
            }
        }

        private string GetResponseFromServer(Stream activeConnection, string request)
        {
            var requestBytes = Encoding.ASCII.GetBytes(request);
            activeConnection.Write(requestBytes, 0, requestBytes.Length);
            var buff = new byte[16 * 1024];
            DebugMessage?.Invoke(this, "\nRequest send\n");
            var responseLength = activeConnection.Read(buff, 0, buff.Length);
            DebugMessage?.Invoke(this, "\nResponse got\n");
            return Encoding.ASCII.GetString(buff, 0, responseLength);
        }

        private bool CheckConnectionEstablisher(string response)
        {
            //response.Split('\r', '\n');
            var errorServiceUnavailable = "503 Service Unavailable";
            var connectionEstablished = "200 connection established";
            if (response.ToLower().Contains(errorServiceUnavailable.ToLower()))
                throw new IncorrectProxyAuthorization($"\nAuthorization is wrong. {errorServiceUnavailable}\n");
            if (response.ToLower().Contains(connectionEstablished.ToLower())) // how to check???!
                return true;
            throw new IncorrectProxyAuthorization("\nAuthorization is wrong. Check credentials\n");
        }


        public TcpClient NtlmAuth(ProxyEndPoint connectToInfo)
        {
            if (string.IsNullOrEmpty(proxyAuthInfo.UserName))
                throw new ArgumentException("User name is empty");
            if (string.IsNullOrEmpty(proxyAuthInfo.UserPassword))
                throw new ArgumentException("User's password is empty");
            if (string.IsNullOrEmpty(proxyAuthInfo.WorkstationName))
                throw new ArgumentException($"{nameof(proxyAuthInfo.WorkstationName)} name is empty");
            if (string.IsNullOrEmpty(proxyAuthInfo.DomainName))
                throw new ArgumentException($"{nameof(proxyAuthInfo.DomainName)} is empty");
            if (string.IsNullOrEmpty(proxyEndPoint.IpAddress))
                throw new ArgumentException("Proxy address is empty");
            if ((connectToInfo.Port < 1) || (connectToInfo.Port > 65535))
                throw new ArgumentException("Destination port connection is wrong");
            if ((proxyEndPoint.Port < 1) || (proxyEndPoint.Port > 65535))
                throw new ArgumentException("Proxy port is wrong");

            proxyAuthInfo.UserName = proxyAuthInfo.UserName.ToUpper();
            proxyAuthInfo.DomainName = proxyAuthInfo.DomainName.ToUpper();
            DebugMessage?.Invoke(this, "Start connection to proxyConnect server\n");

            var connection = new TcpClient();
            try
            {
                connection.Connect(IPAddress.Parse(proxyEndPoint.IpAddress), proxyEndPoint.Port);
            }
            catch (Exception e)
            {
                DebugMessage?.Invoke(this, "\nError connection to proxyConnect (TCP Client)\n");
                throw new SocketConnectionException(e.Message, e.InnerException);
            }
            var proxyStream = connection.GetStream();

            var connectResource = connectToInfo.IpAddress + ":" + connectToInfo.Port;

            DebugMessage?.Invoke(this, "\nConnection to proxyConnect successful\n");
            // Message 0 C -> S
            var serverRequest = PrepareRequest(connectResource, string.Empty);
            var serverResponse = GetResponseFromServer(proxyStream, serverRequest);
            DebugMessage?.Invoke(this, $"\nFirst response :\n {serverResponse}\n");
            CheckAuthorizationType(serverResponse);

            connection.Close();
            // connection closed by proxyConnect server!
            connection = new TcpClient();

            try
            {
                connection.Connect(IPAddress.Parse(proxyEndPoint.IpAddress), proxyEndPoint.Port);
                DebugMessage?.Invoke(this, "\nConnection close by proxyConnect. Reconnecting...\n");
            }
            catch (Exception e)
            {
                DebugMessage?.Invoke(this, $"\nError second connection to proxyConnect (TCP Client). {e.Message}\n");
                return null;
            }

            proxyStream = connection.GetStream();

            // Message 1 C -> S
            var ntlmV1Msg1 = new NtlmV1Message1(proxyAuthInfo.WorkstationName/*proxyAuthInfo.UserName*/, proxyAuthInfo.DomainName);
            serverRequest = PrepareRequest(connectResource, Convert.ToBase64String(ntlmV1Msg1.GetBytes()));
            DebugMessage?.Invoke(this, $"\nMessage-1 (C->S) request :\n{serverRequest}\n");
            // Message 2 S -> C
            serverResponse = GetResponseFromServer(proxyStream, serverRequest);
            DebugMessage?.Invoke(this, $"\nMessage-2 (S->C) response :\n{serverResponse}\n");
            var serverChallenge = GetServerChallenge(serverResponse);
            DebugMessage?.Invoke(this, $"\nChallenge : {BitConverter.ToString(serverChallenge)}\n");

            // Message 3 C -> S
            var ntlmV1Msg3 = new NtlmV1Message3(proxyAuthInfo.UserName, proxyAuthInfo.UserPassword, proxyAuthInfo.WorkstationName, proxyAuthInfo.DomainName, serverChallenge);
            var test = Convert.ToBase64String(ntlmV1Msg3.GetBytes());
            serverRequest = PrepareRequest(connectResource, Convert.ToBase64String(ntlmV1Msg3.GetBytes()));
            DebugMessage?.Invoke(this, $"\nMessage-3 (C->S) request :\n{serverRequest}\n");
            serverResponse = GetResponseFromServer(proxyStream, serverRequest);
            DebugMessage?.Invoke(this, $"\nMessage-3 (S->C) response :\n{serverResponse}\n");

            return CheckConnectionEstablisher(serverResponse) ? connection : null;
        }

        public TcpClient Connect(IPEndPoint ipEndPoint)
        {
            var proxy = new ProxyEndPoint { IpAddress = ipEndPoint.Address.ToString(), Port = ipEndPoint.Port };
            return NtlmAuth(proxy);
        }
    }
}