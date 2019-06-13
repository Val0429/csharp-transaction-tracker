using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Constant;
using Device;
using Interface;

namespace App_POSTransactionServer
{
    public partial class POSManager
    {
        private const String CgiLoadAllConnection = @"cgi-bin/posconfig?action=loadallconnection";
        private const String CgiSaveAllConnection = @"cgi-bin/posconfig?action=saveallconnection";
        private const String CgiLoadConnectionGroup = @"cgi-bin/posconfig?action=loadconnectiongroup";
        private const String CgiSaveConnectionGroup = @"cgi-bin/posconfig?action=saveconnectiongroup";

        public Dictionary<UInt16, IPOSConnection> Connections { get; protected set; }

        
        public void LoadPOSConnection()
        {
            //<AllConnections>"
            //<ConnectionConfiguration id="1">
            //    <Manufacture>MaitreD</Manufacture>
            //    <Model></Model>
            //    <Name></Name>
            //    <Protocol name="MaitreD Back Office">PNP_T1</Protocol>
            //    <QueueInfo>queue://stores.sales.raw.Security</QueueInfo>
            //    <IPAddress>0.0.0.0</IPAddress>
            //    <Port>
            //        <Accept>8081</Accept>
            //        <Connect></Connect>
            //    </Port>
            //    <Authentication>
            //        <Account></Account>
            //        <Password></Password>
            //        <Encryption></Encryption>
            //    </Authentication>
            //    <POSConfig>
            //        <POS channel="1" pos="3" exception="1"/>
            //        <POS channel="2" pos="1" exception="1"/>
            //        <POS channel="3" pos="2" exception="1"/>
            //        <POS channel="4" pos="4" exception="1"/>
            //    </POSConfig>
            //</ConnectionConfiguration>
            //</AllConnections>

            Connections.Clear();
            XmlDocument xmlDoc = Xml.LoadXmlFromHttp(CgiLoadAllConnection, Server.Credential);

            if (xmlDoc == null) return;

            XmlNodeList configurationList = xmlDoc.GetElementsByTagName("ConnectionConfiguration");
            foreach (XmlElement configurationNode in configurationList)
            {
                var posConnection = new POSConnection
                                        {
                                            Id = Convert.ToUInt16(configurationNode.GetAttribute("id")),
                                            Name = Xml.GetFirstElementValueByTagName(configurationNode, "Name"),
                                            Manufacture = Xml.GetFirstElementValueByTagName(configurationNode, "Manufacture"),
                                            IsCapture = Xml.GetFirstElementValueByTagName(configurationNode, "IsCapture") == "true",
                                            Authentication =
                                                {
                                                    Domain = Xml.GetFirstElementValueByTagName(configurationNode, "IPAddress"),
                                                    UserName = Xml.GetFirstElementValueByTagName(configurationNode, "Account"),
                                                    Password = Xml.GetFirstElementValueByTagName(configurationNode, "Password"),
                                                    Encryption = Encryptions.ToIndex(Xml.GetFirstElementValueByTagName(configurationNode, "Encryption"))
                                                },
                                        };
                //posConnection.SetDefaultAuthentication();

                var protocol = Xml.GetFirstElementByTagName(configurationNode, "Protocol");
                posConnection.Protocol = protocol.GetAttribute("name");

                if (posConnection.Manufacture == "Generic")
                {
                    var proto = ""; 
                    switch (protocol.InnerText)
                    {
                        case "PNP_T12": proto = "Multicast"; break;
                        case "PNP_T11": proto = "UDP"; break;
                        case "PNP_T9": proto = "TCP"; break;
                    }

                    posConnection.Protocol = proto ;
                }
                else
                {
                    if (protocol.InnerText == "PNP_Demo")
                        posConnection.Manufacture = "Oracle Demo";
                }
   
                posConnection.QueueInfo = Xml.GetFirstElementValueByTagName(configurationNode, "QueueInfo");
                posConnection.ConnectInfo = Xml.GetFirstElementValueByTagName(configurationNode, "ConnectInfo");

                var acceptPort = Xml.GetFirstElementValueByTagName(configurationNode, "Accept");
                posConnection.AcceptPort = String.IsNullOrEmpty(acceptPort) ? (UInt16)0 : Convert.ToUInt16(acceptPort);

                var connectPort = Xml.GetFirstElementValueByTagName(configurationNode, "Connect");
                posConnection.ConnectPort = String.IsNullOrEmpty(connectPort) ? (UInt16)0 : Convert.ToUInt16(connectPort);

                //-------------------------------------------------------------------------
                XmlNode possNode = configurationNode.SelectSingleNode("POSConfig");
                if (possNode != null)
                {
                    foreach (XmlElement posNode in possNode)
                    {
                        var id = Convert.ToUInt16(posNode.GetAttribute("channel"));
                        var pos = FindPOSByLicenseId(id);
                        if (pos != null)
                            posConnection.POS.Add(pos);
                    }
                    //-------------------------------------------------------------------------
                }

                posConnection.ReadyState = ReadyState.Ready;
                Connections.Add(posConnection.Id, posConnection);
            }
        }

        

