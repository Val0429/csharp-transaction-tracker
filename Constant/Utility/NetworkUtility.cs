using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

namespace Constant.Utility
{
    public static class NetworkUtility
    {
        public static IEnumerable<IPAddress> GetLocalIPAddress()
        {
            // 取得本機名稱
            string strHostName = Dns.GetHostName();
            // 取得本機的IpHostEntry類別實體，用這個會提示已過時
            //IPHostEntry iphostentry = Dns.GetHostByName(strHostName);

            // 取得本機的IpHostEntry類別實體，MSDN建議新的用法
            IPHostEntry iphostentry = Dns.GetHostEntry(strHostName);

            // 取得所有 IP 位址, 只取得IP V4的Address
            return iphostentry.AddressList.Where(i => i.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
        }

        public static IEnumerable<PhysicalAddress> GetEthernetPhysicalAddress()
        {
            var macs = from nic in NetworkInterface.GetAllNetworkInterfaces()
                       where nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet
                       select nic.GetPhysicalAddress();

            return macs;
        }

        public static IEnumerable<NetworkInterface> GetEthernetInterface()
        {
            var macs = NetworkInterface.GetAllNetworkInterfaces()
                                       .Where(nic => nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet);

            return macs;
        }


        public static string ToFormatString(this PhysicalAddress physicalAddress, string delimiter)
        {
            if (physicalAddress == null) return null;

            var address = physicalAddress.GetAddressBytes().Select(b => b.ToString("X2")).ToArray();

            return string.Join(delimiter, address);
        }

        public static PhysicalAddress ToPhysicalAddress(string mac)
        {
            try
            {
                return PhysicalAddress.Parse(mac.ToUpperInvariant());
            }
            catch (FormatException ex)
            {
                Trace.WriteLine(ex.ToString());
            }

            return default(PhysicalAddress);
        }
    }
}
