using System;
using System.Net;

namespace Constant
{
    public class ServerCredential : NetworkCredential
    {
        public ServerCredential() { }

        public ServerCredential(string account, string password, string domain, ushort port, bool ssl)
            : this(account, password, domain, port, ssl, true)
        {

        }

        public ServerCredential(string account, string password, string domain, ushort port, bool ssl, bool rememberMe)
        {
            Domain = domain;
            UserName = account;
            Password = password;
            Port = port;
            SSLEnable = ssl;
            RememberMe = rememberMe;
        }


        public UInt16 Port { get; set; }
        public Boolean SSLEnable;
        public Boolean RememberMe;
        //public String ClientMode;
    }
}