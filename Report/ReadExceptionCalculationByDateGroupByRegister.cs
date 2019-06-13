using System;
using System.Collections.Generic;
using System.Xml;
using Constant;

namespace Report
{
    public class ReadExceptionCalculationByDateGroupByRegister
    {
        private const String CgiReadExceptionCalculationByDateGroupByRegister = @"cgi-bin/posdbquery?method=ReadExceptionCalculationByDateGroupByRegister";

        public static XmlDocument ObtainCondition(String[] posIds, String[] cashierIds, String[] cashiers, UInt64 startutc, UInt64 endutc, String[] exceptions, Int32 timezone)
        {
            //<Request>
            //    <StoreId>1</StoreId> 
            //    <RegisterId>1</RegisterId> 
            //    <RegisterId>18</RegisterId> 
            //    <CashierId>1</CashierId> 
            //    <CashierId>2</CashierId>
            //    <Cashier>Leo</Cashier>
            //    <Cashier>Deray</Cashier>
            //    <StartUTC>1354320000000</StartUTC> 
            //    <EndUTC>1356998399000</EndUTC> 
            //    <TimeZone>28800000</TimeZone> 
            //    <Exceptions>
            //        <Exception>Void</Exception> 
            //    </Exceptions>
            //</Request>

            var xmlDoc = new XmlDocument();
            XmlElement xmlRoot = xmlDoc.CreateElement("Request");
            xmlDoc.AppendChild(xmlRoot);
            //xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "StoreId", "1"));
            if (posIds != null && posIds.Length > 0)
            {
                foreach (var posId in posIds)
                {
                    xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "RegisterId", posId));
                }
            }

            if (cashierIds != null && cashierIds.Length > 0)
            {
                foreach (var cashierId in cashierIds)
                {
                    xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "CashierId", cashierId));
                }
            }

            if (cashiers != null && cashiers.Length > 0)
            {
                foreach (var cashier in cashiers)
                {
                    xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Cashier", cashier));
                }
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

        public static Dictionary<String, List<POS_Exception.ExceptionCount>> Parse(XmlDocument reportXml)
        {
            //<Result status=""200"">
            //    <Register id=""1"">
            //        <Date dateUTC=""1355472000000"" date=""2012/12/14"">
            //            <Exception name=""VOID"" amount=""87196.00"">4134</Exception> 
            //        </Date>
            //        <Date dateUTC=""1356422400000"" date=""2012/12/25"">
            //            <Exception name=""VOID"" amount=""11283.00"">503</Exception> 
            //        </Date>
            //    </Register>
            //    <Register id=""18"">
            //        <Date dateUTC=""1355472000000"" date=""2012/12/14"">
            //            <Exception name=""VOID"" amount=""767.00"">34</Exception> 
            //        </Date>
            //        <Date dateUTC=""1355644800000"" date=""2012/12/16"">
            //            <Exception name=""VOID"" amount=""2359.00"">106</Exception> 
            //        </Date>
            //    </Register>
            //</Result>

            var result = new Dictionary<String, List<POS_Exception.ExceptionCount>>();

            var registerList = reportXml.GetElementsByTagName("Store");
            foreach (XmlElement registerNode in registerList)
            {
                var posId = registerNode.GetAttribute("id");
                if (result.ContainsKey(posId)) continue; //it's wrong

                result.Add(posId, new List<POS_Exception.ExceptionCount>());

                var dateList = registerNode.GetElementsByTagName("Date");

                foreach (XmlElement dateNode in dateList)
                {
                    var dateStr = dateNode.GetAttribute("date");

                    var exceptionList = dateNode.GetElementsByTagName("Exception");

                    foreach (XmlElement exceptionNode in exceptionList)
                    {
                        result[posId].Add(new POS_Exception.ExceptionCount
                        {
                            DateTime = dateStr,
                            Exception = exceptionNode.GetAttribute("name"),
                            Count = Convert.ToInt32(exceptionNode.InnerText)
                        });
                    }
                }
            }

            return result;
        }

        public static XmlDocument Search(XmlDocument xmlDoc, ServerCredential credential)
        {
            return Query.Send(CgiReadExceptionCalculationByDateGroupByRegister, xmlDoc, credential);
        }
    }
}
