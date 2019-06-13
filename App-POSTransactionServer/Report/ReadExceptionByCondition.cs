using System;
using System.Collections.Generic;
using System.Xml;
using Constant;

namespace App_POSTransactionServer
{
    public partial class POSManager
    {
        public POS_Exception.ExceptionDetailList ReadExceptionByCondition(String posId, UInt64 startutc, UInt64 endutc, String searchText, Int32 pageIndex, UInt16 count)
        {
            CheckDateTimeDontGoToFar(ref startutc, ref endutc);

            var exceptionList = new List<String>();
            var keywordList = new List<String>();

            var temp = searchText.Split('+');
            foreach (var str in temp)
            {
                if (String.IsNullOrEmpty(str)) continue;
                keywordList.Add(str);
            }

            if (keywordList.Count > 0)
            {
                var pos = FindPOSById(posId);
                if (Exceptions.ContainsKey(pos.Exception))
                {
                    var exceptions = Exceptions[pos.Exception];
                    foreach (var exception in exceptions.Exceptions)
                    {
                        foreach (var keyword in keywordList)
                        {
                            if (!exception.Key.Equals(keyword, StringComparison.InvariantCultureIgnoreCase) &&
                                !exception.Value.Equals(keyword, StringComparison.InvariantCultureIgnoreCase))
                                continue;

                            keywordList.Remove(keyword);
                            exceptionList.Add(exception.Key);
                            break;
                        }
                    }
                }
            }

            return ReadExceptionByCondition(new[] { posId }, null, null, startutc, endutc, exceptionList.ToArray(), keywordList.ToArray(), pageIndex, count);
        }

        public POS_Exception.ExceptionDetailList ReadExceptionByCondition(String[] posIds, String[] cashierIds, String[] cashiers, UInt64 startutc, UInt64 endutc, String[] exceptions, String[] keywords, Int32 pageIndex, UInt16 count)
        {
            CheckDateTimeDontGoToFar(ref startutc, ref endutc);

            var tempString = new List<String>();
            foreach (String posId in posIds)
            {
                tempString.Add(posId);
                if (UsePTSId(posId))
                    tempString.Add("PTS");
            }

            posIds = tempString.ToArray();

            return ReadExceptionByCondition(Report.ReadExceptionByCondition.ObtainCondition(posIds, cashierIds, cashiers, startutc, endutc, exceptions, keywords, pageIndex, count));
        }

        public POS_Exception.ExceptionDetailList ReadExceptionByCondition(Int32 pageIndex, String condition)
        {
            var xmlDoc = new XmlDocument { InnerXml = condition };
            var pageNode = Xml.GetFirstElementByTagName(xmlDoc, "Page");
            if (pageNode != null)
                pageNode.SetAttribute("index", pageIndex.ToString());

            return ReadExceptionByCondition(xmlDoc);
        }

        protected POS_Exception.ExceptionDetailList ReadExceptionByCondition(XmlDocument xmlDoc)
        {
            _watch.Reset();
            _watch.Start();
            var reportXml = Report.ReadExceptionByCondition.Search(xmlDoc, Server.Credential);
            _watch.Stop();
            Console.WriteLine(@"ReadExceptionByCondition: " + _watch.Elapsed.TotalSeconds.ToString("0.00"));

            //<Result total=""7"" pages=""1"" index=""1"" status=""200"">
            //    <Exception transactionId=""20121214032426222-1355498666222135549866635300754"" type=""VOID"" cashier=""Peter Frampton"" cashierId=""1"" registerId=""1"" datetime=""1355498667364"" amount=""16.90"" /> 
            //    <Exception transactionId=""20121214032456163-1355498696163135549869634001266"" type=""VOID"" cashier=""Peter Frampton"" cashierId=""1"" registerId=""1"" datetime=""1355498679119"" amount=""16.90"" /> 
            //</Result>

            if (reportXml == null)
                return new POS_Exception.ExceptionDetailList();

            XmlNodeList exceptionList = xmlDoc.GetElementsByTagName("Exception");
            XmlNodeList keywordList = xmlDoc.GetElementsByTagName("Keyword");

            var keywords = new List<String>();
            foreach (XmlElement exceptionNode in exceptionList)
            {
                keywords.Add(exceptionNode.InnerText);
            }

            foreach (XmlElement keywordNode in keywordList)
            {
                keywords.Add(keywordNode.InnerText);
            }

            var result = Report.ReadExceptionByCondition.Parse(reportXml, Server.Server.TimeZone);

            result.RawXml = reportXml;
            result.SearchCondition = xmlDoc.InnerXml;
            result.Elapsed = _watch.Elapsed;
            result.Keywords = keywords.ToArray();

            //if (result3.ExceptionDetails.Count == 0)
            //{
            //    Console.WriteLine(reportXml.InnerText);
            //}

            return result;
        }
    }
}
