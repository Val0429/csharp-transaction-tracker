using System;
using System.Collections.Generic;
using System.Xml;
using Constant;

namespace Report
{
    public class ReadDailyReportByStationGroupByException
    {
        private const String CgiReadDailyReportByStationGroupByException = @"cgi-bin/posdbquery?method=ReadDailyReportByStationGroupByException";

        public static XmlDocument ObtainCondition(String[] posIds, UInt64 startutc, UInt64 endutc, String[] exceptions)
        {
            //<RegisterId>2</RegisterId>
            //<Exceptions>
            // <Exception>Void</Exception>
            //</Exceptions>
            //<StartUTC>1333065600000</StartUTC>
            //<EndUTC>1333065600000</EndUTC>

            var xmlDoc = new XmlDocument();
            XmlElement xmlRoot = xmlDoc.CreateElement("Request");
            xmlDoc.AppendChild(xmlRoot);
            //xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "StoreId", "1"));
            foreach (var posId in posIds)
            {
                //if (posId != 0) Oracle Demo
                    xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "RegisterId", posId));
            }
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "StartUTC", startutc));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "EndUTC", endutc));

            //has exception
            if (exceptions.Length > 0)
            {
                XmlElement exceptionsNode = xmlDoc.CreateElement("Exceptions");
                xmlRoot.AppendChild(exceptionsNode);

                foreach (var exception in exceptions)
                {
                    exceptionsNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Exception", exception));
                }
            }

            return xmlDoc;
        }

        public static List<POS_Exception.ExceptionCount> Parse(XmlDocument reportXml)
        {
            //<Result status="200" total="2">
            //    <Exception name="Void">2</Exception>
            //    <Exception name="Coupon">2</Exception>
            //</Result>

            var result = new List<POS_Exception.ExceptionCount>();

            var exceptionList = reportXml.GetElementsByTagName("Exception");

            foreach (XmlElement exceptionNode in exceptionList)
            {
                result.Add(new POS_Exception.ExceptionCount
                {
                    Exception = exceptionNode.GetAttribute("name"),
                    Count = Convert.ToInt32(exceptionNode.InnerText)
                });
            }

            return result;
        }

        public static XmlDocument Search(XmlDocument xmlDoc, ServerCredential credential)
        {
            return Query.Send(CgiReadDailyReportByStationGroupByException, xmlDoc, credential);
        }
    }
}
