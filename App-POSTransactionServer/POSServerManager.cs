using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Constant;
using Device;
using Interface;

namespace App_POSTransactionServer
{
    public partial class POSManager
    {
        private const String CgiLoadAllPOS = @"cgi-bin/posconfig?action=loadallpos";
        private const String CgiSaveAllPOS = @"cgi-bin/posconfig?action=saveallpos";

        private const String CgiLoadAllDivision = @"cgi-bin/posconfig?action=loaddivision";
        private const String CgiSaveAllDividion = @"cgi-bin/posconfig?action=savedivision";

        private const String CgiLoadAllRegion = @"cgi-bin/posconfig?action=loadregion";
        private const String CgiSaveAllRegion = @"cgi-bin/posconfig?action=saveregion";

        private const String CgiLoadAllStore = @"cgi-bin/posconfig?action=loadstore";
        private const String CgiSaveAllStore = @"cgi-bin/posconfig?action=savestore";



        public void LoadStore()
        {
            var pts = Server as IPTS;
            if (pts == null) return;

            //<Stores>
            //    <Store id="1">
            //        <Name></Name>
            //        <POSs>
            //            <POS id="1" />
            //            <POS id="2" />
            //         </POSs>
            //    </Store>
            //</Stores>

            StoreManager.Clear();

            var xmlDoc = Xml.LoadXmlFromHttp(CgiLoadAllStore, Server.Credential);
            //var xmlDoc = new XmlDocument();

            //xmlDoc.LoadXml("<Stores>" +
            //               "<Store id=\"1\"><Name>Stroe 1</Name><POSs><POS id=\"32767\" /></POSs></Store>" +
            //               "<Store id=\"2\"><Name>Stroe 2</Name><POSs><POS id=\"32768\" /></POSs></Store>" +
            //               "<Store id=\"3\"><Name>Stroe 3</Name><POSs><POS id=\"32769\" /></POSs></Store>" +
            //               "<Store id=\"4\"><Name>Stroe 4</Name><POSs><POS id=\"32770\" /></POSs></Store>" +
            //               "</Stores>");


            if (xmlDoc == null) return;

            XmlNodeList storeList = xmlDoc.GetElementsByTagName("Store");
            foreach (XmlElement storeNode in storeList)
            {
                var id = Convert.ToUInt16(storeNode.GetAttribute("id"));

                var store = new PTSStore
                {
                    Id = id,
                    Name = Xml.GetFirstElementValueByTagName(storeNode, "Name"),
                    ReadyState = ReadyState.Ready,
                    Pos = new Dictionary<String, IPOS>()
                };


                StoreManager[id] = store;

                var poss = Xml.GetFirstElementByTagName(storeNode, "POSs");
                XmlNodeList posList = poss.GetElementsByTagName("POS");

                foreach (XmlElement o in posList)
                {
                    var _id = o.GetAttribute("id");

                    foreach (IPOS pos in POSServer)
                    {
                        if (pos.Id == _id)
                        {
                            store.Pos[_id] = pos;
                            break;
                        }
                    }
                }
            }
        }

        private void SaveStore()
        {
            var orangeXmlDoc = Xml.LoadXmlFromHttp(CgiLoadAllStore, Server.Credential);

            var xmlDoc = new XmlDocument();

            var xmlRoot = xmlDoc.CreateElement("Stores");
            xmlDoc.AppendChild(xmlRoot);

            var sortResult = new List<IStore>(StoreManager.Values);
            sortResult.Sort((x, y) => (x.Id - y.Id));

            foreach (IStore store in sortResult)
            {
                var storeNode = xmlDoc.CreateElement("Store");
                storeNode.SetAttribute("id", store.Id.ToString());

                xmlRoot.AppendChild(storeNode);

                storeNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Name", store.Name));

                var POSsNode = xmlDoc.CreateElement("POSs");
                storeNode.AppendChild(POSsNode);

                var sortPos = new List<IPOS>(store.Pos.Values);
                sortPos.Sort((x, y) => x.Id.CompareTo(y.Id));

                foreach (IPOS pos in sortPos)
                {
                    var _contains = false;
                    foreach (IPOS pos1 in POSServer)
                    {
                        if (pos.Id == pos1.Id)
                        {
                            _contains = true;
                            break;
                        }
                    }

                    if (!_contains)
                        continue;

                    var posNode = xmlDoc.CreateElement("POS");
                    posNode.SetAttribute("id", pos.Id.ToString());
                    POSsNode.AppendChild(posNode);
                }
            }

            if (orangeXmlDoc == null)
                Xml.PostXmlToHttp(CgiSaveAllStore, xmlDoc, Server.Credential);
            else
            {
                if (!String.Equals(orangeXmlDoc.InnerXml, xmlDoc.InnerXml))
                    Xml.PostXmlToHttp(CgiSaveAllStore, xmlDoc, Server.Credential);
            }
        }

