using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml;
using Constant;

namespace ServerProfile
{
    public partial class POSManager
    {
        private const String CgiLoadAllException = @"cgi-bin/posconfig?action=loadallexception";
        private const String CgiSaveAllException = @"cgi-bin/posconfig?action=saveallexception";
        private const String CgiLoadExceptionGroup = @"cgi-bin/posconfig?action=loadexceptiongroup";
        private const String CgiSaveExceptionGroup = @"cgi-bin/posconfig?action=saveexceptiongroup";

        public Dictionary<UInt16, POS_Exception> Exceptions { get; protected set; }
        public Dictionary<String, POS_Exception.ExceptionThreshold> ExceptionThreshold { get; protected set; }

        private delegate void LoadExceptionDelegate();
        public void LoadException()
        {
            //<AllExceptions>"
            //<ExceptionConfiguration id="1">
            //    <Manufacture>Retalix</Manufacture>
            //    <Name>Exception 1</Name>
            //    <Worker>4</Worker>
            //    <Buffer>512</Buffer>
            //    <Exceptions>
            //        <Exception tag="VOID" dir="++">VOID</Exception>
            //        <Exception tag="CLEAR" dir="--">CLEAR</Exception>
            //        <Exception tag="LESS" dir="+">LESS</Exception>
            //        <Exception tag="COUPON" dir="=">Coupon</Exception>
            //        <Exception tag="NO SALE">NO SALE</Exception>
            //    </Exceptions>
            //    <Segments>
            //        <Segment tag="ID">,</Segment>
            //        <Segment tag="BEGIN">--Begin--</Segment>
            //        <Segment tag="END">--End--</Segment>
            //    </Segments>
            //    <Tags>
            //        <Tag tag="VISA">VISA</Tag>
            //        <Tag tag="MASTER">MASTER CARD</Tag>
            //        <Tag tag="CASH">CASH</Tag>
            //        <Tag tag="CHECK">CHECK</Tag>
            //        <Tag tag="TOTAL">TOTAL</Tag>
            //        <Tag tag="TABLE">TABLE</Tag>
            //        <Tag tag="ORDER">ORDER</Tag>
            //    </Tags>
            //</ExceptionConfiguration>
            //</AllExceptions>

            Exceptions.Clear();
            XmlDocument xmlDoc = Xml.LoadXmlFromHttp(CgiLoadAllException, Server.Credential);

            if (xmlDoc == null) return;

            var configurationList = xmlDoc.GetElementsByTagName("ExceptionConfiguration");
            foreach (XmlElement configurationNode in configurationList)
            {
                var exception = new POS_Exception
                                    {
                                        ReadyState = ReadyState.Ready,
                                        Id = Convert.ToUInt16(configurationNode.GetAttribute("id")),
                                        Name = Xml.GetFirstElementValueByTagName(configurationNode, "Name"),
                                        Manufacture = Xml.GetFirstElementValueByTagName(configurationNode, "Manufacture"),
                                        Worker = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(configurationNode, "Worker")),
                                        Buffer = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(configurationNode, "Buffer")),
                                    };
                
                
                Boolean editable = true;
                switch (exception.Manufacture)
                {
                    case "Retalix":
                    case "Radiant":
                    case "Micros":
                    case "Menards":
                        editable = false;
                        break;
                }
                //-------------------------------------------------------------------------
                XmlNode exceptionsNode = configurationNode.SelectSingleNode("Exceptions");
                if (exceptionsNode != null)
                {
                    foreach (XmlElement exceptionNode in exceptionsNode.ChildNodes)
                    {
                        var editableValue = exceptionNode.GetAttribute("editable");
                        var valueType = exceptionNode.GetAttribute("valuetype");
                        exception.Exceptions.Add(new POS_Exception.Exception
                                                     {
                                                         Key = exceptionNode.GetAttribute("tag"),
                                                         Dir = exceptionNode.GetAttribute("dir"),
                                                         Value = exceptionNode.InnerText,
                                                         Editable = String.IsNullOrEmpty(editableValue) ? editable : (editableValue == "true"),
                                                         ValueType = String.IsNullOrEmpty(valueType) ? "" : valueType,
                                                     });
                    }

                    exception.Exceptions.Sort((x, y) => (y.Key.CompareTo(x.Key)));
                }
                //-------------------------------------------------------------------------
                XmlNode segmentsNode = configurationNode.SelectSingleNode("Segments");
                if (segmentsNode != null)
                {
                    foreach (XmlElement segmentNode in segmentsNode.ChildNodes)
                    {
                        exception.Segments.Add(new POS_Exception.Segment
                                                   {
                                                       Key = segmentNode.GetAttribute("tag"),
                                                       Value = segmentNode.InnerText,
                                                       Editable = editable
                                                   });
                    }
                }
                //-------------------------------------------------------------------------
                XmlNode tagsNode = configurationNode.SelectSingleNode("Tags");
                if (tagsNode != null)
                {
                    foreach (XmlElement tagNode in tagsNode)
                    {
                        var dir = tagNode.GetAttribute("dir");
                        var valueType = tagNode.GetAttribute("valuetype");
                        exception.Tags.Add(new POS_Exception.Tag
                                               {
                                                   Key = tagNode.GetAttribute("tag"),
                                                   Value = tagNode.InnerText,
                                                   Editable = editable,
                                                   Dir = String.IsNullOrEmpty(dir) ? "" : dir,
                                                   ValueType = String.IsNullOrEmpty(valueType) ? "" : valueType
                                               });
                    }

                    exception.Tags.Sort((x, y) => (y.Key.CompareTo(x.Key)));
                }
                //-------------------------------------------------------------------------

