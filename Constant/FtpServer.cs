using System;
using System.Net;
namespace Constant
{
    public class FtpServer
    {
        public NetworkCredential Credential;
        public String Directory;
        public UInt16 Port = 21;

        public FtpServer()
        {
            Credential = new NetworkCredential();
        }
    }
}