        public void LoadRegion()
        {
            var pts = Server as IPTS;
            if (pts == null) return;

            //<Regions>
            //    <Region id="1">
            //        <Name></Name>
            //        <Stores>
            //            <Store id="1" />
            //            <Store id="2" />
            //         </Stores>
            //    </Region>
            //</Regions>

            RegionManager.Clear();

            var xmlDoc = Xml.LoadXmlFromHttp(CgiLoadAllRegion, Server.Credential);
            //var xmlDoc = new XmlDocument();

            //xmlDoc.LoadXml("<Regions>" +
            //               "<Region id=\"1\"><Name>Region 1</Name><Stores><Store id=\"1\" /></Stores></Region>" +
            //               "<Region id=\"2\"><Name>Region 2</Name><Stores><Store id=\"2\" /></Stores></Region>" +
            //               "<Region id=\"3\"><Name>Region 3</Name><Stores><Store id=\"3\" /></Stores></Region>" +
            //               "<Region id=\"4\"><Name>Region 4</Name><Stores><Store id=\"4\" /></Stores></Region>" +
            //               "</Regions>");


            if (xmlDoc == null) return;

            XmlNodeList regionList = xmlDoc.GetElementsByTagName("Region");
            foreach (XmlElement regionNode in regionList)
            {
                var id = Convert.ToUInt16(regionNode.GetAttribute("id"));

                var region = new Region
                {
                    Id = id,
                    Name = Xml.GetFirstElementValueByTagName(regionNode, "Name"),
                    ReadyState = ReadyState.Ready,
                    Stores = new Dictionary<ushort, IStore>()
                };


                RegionManager[id] = region;

                var stores = Xml.GetFirstElementByTagName(regionNode, "Stores");
                XmlNodeList storeList = stores.GetElementsByTagName("Store");

                foreach (XmlElement o in storeList)
                {
                    var _id = Convert.ToUInt16(o.GetAttribute("id"));
                    if (StoreManager.ContainsKey(_id))
                    {
                        region.Stores[_id] = StoreManager[_id];
                    }
                }
            }
        }

        private void SaveRegion()
        {
            var orangeXmlDoc = Xml.LoadXmlFromHttp(CgiLoadAllRegion, Server.Credential);

            var xmlDoc = new XmlDocument();

            var xmlRoot = xmlDoc.CreateElement("Regions");
            xmlDoc.AppendChild(xmlRoot);

            var sortResult = new List<IRegion>(RegionManager.Values);
            sortResult.Sort((x, y) => (x.Id - y.Id));

            foreach (IRegion region in sortResult)
            {
                var regionNode = xmlDoc.CreateElement("Region");
                regionNode.SetAttribute("id", region.Id.ToString());

                xmlRoot.AppendChild(regionNode);

                regionNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Name", region.Name));

                var RegionsNode = xmlDoc.CreateElement("Stores");
                regionNode.AppendChild(RegionsNode);

                var sortStore = new List<IStore>(region.Stores.Values);
                sortStore.Sort((x, y) => (x.Id - y.Id));

                foreach (IStore store in sortStore)
                {
                    if (!StoreManager.ContainsKey(store.Id))
                        continue;

                    var storeNode = xmlDoc.CreateElement("Store");
                    storeNode.SetAttribute("id", store.Id.ToString());
                    RegionsNode.AppendChild(storeNode);
                }
            }

            if (orangeXmlDoc == null)
                Xml.PostXmlToHttp(CgiSaveAllRegion, xmlDoc, Server.Credential);
            else
            {
                if (!String.Equals(orangeXmlDoc.InnerXml, xmlDoc.InnerXml))
                    Xml.PostXmlToHttp(CgiSaveAllRegion, xmlDoc, Server.Credential);
            }
        }