        private void SaveConnection()
        {
            var orgXmlDoc = Xml.LoadXmlFromHttp(CgiLoadAllConnection, Server.Credential);

            XmlNodeList orgConnectionNodeList = null;
            if (orgXmlDoc != null)
                orgConnectionNodeList = orgXmlDoc.GetElementsByTagName("ConnectionConfiguration");

            var xmlDoc = new XmlDocument();
            XmlElement xmlRoot = xmlDoc.CreateElement("AllConnections");
            xmlDoc.AppendChild(xmlRoot);

            foreach (KeyValuePair<UInt16, IPOSConnection> obj in Connections)
            {
                //if (obj.Value.ReadyState == ReadyState.Ready) continue;

                XmlDocument connectionNode = ParseConnectionToXml(obj.Value);

                var isChange = true;
                if (orgConnectionNodeList != null && connectionNode.FirstChild != null)
                {
                    foreach (XmlElement orgConnectionNode in orgConnectionNodeList)
                    {
                        var id = Convert.ToUInt16(orgConnectionNode.GetAttribute("id"));
                        if (id == obj.Key)
                        {
                            if (orgConnectionNode.InnerXml == connectionNode.FirstChild.InnerXml)
                                isChange = false;
                            break;
                        }
                    }
                }

                if (isChange && connectionNode.FirstChild != null)
                    xmlRoot.AppendChild(xmlDoc.ImportNode(connectionNode.FirstChild, true));

                obj.Value.ReadyState = ReadyState.Ready;
            }

            if (xmlRoot.ChildNodes.Count > 0)
                Xml.PostXmlToHttp(CgiSaveAllConnection, xmlDoc, Server.Credential);
        }

        private void SaveConnectionGroup()
        {
            var orangeXmlDoc = Xml.LoadXmlFromHttp(CgiLoadConnectionGroup, Server.Credential);

            var xmlDoc = new XmlDocument();

            var xmlRoot = xmlDoc.CreateElement("Groups");
            xmlDoc.AppendChild(xmlRoot);

            var sortResult = new List<IPOSConnection>(Connections.Values);
            sortResult.Sort((x, y) => (x.Id - y.Id));

            IEnumerable<String> temp = sortResult.Select(connection => (connection.Id != 0) ? connection.Id.ToString() : "");

            var groupNode = xmlDoc.CreateElement("Group");
            groupNode.SetAttribute("id", "0");
            groupNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Items", String.Join(",", temp.ToArray())));
            xmlRoot.AppendChild(groupNode);

            if (orangeXmlDoc != null && !String.Equals(orangeXmlDoc.InnerXml, xmlDoc.InnerXml))
                Xml.PostXmlToHttp(CgiSaveConnectionGroup, xmlDoc, Server.Credential);
        }

