using System;
using System.Collections.Generic;
using System.Xml;
using Report;

namespace Constant.Report
{
    public class ReadExceptionCalculationByMonth
    {
        private const String CgiReadExceptionCalculationByMonth = @"cgi-bin/posdbquery?method=ReadExceptionCalculationByMonth";

        public static XmlDocument ObtainCondition(String[] posIds, UInt64 startutc, UInt64 endutc, String[] exceptions, Int32 timezone)
        {
            //<Request>
            //    <RegisterId>2</RegisterId>
            //    <StartUTC>1333065600000</StartUTC>
            //    <EndUTC>1333065600000</EndUTC>
            //    <TimeZone>28800000</TimeZone>
            //    <Exceptions>
            //        <Exception>Void</Exception>
            //        <Exception>Coupon</Exception>
            //    </Exceptions>
            //</Request>

            var xmlDoc = new XmlDocument();
            XmlElement xmlRoot = xmlDoc.CreateElement("Request");
            xmlDoc.AppendChild(xmlRoot);
            //xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "StoreId", "1"));

            foreach (var posId in posIds)
            {
                xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "RegisterId", posId));
            }
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "StartUTC", startutc));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "EndUTC", endutc));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "TimeZone", timezone + "000"));

            XmlElement exceptionsNode = xmlDoc.CreateElement("Exceptions");
            xmlRoot.AppendChild(exceptionsNode);

            foreach (var exception in exceptions)
            {
                exceptionsNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Exception", exception));
            }

            return xmlDoc;
        }

        public static List<POS_Exception.ExceptionCount> Parse(XmlDocument reportXml)
        {
            //<Result status=""200"">
            //    <Dates>
            //        <Date dateUTC=""1355472000000"" date=""2012/12/14"">
            //            <Exception name=""VOID"">4134</Exception> 
            //        </Date>
            //        <Date dateUTC=""1356422400000"" date=""2012/12/25"">
            //            <Exception name=""VOID"">503</Exception> 
            //        </Date>
            //    </Dates>
            //</Result>

            var result = new List<POS_Exception.ExceptionCount>();

            var dateList = reportXml.GetElementsByTagName("Date");

            foreach (XmlElement dateNode in dateList)
            {
                var dateStr = dateNode.GetAttribute("date");

                var exceptionList = dateNode.GetElementsByTagName("Exception");

                foreach (XmlElement exceptionNode in exceptionList)
                {
                    result.Add(new POS_Exception.ExceptionCount
                    {
                        DateTime = dateStr,
                        Exception = exceptionNode.GetAttribute("name"),
                        Count = Convert.ToInt32(exceptionNode.InnerText)
                    });
                }
            }

            return result;
        }

        public static XmlDocument Search(XmlDocument xmlDoc, ServerCredential credential)
        {
            return Query.Send(CgiReadExceptionCalculationByMonth, xmlDoc, credential);
        }
    }
}
