using System;
using System.Collections.Generic;
using Constant;
using Interface;

namespace Device
{
    public class POSConnection : IPOSConnection
    {
        public String Manufacture { get; set; }
        public String Model { get; set; }
        public UInt16 Id { get; set; }
        public String Name { get; set; }
        public String Protocol { get; set; }
        public String QueueInfo { get; set; }
        public String ConnectInfo { get; set; }
        public UInt16 AcceptPort { get; set; }
        public UInt16 ConnectPort { get; set; }
        public Boolean IsCapture { get; set; }
        public Authentication Authentication { get; set; }
        public List<IPOS> POS { get; set; }

        public ReadyState ReadyState { get; set; }

        public POSConnection()
        {
            AcceptPort = 0;// 8081;
            ConnectPort = 0;
            Authentication = new Authentication
            {
                Domain = "0.0.0.0",
            };
            POS = new List<IPOS>();
            IsCapture = false;
            ReadyState = ReadyState.New;
        }

        public void SetDefaultAuthentication()
        {
            switch (Manufacture)
            {
                case "ActiveMQ":
                    Authentication.Domain = "127.0.0.1";
                    ConnectPort = 61616;
                    QueueInfo = "queue://stores.sales.raw.Security";
                    break;

                case "Oracle":
                case "Oracle Demo":
                    Authentication.Domain = "0.0.0.0";
                    ConnectPort = 61616;
                    QueueInfo = "RTLOGQUEUE";
                    break;

                default:
                    Authentication.Domain = "0.0.0.0";
                    ConnectPort = 0;
                    QueueInfo = "";
                    break;
            }
        }

        public override String ToString()
        {
            return Id.ToString().PadLeft(2, '0') + " " + Name;
        }

        public String ProtocolValue(String manufacture)
        {
            switch (manufacture)
            {
                case "MaitreD":
                    return "PNP_T1";

                case "Retalix":
                    return "PNP_T2";

                case "Radiant":
                    return "PNP_T3";

                case "Micros":
                    return "PNP_T4";

                case "ActiveMQ":
                    return "PNP_T5";

                case "NEC":
                    return "PNP_T6";

                case "Oracle":
                    return "PNP_T7";

                case "Oracle Demo":
                    return "PNP_Demo";

                case "everrich":
                    return "PNP_T8";

                case "BHG":
                    return "PNP_T10";
            }

            return "";
        }
        
        //----------------------------------------------------------------------------------------
        public static List<String> ProtocolList = new List<String>
        {
            "Back Office",
        };
    }
}