                Exceptions.Add(exception.Id, exception);
            }
        }

        private delegate void LoadExceptionGroupDelegate();
        public void LoadExceptionGroup()
        {
            //<Exceptions>
            //    <Exception tag="VOID" threshold1="5" threshold2="10" color ="255,255,255" />
            //    <Exception tag="15DISCOUNT" threshold1="5" threshold2="10" color ="128,0,0" />
            //</Exceptions>

            //list ALL exception
            ExceptionThreshold.Clear();
            var manufactures = POS_Exception.Manufactures;

            //var List
            foreach (var manufacture in manufactures)
            {
                var exception = new POS_Exception
                {
                    Manufacture = manufacture
                };

                POS_Exception.SetDefaultExceptions(exception);
                foreach (var posException in exception.Exceptions)
                {
                    if (ExceptionThreshold.ContainsKey(posException.Key)) continue;

                    var color = (_predefineColorList.Count > 0) ? _predefineColorList[0] : Color.White;
                    _predefineColorList.Remove(color);

                    ExceptionThreshold.Add(posException.Key, new POS_Exception.ExceptionThreshold
                                                                 {
                                                                     ThresholdValue1 = 5,
                                                                     ThresholdValue2 = 20,
                                                                     Color = color,
                                                                 });
                }
            }
            
            var xmlDoc = Xml.LoadXmlFromHttp(CgiLoadExceptionGroup, Server.Credential);
            if (xmlDoc == null) return;
            
            var exceptionThresholdNodeList = xmlDoc.GetElementsByTagName("Exception");

            foreach (XmlElement exceptionThresholdNodeL in exceptionThresholdNodeList)
            {
                var tag = exceptionThresholdNodeL.GetAttribute("tag");

                if (!ExceptionThreshold.ContainsKey(tag)) continue;

                ExceptionThreshold[tag].ThresholdValue1 = Convert.ToInt32(exceptionThresholdNodeL.GetAttribute("threshold1"));
                ExceptionThreshold[tag].ThresholdValue2 = Convert.ToInt32(exceptionThresholdNodeL.GetAttribute("threshold2"));
                    
                var color = Array.ConvertAll(exceptionThresholdNodeL.GetAttribute("color").Split(','), Convert.ToInt32);

                ExceptionThreshold[tag].Color = Color.FromArgb(color[0], color[1], color[2]);
            }
        }

        private void LoadExceptionCallback(IAsyncResult result)
        {
            LoadExceptionGroupDelegate loadExceptionGroupDelegate = LoadExceptionGroup;
            loadExceptionGroupDelegate.BeginInvoke(LoadExceptionGroupCallback, loadExceptionGroupDelegate);
        }

        private void LoadExceptionGroupCallback(IAsyncResult result)
        {
            LoadPOSDelegate loadPOSDelegate = LoadPOS;
            loadPOSDelegate.BeginInvoke(LoadPOSCallback, loadPOSDelegate);
        }

        private void SaveException()
        {
            var orgXmlDoc = Xml.LoadXmlFromHttp(CgiLoadAllException, Server.Credential);

            XmlNodeList orgExceptionNodeList = null;
            if (orgXmlDoc != null)
                orgExceptionNodeList = orgXmlDoc.GetElementsByTagName("ExceptionConfiguration");
            
            var xmlDoc = new XmlDocument();
            var xmlRoot = xmlDoc.CreateElement("AllExceptions");
            xmlDoc.AppendChild(xmlRoot);

            foreach (KeyValuePair<UInt16, POS_Exception> obj in Exceptions)
            {
                XmlDocument exceptionNode = ParseExceptionToXml(obj.Value);

                var isChange = true;
                if (orgExceptionNodeList != null && exceptionNode.FirstChild != null)
                {
                    foreach (XmlElement orgExceptionNode in orgExceptionNodeList)
                    {
                        var id = Convert.ToUInt16(orgExceptionNode.GetAttribute("id"));
                        if (id == obj.Key)
                        {
                            if (orgExceptionNode.InnerXml == exceptionNode.FirstChild.InnerXml)
                                isChange = false;
                            break;
                        }
                    }
                }

                if (isChange && exceptionNode.FirstChild != null)
                    xmlRoot.AppendChild(xmlDoc.ImportNode(exceptionNode.FirstChild, true));
            }

            if (xmlRoot.ChildNodes.Count > 0)
                Xml.PostXmlToHttp(CgiSaveAllException, xmlDoc, Server.Credential);

            SaveExceptionGroup();
        }

        private void SaveExceptionGroup()
        {
            //<Exceptions>
            //    <Exception tag="VOID" threshold1="5" threshold2="10" color ="255,255,255" />
            //    <Exception tag="15DISCOUNT" threshold1="5" threshold2="10" color ="128,0,0" />
            //</Exceptions>

            var orangeXmlDoc = Xml.LoadXmlFromHttp(CgiLoadExceptionGroup, Server.Credential);

            var xmlDoc = new XmlDocument();

            var xmlRoot = xmlDoc.CreateElement("Groups");
            xmlDoc.AppendChild(xmlRoot);

            var sortResult = new List<POS_Exception>(Exceptions.Values);
            sortResult.Sort((x, y) => (x.Id - y.Id));

            IEnumerable<String> temp = sortResult.Select(exception => (exception.Id != 0) ? exception.Id.ToString() : "");

            var groupNode = xmlDoc.CreateElement("Group");
            groupNode.SetAttribute("id", "0");
            groupNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Items", String.Join(",", temp.ToArray())));
            xmlRoot.AppendChild(groupNode);

            var exceptionsNode = xmlDoc.CreateElement("Exceptions");
            xmlRoot.AppendChild(exceptionsNode);
            
            foreach (var exceptionThreshold in ExceptionThreshold)
            {
                var hasValue = false;
                foreach (var obj in Exceptions)
                {
                    foreach (var exception in obj.Value.Exceptions)
                    {
                        if (exception.Key == exceptionThreshold.Key)
                        {
                            hasValue = true;
                            break;
                        }
                    }
                }

                if(!hasValue) continue;

                var exceptionNode = xmlDoc.CreateElement("Exception");

                exceptionNode.SetAttribute("tag", exceptionThreshold.Key);
                exceptionNode.SetAttribute("threshold1", exceptionThreshold.Value.ThresholdValue1.ToString());
                exceptionNode.SetAttribute("threshold2", exceptionThreshold.Value.ThresholdValue2.ToString());
                exceptionNode.SetAttribute("color", exceptionThreshold.Value.Color.R+","+exceptionThreshold.Value.Color.G+","+exceptionThreshold.Value.Color.B);

                exceptionsNode.AppendChild(exceptionNode);
            }

            if (orangeXmlDoc != null && !String.Equals(orangeXmlDoc.InnerXml, xmlDoc.InnerXml))
                Xml.PostXmlToHttp(CgiSaveExceptionGroup, xmlDoc, Server.Credential);
        }

        private XmlDocument ParseExceptionToXml(POS_Exception posException)
        {
            var xmlDoc = new XmlDocument();
            
            posException.ReadyState = ReadyState.Ready;

            var xmlRoot = xmlDoc.CreateElement("ExceptionConfiguration");
            xmlRoot.SetAttribute("id", posException.Id.ToString());
            xmlDoc.AppendChild(xmlRoot);

            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Manufacture", posException.Manufacture));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Name", posException.Name));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Worker", posException.Worker));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Buffer", posException.Buffer));

            //-------------------------------------------------------------------------
            var exceptionsRoot = xmlDoc.CreateElement("Exceptions");
            xmlRoot.AppendChild(exceptionsRoot);
            foreach (var exception in posException.Exceptions)
            {
                var exceptionNode = xmlDoc.CreateElement("Exception");
                exceptionNode.SetAttribute("tag", exception.Key);
                exceptionNode.SetAttribute("dir", exception.Dir);
                exceptionNode.SetAttribute("editable", exception.Editable ? "true" : "false");
                exceptionNode.SetAttribute("valuetype", exception.ValueType);
                exceptionNode.InnerText = exception.Value ?? "";

                exceptionsRoot.AppendChild(exceptionNode);
            }

            //-------------------------------------------------------------------------
            var segmentsRoot = xmlDoc.CreateElement("Segments");
            xmlRoot.AppendChild(segmentsRoot);
            foreach (var segment in posException.Segments)
            {
                var segmentNode = xmlDoc.CreateElement("Segment");
                segmentNode.SetAttribute("tag", segment.Key);
                segmentNode.SetAttribute("editable", segment.Editable ? "true" : "false");
                segmentNode.InnerText = segment.Value ?? "";

                segmentsRoot.AppendChild(segmentNode);
            }

            //-------------------------------------------------------------------------
            var tagsRoot = xmlDoc.CreateElement("Tags");
            xmlRoot.AppendChild(tagsRoot);
            foreach (var tag in posException.Tags)
            {
                var tagNode = xmlDoc.CreateElement("Tag");
                tagNode.SetAttribute("tag", tag.Key);
                tagNode.SetAttribute("dir", tag.Dir);
                tagNode.SetAttribute("editable", tag.Editable ? "true" : "false");
                tagNode.SetAttribute("valuetype", tag.ValueType);
                tagNode.InnerText = tag.Value ?? "";

                tagsRoot.AppendChild(tagNode);
            }

            return xmlDoc;
        }

        public UInt16 GetNewExceptionId()
        {
            UInt16 max = Server.License.Amount; //65535
            for (UInt16 id = 1; id <= max; id++)
            {
                if (Exceptions.ContainsKey(id)) continue;
                return id;
            }

            return 0;
        }

        private readonly List<Color> _predefineColorList = new List<Color>
        {
            ColorTranslator.FromHtml("#A8BB19"),
            ColorTranslator.FromHtml("#7CB9E8"),
            ColorTranslator.FromHtml("#C9FFE5"),
            ColorTranslator.FromHtml("#B284BE"),
            ColorTranslator.FromHtml("#5D8AA8"),
            ColorTranslator.FromHtml("#00308F"),
            ColorTranslator.FromHtml("#72A0C1"),
            ColorTranslator.FromHtml("#AF002A"),
            ColorTranslator.FromHtml("#F0F8FF"),
            ColorTranslator.FromHtml("#E32636"),
            ColorTranslator.FromHtml("#C46210"),
            ColorTranslator.FromHtml("#EFDECD"),
            ColorTranslator.FromHtml("#E52B50"),
            ColorTranslator.FromHtml("#D69CBB"),
            ColorTranslator.FromHtml("#3B7A57"),
            ColorTranslator.FromHtml("#FFBF00"),
            ColorTranslator.FromHtml("#FF7E00"),
            ColorTranslator.FromHtml("#FF033E"),
            ColorTranslator.FromHtml("#9966CC"),
            ColorTranslator.FromHtml("#A4C639"),
            ColorTranslator.FromHtml("#F2F3F4"),
            ColorTranslator.FromHtml("#CD9575"),
            ColorTranslator.FromHtml("#665D1E"),
            ColorTranslator.FromHtml("#915C83"),
            ColorTranslator.FromHtml("#841B2D"),
            ColorTranslator.FromHtml("#FAEBD7"),
            ColorTranslator.FromHtml("#008000"),
            ColorTranslator.FromHtml("#8DB600"),
            ColorTranslator.FromHtml("#FBCEB1"),
            ColorTranslator.FromHtml("#00FFFF"),
            ColorTranslator.FromHtml("#7FFFD4"),
            ColorTranslator.FromHtml("#4B5320"),
            ColorTranslator.FromHtml("#3B444B"),
            ColorTranslator.FromHtml("#8F9779"),
            ColorTranslator.FromHtml("#E9D66B"),
            ColorTranslator.FromHtml("#B2BEB5"),
            ColorTranslator.FromHtml("#87A96B"),
            ColorTranslator.FromHtml("#FF9966"),
            ColorTranslator.FromHtml("#A52A2A"),
            ColorTranslator.FromHtml("#FDEE00"),
            ColorTranslator.FromHtml("#6E7F80"),
            ColorTranslator.FromHtml("#568203"),
            ColorTranslator.FromHtml("#FF2052"),
            ColorTranslator.FromHtml("#007FFF"),
            ColorTranslator.FromHtml("#F0FFFF"),
            ColorTranslator.FromHtml("#89CFF0"),
            ColorTranslator.FromHtml("#A1CAF1"),
            ColorTranslator.FromHtml("#F4C2C2"),
            ColorTranslator.FromHtml("#FEFEFA"),
            ColorTranslator.FromHtml("#FF91AF"),
            ColorTranslator.FromHtml("#21ABCD"),
            ColorTranslator.FromHtml("#FAE7B5"),
            ColorTranslator.FromHtml("#FFE135"),
            ColorTranslator.FromHtml("#E0218A"),
            ColorTranslator.FromHtml("#7C0A02"),
            ColorTranslator.FromHtml("#848482"),
            ColorTranslator.FromHtml("#98777B"),
            ColorTranslator.FromHtml("#BCD4E6"),
            ColorTranslator.FromHtml("#9F8170"),
            ColorTranslator.FromHtml("#F5F5DC"),
            ColorTranslator.FromHtml("#2E5894"),
            ColorTranslator.FromHtml("#9C2542"),
            ColorTranslator.FromHtml("#FFE4C4"),
            ColorTranslator.FromHtml("#3D2B1F"),
            ColorTranslator.FromHtml("#967117"),
            ColorTranslator.FromHtml("#CAE00D"),
            ColorTranslator.FromHtml("#648C11"),
            ColorTranslator.FromHtml("#FE6F5E"),
            ColorTranslator.FromHtml("#BF4F51"),
            ColorTranslator.FromHtml("#000000"),
            ColorTranslator.FromHtml("#3D0C02"),
            ColorTranslator.FromHtml("#253529"),
            ColorTranslator.FromHtml("#3B3C36"),
            ColorTranslator.FromHtml("#FFEBCD"),
            ColorTranslator.FromHtml("#A57164"),
            ColorTranslator.FromHtml("#318CE7"),
            ColorTranslator.FromHtml("#ACE5EE"),
            ColorTranslator.FromHtml("#FAF0BE"),
            ColorTranslator.FromHtml("#0000FF"),
            ColorTranslator.FromHtml("#1F75FE"),
            ColorTranslator.FromHtml("#0093AF"),
            ColorTranslator.FromHtml("#0087BD"),
            ColorTranslator.FromHtml("#333399"),
            ColorTranslator.FromHtml("#0247FE"),
            ColorTranslator.FromHtml("#A2A2D0"),
            ColorTranslator.FromHtml("#6699CC"),
            ColorTranslator.FromHtml("#0D98BA"),
            ColorTranslator.FromHtml("#126180"),
            ColorTranslator.FromHtml("#8A2BE2"),
            ColorTranslator.FromHtml("#5072A7"),
            ColorTranslator.FromHtml("#4F86F7"),
            ColorTranslator.FromHtml("#1C1CF0"),
            ColorTranslator.FromHtml("#DE5D83"),
            ColorTranslator.FromHtml("#79443B"),
            ColorTranslator.FromHtml("#0095B6"),
            ColorTranslator.FromHtml("#E3DAC9"),
            ColorTranslator.FromHtml("#CC0000"),
            ColorTranslator.FromHtml("#006A4E"),
            ColorTranslator.FromHtml("#873260"),
            ColorTranslator.FromHtml("#0070FF"),
            ColorTranslator.FromHtml("#B5A642"),
            ColorTranslator.FromHtml("#CB4154"),
            ColorTranslator.FromHtml("#1DACD6"),
            ColorTranslator.FromHtml("#66FF00"),
            ColorTranslator.FromHtml("#BF94E4"),
            ColorTranslator.FromHtml("#C32148"),
            ColorTranslator.FromHtml("#1974D2"),
            ColorTranslator.FromHtml("#FF007F"),
            ColorTranslator.FromHtml("#08E8DE"),
            ColorTranslator.FromHtml("#D19FE8"),
            ColorTranslator.FromHtml("#F4BBFF"),
            ColorTranslator.FromHtml("#FF55A3"),
            ColorTranslator.FromHtml("#FB607F"),
            ColorTranslator.FromHtml("#004225"),
            ColorTranslator.FromHtml("#CD7F32"),
            ColorTranslator.FromHtml("#737000"),
            ColorTranslator.FromHtml("#964B00"),
            ColorTranslator.FromHtml("#A52A2A"),
            ColorTranslator.FromHtml("#664423"),
            ColorTranslator.FromHtml("#1B4D3E"),
            ColorTranslator.FromHtml("#FFC1CC"),
            ColorTranslator.FromHtml("#E7FEFF"),
            ColorTranslator.FromHtml("#F0DC82"),
            ColorTranslator.FromHtml("#7BB661"),
            ColorTranslator.FromHtml("#480607"),
            ColorTranslator.FromHtml("#800020"),
            ColorTranslator.FromHtml("#DEB887"),
            ColorTranslator.FromHtml("#CC5500"),
            ColorTranslator.FromHtml("#E97451"),
            ColorTranslator.FromHtml("#8A3324"),
            ColorTranslator.FromHtml("#BD33A4"),
            ColorTranslator.FromHtml("#702963"),
            ColorTranslator.FromHtml("#536872"),
            ColorTranslator.FromHtml("#5F9EA0"),
            ColorTranslator.FromHtml("#91A3B0"),
            ColorTranslator.FromHtml("#006B3C"),
            ColorTranslator.FromHtml("#ED872D"),
            ColorTranslator.FromHtml("#E30022"),
            ColorTranslator.FromHtml("#FFF600"),
            ColorTranslator.FromHtml("#A67B5B"),
            ColorTranslator.FromHtml("#4B3621"),
            ColorTranslator.FromHtml("#1E4D2B"),
            ColorTranslator.FromHtml("#A3C1AD"),
            ColorTranslator.FromHtml("#C19A6B"),
            ColorTranslator.FromHtml("#EFBBCC"),
            ColorTranslator.FromHtml("#78866B"),
            ColorTranslator.FromHtml("#FFEF00"),
            ColorTranslator.FromHtml("#FF0800"),
            ColorTranslator.FromHtml("#E4717A"),
            ColorTranslator.FromHtml("#00BFFF"),
            ColorTranslator.FromHtml("#592720"),
            ColorTranslator.FromHtml("#C41E3A"),
            ColorTranslator.FromHtml("#00CC99"),
            ColorTranslator.FromHtml("#960018"),
            ColorTranslator.FromHtml("#D70040"),
            ColorTranslator.FromHtml("#EB4C42"),
            ColorTranslator.FromHtml("#FF0038"),
            ColorTranslator.FromHtml("#FFA6C9"),
            ColorTranslator.FromHtml("#B31B1B"),
            ColorTranslator.FromHtml("#99BADD"),
            ColorTranslator.FromHtml("#ED9121"),
            ColorTranslator.FromHtml("#00563F"),
            ColorTranslator.FromHtml("#062A78"),
            ColorTranslator.FromHtml("#703642"),
            ColorTranslator.FromHtml("#C95A49"),
            ColorTranslator.FromHtml("#92A1CF"),
            ColorTranslator.FromHtml("#ACE1AF"),
            ColorTranslator.FromHtml("#007BA7"),
            ColorTranslator.FromHtml("#2F847C"),
            ColorTranslator.FromHtml("#B2FFFF"),
            ColorTranslator.FromHtml("#4997D0"),
            ColorTranslator.FromHtml("#DE3163"),
            ColorTranslator.FromHtml("#EC3B83"),
            ColorTranslator.FromHtml("#007BA7"),
            ColorTranslator.FromHtml("#2A52BE"),
            ColorTranslator.FromHtml("#6D9BC3"),
            ColorTranslator.FromHtml("#007AA5"),
            ColorTranslator.FromHtml("#E03C31"),
            ColorTranslator.FromHtml("#A0785A"),
            ColorTranslator.FromHtml("#F7E7CE"),
            ColorTranslator.FromHtml("#36454F"),
            ColorTranslator.FromHtml("#232B2B"),
            ColorTranslator.FromHtml("#E68FAC"),
            ColorTranslator.FromHtml("#DFFF00"),
            ColorTranslator.FromHtml("#7FFF00"),
            ColorTranslator.FromHtml("#DE3163"),
            ColorTranslator.FromHtml("#FFB7C5"),
            ColorTranslator.FromHtml("#904535"),
            ColorTranslator.FromHtml("#DE6FA1"),
            ColorTranslator.FromHtml("#A8516E"),
            ColorTranslator.FromHtml("#AA381E"),
            ColorTranslator.FromHtml("#856088"),
            ColorTranslator.FromHtml("#7B3F00"),
            ColorTranslator.FromHtml("#D2691E"),
            ColorTranslator.FromHtml("#FFA700"),
            ColorTranslator.FromHtml("#98817B"),
            ColorTranslator.FromHtml("#E34234"),
            ColorTranslator.FromHtml("#D2691E"),
            ColorTranslator.FromHtml("#E4D00A"),
            ColorTranslator.FromHtml("#9EA91F"),
            ColorTranslator.FromHtml("#7F1734"),
            ColorTranslator.FromHtml("#FBCCE7"),
            ColorTranslator.FromHtml("#030507"),
            ColorTranslator.FromHtml("#0047AB"),
            ColorTranslator.FromHtml("#D2691E"),
            ColorTranslator.FromHtml("#965A3E"),
            ColorTranslator.FromHtml("#6F4E37"),
            ColorTranslator.FromHtml("#9BDDFF"),
            ColorTranslator.FromHtml("#F88379"),
            ColorTranslator.FromHtml("#002E63"),
            ColorTranslator.FromHtml("#8C92AC"),
            ColorTranslator.FromHtml("#B87333"),
            ColorTranslator.FromHtml("#DA8A67"),
            ColorTranslator.FromHtml("#AD6F69"),
            ColorTranslator.FromHtml("#CB6D51"),
            ColorTranslator.FromHtml("#996666"),
            ColorTranslator.FromHtml("#FF3800"),
            ColorTranslator.FromHtml("#FF7F50"),
            ColorTranslator.FromHtml("#F88379"),
            ColorTranslator.FromHtml("#FF4040"),
            ColorTranslator.FromHtml("#893F45"),
            ColorTranslator.FromHtml("#FBEC5D"),
            ColorTranslator.FromHtml("#B31B1B"),
            ColorTranslator.FromHtml("#6495ED"),
            ColorTranslator.FromHtml("#FFF8DC"),
            ColorTranslator.FromHtml("#FFF8E7"),
            ColorTranslator.FromHtml("#FFBCD9"),
            ColorTranslator.FromHtml("#FFFDD0"),
            ColorTranslator.FromHtml("#DC143C"),
            ColorTranslator.FromHtml("#BE0032"),
            ColorTranslator.FromHtml("#00FFFF"),
            ColorTranslator.FromHtml("#00B7EB"),
            ColorTranslator.FromHtml("#58427C"),
            ColorTranslator.FromHtml("#FFD300"),
            ColorTranslator.FromHtml("#FFFF31"),
            ColorTranslator.FromHtml("#F0E130"),
            ColorTranslator.FromHtml("#00008B"),
            ColorTranslator.FromHtml("#666699"),
            ColorTranslator.FromHtml("#654321"),
            ColorTranslator.FromHtml("#5D3954"),
            ColorTranslator.FromHtml("#A40000"),
            ColorTranslator.FromHtml("#08457E"),
            ColorTranslator.FromHtml("#986960"),
            ColorTranslator.FromHtml("#CD5B45"),
            ColorTranslator.FromHtml("#008B8B"),
            ColorTranslator.FromHtml("#536878"),
            ColorTranslator.FromHtml("#B8860B"),
            ColorTranslator.FromHtml("#A9A9A9"),
            ColorTranslator.FromHtml("#013220"),
            ColorTranslator.FromHtml("#00416A"),
            ColorTranslator.FromHtml("#1A2421"),
            ColorTranslator.FromHtml("#BDB76B"),
            ColorTranslator.FromHtml("#483C32"),
            ColorTranslator.FromHtml("#734F96"),
            ColorTranslator.FromHtml("#534B4F"),
            ColorTranslator.FromHtml("#543D37"),
            ColorTranslator.FromHtml("#8B008B"),
            ColorTranslator.FromHtml("#003366"),
            ColorTranslator.FromHtml("#4A5D23"),
            ColorTranslator.FromHtml("#556B2F"),
            ColorTranslator.FromHtml("#FF8C00"),
            ColorTranslator.FromHtml("#9932CC"),
            ColorTranslator.FromHtml("#779ECB"),
            ColorTranslator.FromHtml("#03C03C"),
            ColorTranslator.FromHtml("#966FD6"),
            ColorTranslator.FromHtml("#C23B22"),
            ColorTranslator.FromHtml("#E75480"),
            ColorTranslator.FromHtml("#003399"),
            ColorTranslator.FromHtml("#4F3A3C"),
            ColorTranslator.FromHtml("#872657"),
            ColorTranslator.FromHtml("#8B0000"),
            ColorTranslator.FromHtml("#E9967A"),
            ColorTranslator.FromHtml("#560319"),
            ColorTranslator.FromHtml("#8FBC8F"),
            ColorTranslator.FromHtml("#3C1414"),
            ColorTranslator.FromHtml("#8CBED6"),
            ColorTranslator.FromHtml("#483D8B"),
            ColorTranslator.FromHtml("#2F4F4F"),
            ColorTranslator.FromHtml("#177245"),
            ColorTranslator.FromHtml("#918151"),
            ColorTranslator.FromHtml("#FFA812"),
            ColorTranslator.FromHtml("#483C32"),
            ColorTranslator.FromHtml("#CC4E5C"),
            ColorTranslator.FromHtml("#00CED1"),
            ColorTranslator.FromHtml("#D1BEA8"),
            ColorTranslator.FromHtml("#9400D3"),
            ColorTranslator.FromHtml("#9B870C"),
            ColorTranslator.FromHtml("#00703C"),
            ColorTranslator.FromHtml("#555555"),
            ColorTranslator.FromHtml("#D70A53"),
            ColorTranslator.FromHtml("#A9203E"),
            ColorTranslator.FromHtml("#EF3038"),
            ColorTranslator.FromHtml("#E9692C"),
            ColorTranslator.FromHtml("#DA3287"),
            ColorTranslator.FromHtml("#FAD6A5"),
            ColorTranslator.FromHtml("#B94E48"),
            ColorTranslator.FromHtml("#704241"),
            ColorTranslator.FromHtml("#C154C1"),
            ColorTranslator.FromHtml("#004B49"),
            ColorTranslator.FromHtml("#F5C74C"),
            ColorTranslator.FromHtml("#9955BB"),
            ColorTranslator.FromHtml("#CC00CC"),
            ColorTranslator.FromHtml("#D473D4"),
            ColorTranslator.FromHtml("#355E3B"),
            ColorTranslator.FromHtml("#FFCBA4"),
            ColorTranslator.FromHtml("#FF1493"),
            ColorTranslator.FromHtml("#A95C68"),
            ColorTranslator.FromHtml("#843F5B"),
            ColorTranslator.FromHtml("#FF9933"),
            ColorTranslator.FromHtml("#00BFFF"),
            ColorTranslator.FromHtml("#4A646C"),
            ColorTranslator.FromHtml("#7E5E60"),
            ColorTranslator.FromHtml("#66424D"),
            ColorTranslator.FromHtml("#BA8759"),
            ColorTranslator.FromHtml("#1560BD"),
            ColorTranslator.FromHtml("#C19A6B"),
            ColorTranslator.FromHtml("#EDC9AF"),
            ColorTranslator.FromHtml("#EA3C53"),
            ColorTranslator.FromHtml("#B9F2FF"),
            ColorTranslator.FromHtml("#696969"),
            ColorTranslator.FromHtml("#9B7653"),
            ColorTranslator.FromHtml("#1E90FF"),
            ColorTranslator.FromHtml("#D71868"),
            ColorTranslator.FromHtml("#85BB65"),
            ColorTranslator.FromHtml("#664C28"),
            ColorTranslator.FromHtml("#967117"),
            ColorTranslator.FromHtml("#00009C"),
            ColorTranslator.FromHtml("#E5CCC9"),
            ColorTranslator.FromHtml("#EFDFBB"),
            ColorTranslator.FromHtml("#E1A95F"),
            ColorTranslator.FromHtml("#555D50"),
            ColorTranslator.FromHtml("#C2B280"),
            ColorTranslator.FromHtml("#1B1B1B"),
            ColorTranslator.FromHtml("#614051"),
            ColorTranslator.FromHtml("#F0EAD6"),
            ColorTranslator.FromHtml("#1034A6"),
            ColorTranslator.FromHtml("#7DF9FF"),
            ColorTranslator.FromHtml("#FF003F"),
            ColorTranslator.FromHtml("#00FFFF"),
            ColorTranslator.FromHtml("#00FF00"),
            ColorTranslator.FromHtml("#6F00FF"),
            ColorTranslator.FromHtml("#F4BBFF"),
            ColorTranslator.FromHtml("#CCFF00"),
            ColorTranslator.FromHtml("#BF00FF"),
            ColorTranslator.FromHtml("#3F00FF"),
            ColorTranslator.FromHtml("#8F00FF"),
            ColorTranslator.FromHtml("#FFFF00"),
            ColorTranslator.FromHtml("#50C878"),
            ColorTranslator.FromHtml("#6C3082"),
            ColorTranslator.FromHtml("#1B4D3E"),
            ColorTranslator.FromHtml("#B48395"),
            ColorTranslator.FromHtml("#AB4E52"),
            ColorTranslator.FromHtml("#563C5C"),
            ColorTranslator.FromHtml("#96C8A2"),
            ColorTranslator.FromHtml("#44D7A8"),
            ColorTranslator.FromHtml("#C19A6B"),
            ColorTranslator.FromHtml("#801818"),
            ColorTranslator.FromHtml("#B53389"),
            ColorTranslator.FromHtml("#DE5285"),
            ColorTranslator.FromHtml("#F400A1"),
            ColorTranslator.FromHtml("#E5AA70"),
            ColorTranslator.FromHtml("#4D5D53"),
            ColorTranslator.FromHtml("#E0B582"),
            ColorTranslator.FromHtml("#4F7942"),
            ColorTranslator.FromHtml("#FF2800"),
            ColorTranslator.FromHtml("#6C541E"),
            ColorTranslator.FromHtml("#B22222"),
            ColorTranslator.FromHtml("#CE2029"),
            ColorTranslator.FromHtml("#E25822"),
            ColorTranslator.FromHtml("#FC8EAC"),
            ColorTranslator.FromHtml("#664423"),
            ColorTranslator.FromHtml("#F7E98E"),
            ColorTranslator.FromHtml("#EEDC82"),
            ColorTranslator.FromHtml("#A2006D"),
            ColorTranslator.FromHtml("#FFFAF0"),
            ColorTranslator.FromHtml("#FFBF00"),
            ColorTranslator.FromHtml("#FF1493"),
            ColorTranslator.FromHtml("#CCFF00"),
            ColorTranslator.FromHtml("#FF004F"),
            ColorTranslator.FromHtml("#014421"),
            ColorTranslator.FromHtml("#228B22"),
            ColorTranslator.FromHtml("#A67B5B"),
            ColorTranslator.FromHtml("#856D4D"),
            ColorTranslator.FromHtml("#0072BB"),
            ColorTranslator.FromHtml("#86608E"),
            ColorTranslator.FromHtml("#9EFD38"),
            ColorTranslator.FromHtml("#D473D4"),
            ColorTranslator.FromHtml("#FD6C9E"),
            ColorTranslator.FromHtml("#4E1609"),
            ColorTranslator.FromHtml("#C72C48"),
            ColorTranslator.FromHtml("#F64A8A"),
            ColorTranslator.FromHtml("#77B5FE"),
            ColorTranslator.FromHtml("#8806CE"),
            ColorTranslator.FromHtml("#AC1E44"),
            ColorTranslator.FromHtml("#A6E7FF"),
            ColorTranslator.FromHtml("#FF00FF"),
            ColorTranslator.FromHtml("#C154C1"),
            ColorTranslator.FromHtml("#FF77FF"),
            ColorTranslator.FromHtml("#C74375"),
            ColorTranslator.FromHtml("#E48400"),
            ColorTranslator.FromHtml("#CC6666")
        };
    }
}
