using System;
using System.Collections.Generic;

namespace DeviceConstant
{
    public enum ConnectionProtocol : ushort
    {
        NonSpecific = 0,
        Tcp = 1,
        Http = 2,
        RtspOverUdp = 3,
        RtspOverTcp = 4,
        RtspOverHttp = 5,
        Multicast = 6,
        Https = 7
    }
    public static class ConnectionProtocols
    {
        public static ConnectionProtocol ToIndex(String value)
        {
            foreach (KeyValuePair<ConnectionProtocol, String> keyValuePair in List)
            {
                if (String.Equals(value, keyValuePair.Value))
                    return keyValuePair.Key;
            }

            foreach (KeyValuePair<ConnectionProtocol, String> keyValuePair in OldNamingList)
            {
                if (String.Equals(value, keyValuePair.Value))
                    return keyValuePair.Key;
            }

            return ConnectionProtocol.NonSpecific;
        }

        public static ConnectionProtocol DisplayStringToIndex(String value)
        {
            foreach (KeyValuePair<ConnectionProtocol, String> keyValuePair in DisplayList)
                if (String.Equals(value, keyValuePair.Value))
                    return keyValuePair.Key;

            return ConnectionProtocol.NonSpecific;
        }

        public static String ToString(ConnectionProtocol index)
        {
            foreach (KeyValuePair<ConnectionProtocol, String> keyValuePair in List)
            {
                if (index == keyValuePair.Key)
                    return keyValuePair.Value;
            }

            return "";
        }

        public static String ToDisplayString(ConnectionProtocol index)
        {
            foreach (KeyValuePair<ConnectionProtocol, String> keyValuePair in DisplayList)
            {
                if (index == keyValuePair.Key)
                    return keyValuePair.Value;
            }

            return "";
        }

        public static readonly Dictionary<ConnectionProtocol, String> DisplayList = new Dictionary<ConnectionProtocol, String>
                                                             {
                                                                 { ConnectionProtocol.NonSpecific, "" },
                                                                 { ConnectionProtocol.Tcp, "TCP" },
                                                                 { ConnectionProtocol.Http, "HTTP" },
                                                                 { ConnectionProtocol.RtspOverUdp, "RTSP" },
                                                                 { ConnectionProtocol.RtspOverTcp, "RTSP / TCP" },
                                                                 { ConnectionProtocol.RtspOverHttp, "RTSP / HTTP" },
                                                                 { ConnectionProtocol.Multicast, "Multicast" },
                                                                 { ConnectionProtocol.Https, "HTTPS" },
                                                             };

        public static readonly Dictionary<ConnectionProtocol, String> List = new Dictionary<ConnectionProtocol, String>
                                                             {
                                                                 { ConnectionProtocol.NonSpecific, "" },
                                                                 { ConnectionProtocol.Tcp, "TCP" },
                                                                 { ConnectionProtocol.Http, "HTTP" },
                                                                 { ConnectionProtocol.RtspOverUdp, "RTSP" },
                                                                 { ConnectionProtocol.RtspOverTcp, "RTSPTCP" },
                                                                 { ConnectionProtocol.RtspOverHttp, "RTSPHTTP" },
                                                                 { ConnectionProtocol.Multicast, "Multicast" },
                                                                 { ConnectionProtocol.Https, "HTTPS" },
                                                             };

        public static readonly Dictionary<ConnectionProtocol, String> OldNamingList = new Dictionary<ConnectionProtocol, String>
                                                             {
                                                                 { ConnectionProtocol.RtspOverUdp, "RTSP Over UDP" },
                                                                 { ConnectionProtocol.RtspOverTcp, "RTSP Over TCP" },
                                                                 { ConnectionProtocol.RtspOverHttp, "RTSP Over HTTP" },
                                                             };
        
    }
}