        private XmlDocument ParseConnectionToXml(IPOSConnection connection)
        {
            var xmlDoc = new XmlDocument();

            var xmlRoot = xmlDoc.CreateElement("ConnectionConfiguration");
            xmlRoot.SetAttribute("id", connection.Id.ToString());
            xmlDoc.AppendChild(xmlRoot);

            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Name", connection.Name));

            var manufacture = connection.Manufacture;
            if (manufacture == "Oracle Demo")
                manufacture = "Oracle";
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Manufacture", manufacture));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Model", connection.Model));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "IsCapture", connection.IsCapture ? "true" : "false"));
            //-------------------------------------------------------------------------
            if(manufacture == "Generic")
            {
                var proto = "";

                switch (connection.Protocol)
                {
                    case "Multicast":   proto = "PNP_T12";  break;
                    case "UDP": proto = "PNP_T11"; break;
                    case "TCP": proto = "PNP_T9"; break;
                }

                var protocolNode = Xml.CreateXmlElementWithText(xmlDoc, "Protocol", proto);
                protocolNode.SetAttribute("name", "Back Office");
                xmlRoot.AppendChild(protocolNode);
            }
            else
            {
                var protocolNode = Xml.CreateXmlElementWithText(xmlDoc, "Protocol", connection.ProtocolValue(connection.Manufacture));
                protocolNode.SetAttribute("name", connection.Protocol);
                xmlRoot.AppendChild(protocolNode);
            }
            
            //-------------------------------------------------------------------------
            var queueInfoNode = Xml.CreateXmlElementWithText(xmlDoc, "QueueInfo", connection.QueueInfo);
            xmlRoot.AppendChild(queueInfoNode);
            //-------------------------------------------------------------------------
            var connectInfoDNode = Xml.CreateXmlElementWithText(xmlDoc, "ConnectInfo", connection.ConnectInfo);
            xmlRoot.AppendChild(connectInfoDNode);
            //-------------------------------------------------------------------------
            var authenticationNode = xmlDoc.CreateElement("Authentication");
            xmlRoot.AppendChild(authenticationNode);

            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "IPAddress", connection.Authentication.Domain));
            authenticationNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Account", connection.Authentication.UserName));
            authenticationNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Password", connection.Authentication.Password));
            authenticationNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Encryption", Encryptions.ToString(connection.Authentication.Encryption)));

            //-------------------------------------------------------------------------
            var portNode = xmlDoc.CreateElement("Port");
            xmlRoot.AppendChild(portNode);
            portNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Accept", connection.AcceptPort));
            portNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Connect", connection.ConnectPort));

            //-------------------------------------------------------------------------
            var posConfigNode = xmlDoc.CreateElement("POSConfig");
            xmlRoot.AppendChild(posConfigNode);

            connection.POS.Sort((x, y) => (x.Id.CompareTo(y.Id)));

            foreach (var pos in connection.POS)
            {
                var posNode = xmlDoc.CreateElement("POS");
                posNode.SetAttribute("channel", pos.LicenseId.ToString());
                //JO need this
                posNode.SetAttribute("pos", pos.Id.ToString());
                posNode.SetAttribute("exception", pos.Exception.ToString());
                posNode.SetAttribute("keyword", pos.Keyword.ToString());

                posConfigNode.AppendChild(posNode);
            }

            return xmlDoc;
        }

        public UInt16 GetNewConnectionId(Boolean reverse)
        {
            UInt16 max = Server.License.Amount; //65535

            if (reverse)
            {
                for (UInt16 id = max; id >= 1; id--)
                {
                    if (Connections.ContainsKey(id)) continue;
                    return id;
                }
            }
            else
            {
                for (UInt16 id = 1; id <= max; id++)
                {
                    if (Connections.ContainsKey(id)) continue;
                    return id;
                }
            }
            
            return 0;
        }
    }
}