        public void LoadDivision()
        {
            var pts = Server as IPTS;
            if (pts == null) return;

            //<Divisions>
            //    <Division id="1">
            //        <Name></Name>
            //        <Regions>
            //            <Region id="1" />
            //            <Region id="2" />
            //        </Regions>
            //    </Division>
            //</Divisions>

            DivisionManager.Clear();
            var xmlDoc = Xml.LoadXmlFromHttp(CgiLoadAllDivision, Server.Credential);
            //var xmlDoc = new XmlDocument();

            //xmlDoc.LoadXml("<Divisions>" +
            //               "<Division id=\"1\"><Name>Division 1</Name><Regions><Region id=\"1\" /></Regions></Division>" +
            //               "<Division id=\"2\"><Name>Division 2</Name><Regions><Region id=\"2\" /></Regions></Division>" +
            //               "<Division id=\"3\"><Name>Division 3</Name><Regions><Region id=\"3\" /></Regions></Division>" +
            //               "<Division id=\"4\"><Name>Division 4</Name><Regions><Region id=\"4\" /></Regions></Division>" +
            //               "</Divisions>");


            if (xmlDoc == null) return;

            XmlNodeList divisionList = xmlDoc.GetElementsByTagName("Division");
            foreach (XmlElement divisionNode in divisionList)
            {
                var id = Convert.ToUInt16(divisionNode.GetAttribute("id"));

                var division = new Division
                {
                    Id = id,
                    Name = Xml.GetFirstElementValueByTagName(divisionNode, "Name"),
                    ReadyState = ReadyState.Ready,
                    Regions = new Dictionary<ushort, IRegion>()
                };

                DivisionManager[id] = division;

                var regions = Xml.GetFirstElementByTagName(divisionNode, "Regions");
                XmlNodeList regionList = regions.GetElementsByTagName("Region");

                foreach (XmlElement o in regionList)
                {
                    var _id = Convert.ToUInt16(o.GetAttribute("id"));
                    if (RegionManager.ContainsKey(_id))
                    {
                        division.Regions[_id] = RegionManager[_id];
                    }
                }
            }
        }

        private void SaveDivision()
        {
            var orangeXmlDoc = Xml.LoadXmlFromHttp(CgiLoadAllDivision, Server.Credential);

            var xmlDoc = new XmlDocument();

            var xmlRoot = xmlDoc.CreateElement("Divisions");
            xmlDoc.AppendChild(xmlRoot);

            var sortResult = new List<IDivision>(DivisionManager.Values);
            sortResult.Sort((x, y) => (x.Id - y.Id));

            foreach (IDivision division in sortResult)
            {
                var divisionNode = xmlDoc.CreateElement("Division");
                divisionNode.SetAttribute("id", division.Id.ToString());

                xmlRoot.AppendChild(divisionNode);

                divisionNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Name", division.Name));

                var RegionsNode = xmlDoc.CreateElement("Regions");
                divisionNode.AppendChild(RegionsNode);

                var sortRegion = new List<IRegion>(division.Regions.Values);
                sortRegion.Sort((x, y) => (x.Id - y.Id));

                foreach (IRegion region in sortRegion)
                {
                    if (!RegionManager.ContainsKey(region.Id))
                        continue;

                    var regionNode = xmlDoc.CreateElement("Region");
                    regionNode.SetAttribute("id", region.Id.ToString());
                    RegionsNode.AppendChild(regionNode);
                }
            }

            if (orangeXmlDoc == null)
                Xml.PostXmlToHttp(CgiSaveAllDividion, xmlDoc, Server.Credential);
            else
            {
                if (!String.Equals(orangeXmlDoc.InnerXml, xmlDoc.InnerXml))
                    Xml.PostXmlToHttp(CgiSaveAllDividion, xmlDoc, Server.Credential);
            }
        }

