using System;
using System.Net;

namespace Constant
{
    public class MailServer
    {
        public NetworkCredential Credential;
        public String Sender;
        public String MailAddress;
        public SecurityType Security = SecurityType.PLAIN;
        public UInt16 Port = 25;

        public MailServer()
        {
            Credential = new NetworkCredential();
        }
    }

    public enum SecurityType : ushort
    {
        PLAIN = 25,
        SSL = 465,
        TLS = 587,
    }
}
