using System;
using System.Collections.Generic;
using System.Xml;
using Constant;

namespace Report
{
    public class ReadDailyReportExceptionGroupByCashier
    {
        private const String CgiReadTransactionById = @"cgi-bin/posdbquery?method=ReadDailyReportExceptionGroupByCashier";

        public static XmlDocument ObtainCondition(UInt64 startutc, UInt64 endutc)
        {
            //<Request>
            //    <StartUTC>1355443200000</StartUTC> 
            //    <EndUTC>1355515200000</EndUTC> 
            //</Request>

            var xmlDoc = new XmlDocument();
            XmlElement xmlRoot = xmlDoc.CreateElement("Request");
            xmlDoc.AppendChild(xmlRoot);
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "StartUTC", startutc));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "EndUTC", endutc));

            return xmlDoc;
        }

        public static List<POS_Exception.CashierExceptionCountList> Parse(XmlDocument reportXml, Int32 timezone)
        {
            //Result startUTC=""1355443200000"" endUTC=""1355515200000"" status=""200"">
            //    <Cashier id=""1"" name=""peter"">
            //        <Exception name=""COUPON"">1643</Exception> 
            //        <Exception name=""VOID"">4841</Exception> 
            //    </Cashier>
            //    <AllCashiers>
            //        <Exception name=""COUPON"">1643</Exception> 
            //        <Exception name=""VOID"">4841</Exception> 
            //    </AllCashiers>
            //</Result>
            //-------------------------------------------------------------
            var result = new List<POS_Exception.CashierExceptionCountList>();

            var root = reportXml.FirstChild as XmlElement;
            if (root == null)
                return result;

            var startDate = Convert.ToUInt64(root.GetAttribute("startUTC"));
            var endDate = Convert.ToUInt64(root.GetAttribute("endUTC"));

            XmlNodeList cashierList = reportXml.GetElementsByTagName("Cashier");

            foreach (XmlElement cashierNode in cashierList)
            {
                var list = new POS_Exception.CashierExceptionCountList
                {
                    CashierId = cashierNode.GetAttribute("id"),
                    Cashier = cashierNode.GetAttribute("name"),
                    StartDateTime = startDate,
                    EndDateTime = endDate
                };

                var start = DateTimes.ToDateTime(startDate, timezone);
                var end = DateTimes.ToDateTime(endDate, timezone);

                var startStr = start.ToString("MM-dd-yyyy");
                var endStr = end.ToString("MM-dd-yyyy");
                var range = (startStr != endStr)
                                ? (startStr + @" ~ " + endStr)
                                : startStr;

                result.Add(list);
                var exceptionList = cashierNode.GetElementsByTagName("Exception");

                foreach (XmlElement exceptionNode in exceptionList)
                {
                    list.ExceptionList.Add(new POS_Exception.ExceptionCount
                    {
                        Exception = exceptionNode.GetAttribute("name"),
                        Count = Convert.ToInt32(exceptionNode.InnerText),
                        DateTime = range,
                    });
                }
            }

            return result;
        }

        public static XmlDocument Search(XmlDocument xmlDoc, ServerCredential credential)
        {
            return Query.Send(CgiReadTransactionById, xmlDoc, credential);
        }
    }
}
