using System;
using System.Collections.Generic;
using System.Xml;
using Constant;

namespace Report
{
    public class CustomReport
    {
        public static List<POS_Exception.ExceptionCount> ParseSummaryXmlToExceptionCountList(XmlDocument reportXml)
        {
            //<Result>
            //    <Data date="12-01 ~ 12-07">
            //        <Exception name="VOID">123</Exception>
            //        <Exception name="Clear">456</Exception>
            //    </Data>
            //    <Data date="12-08 ~ 12-15">
            //        <Exception name="Less">333</Exception>
            //    </Data>
            //</Result>

            var result = new List<POS_Exception.ExceptionCount>();

            var dateList = reportXml.GetElementsByTagName("Data");

            //if an exception in 3 range also count = 0, remove from parser
            var exceptions = new List<String>();
            foreach (XmlElement dateNode in dateList)
            {
                var dateStr = dateNode.GetAttribute("date");

                var exceptionList = dateNode.GetElementsByTagName("Exception");

                foreach (XmlElement exceptionNode in exceptionList)
                {
                    var exceptionCount = new POS_Exception.ExceptionCount
                    {
                        DateTime = dateStr,
                        Exception = exceptionNode.GetAttribute("name"),
                        Count = Convert.ToInt32(exceptionNode.InnerText)
                    };
                    result.Add(exceptionCount);

                    if (!exceptions.Contains(exceptionCount.Exception))
                        exceptions.Add(exceptionCount.Exception);
                }
            }


            var temp = new List<POS_Exception.ExceptionCount>();
            foreach (var exception in exceptions)
            {
                var totalCount = 0;
                temp.Clear();
                foreach (var exceptionCount in result)
                {
                    if (exceptionCount.Exception == exception)
                    {
                        totalCount += exceptionCount.Count;
                        temp.Add(exceptionCount);
                    }
                }

                //useless exception, remove it
                if (totalCount == 0)
                {
                    foreach (var exceptionCount in temp)
                    {
                        result.Remove(exceptionCount);
                    }
                }
            }

            return result;
        }

        public static List<POS_Exception.ExceptionCount> ParseTrendXmlToExceptionCountList(XmlDocument reportXml)
        {
            //<Result>
            //    <Data date="12-01 ~ 12-07">
            //        <Exception>123</Exception>
            //    </Data>
            //    <Data date="12-08 ~ 12-15">
            //        <Exception>333</Exception>
            //    </Data>
            //</Result>

            var result = new List<POS_Exception.ExceptionCount>();

            var dateList = reportXml.GetElementsByTagName("Data");

            //if an exception in 3 range also count = 0, remove from parser
            var exceptions = new List<String>();
            foreach (XmlElement dateNode in dateList)
            {
                var dateStr = dateNode.GetAttribute("date");

                var exceptionList = dateNode.GetElementsByTagName("Exception");

                foreach (XmlElement exceptionNode in exceptionList)
                {
                    var exceptionCount = new POS_Exception.ExceptionCount
                    {
                        DateTime = dateStr,
                        Count = Convert.ToInt32(exceptionNode.InnerText)
                    };
                    result.Add(exceptionCount);

                    if (!exceptions.Contains(exceptionCount.Exception))
                        exceptions.Add(exceptionCount.Exception);
                }
            }

            return result;
        }
    }
}