        public void LoadPOS()
        {
            var pts = Server as IPTS;
            if (pts == null) return;

            //<AllPos>
            //<POSConfiguration id="1">
            //    <Name></Name>
            //    <POSRegisterID>3</POSRegisterID>
            //    <Manufacture>MaitreD</Manufacture>
            //    <Model></Model>
            //    <ExceptionConfig>1</ExceptionConfig>
            //    <KeywordConfig>1</KeywordConfig>
            //    <Items>1,2,3</Items>
            //</POSConfiguration>
            //</AllPos>

            POSServer.Clear();
            var xmlDoc = Xml.LoadXmlFromHttp(CgiLoadAllPOS, Server.Credential);

            if (xmlDoc == null) return;

            XmlNodeList configurationList = xmlDoc.GetElementsByTagName("POSConfiguration");
            foreach (XmlElement configurationNode in configurationList)
            {
                var pos = new POS
                {
                    LicenseId = Convert.ToUInt16(configurationNode.GetAttribute("id")),
                    Name = Xml.GetFirstElementValueByTagName(configurationNode, "Name"),
                    Id = Xml.GetFirstElementValueByTagName(configurationNode, "POSRegisterID"),
                    Manufacture = Xml.GetFirstElementValueByTagName(configurationNode, "Manufacture"),
                    IsCapture = Xml.GetFirstElementValueByTagName(configurationNode, "IsCapture") == "true",
                    Exception = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(configurationNode, "ExceptionConfig")),
                    Keyword = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(configurationNode, "KeywordConfig")),
                    ReadyState = ReadyState.Ready
                };

                String[] listId = Xml.GetFirstElementValueByTagName(configurationNode, "Items").Split(',');
                foreach (String str in listId)
                {
                    UInt16[] arr = str.Split(':').Select(s => String.Equals(s, "") ? (UInt16)0 : Convert.ToUInt16(s)).ToArray();
                    if (arr.Length != 2)
                        continue;

                    if (!pts.NVR.NVRs.ContainsKey(arr[0])) continue;

                    var device = new BasicDevice
                    {
                        Server = pts.NVR.NVRs[arr[0]],
                        Id = arr[1]
                    };

                    pos.Items.Add(device);
                }

                POSServer.Add(pos);
            }
        }
        private void SavePOS()
        {
            //var orgXmlDoc = Xml.LoadXmlFromHttp(CgiLoadAllPOS, Server.Credential);

            //XmlNodeList orgPOSNodeList = null;
            //if (orgXmlDoc != null)
            //    orgPOSNodeList = orgXmlDoc.GetElementsByTagName("POSConfiguration");

            var xmlDoc = new XmlDocument();
            XmlElement xmlRoot = xmlDoc.CreateElement("AllPos");
            xmlDoc.AppendChild(xmlRoot);

            foreach (IPOS pos in POSServer)
            {
                if (pos.ReadyState == ReadyState.Ready) continue;

                var posNode = ParsePOSToXml(pos);

                //var isChange = true;
                //if (orgPOSNodeList != null && posNode.FirstChild != null)
                //{
                //    foreach (XmlElement orgPOSNode in orgPOSNodeList)
                //    {
                //        var id = Convert.ToUInt16(orgPOSNode.GetAttribute("id"));
                //        if (id == obj.Key)
                //        {
                //            if (orgPOSNode.InnerXml == posNode.FirstChild.InnerXml)
                //                isChange = false;
                //            break;
                //        }
                //    }
                //}

                if (posNode.FirstChild != null)
                    xmlRoot.AppendChild(xmlDoc.ImportNode(posNode.FirstChild, true));

                pos.ReadyState = ReadyState.Ready;
            }

            if (xmlRoot.ChildNodes.Count > 0)
                Xml.PostXmlToHttp(CgiSaveAllPOS, xmlDoc, Server.Credential);
        }



    }
}
