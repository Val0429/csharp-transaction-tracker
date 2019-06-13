using System.Net;

namespace Constant
{
    public class Authentication : NetworkCredential
    {
        public uint OccupancyPriority = 0;
        public Encryption Encryption = Encryption.Basic; //Plain, Basic, Digest
    }
}