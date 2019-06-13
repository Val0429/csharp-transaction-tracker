using System;
using System.Collections.Generic;
using System.Xml;
using Constant;

namespace App_POSTransactionServer
{
    public partial class POSManager
    {
        public POS_Exception.TransactionList ReadTransactionHeadByCondition(String posId, UInt64 startutc, UInt64 endutc, String searchText, Int32 pageIndex, UInt16 count)
        {
            CheckDateTimeDontGoToFar(ref startutc, ref endutc);

            var exceptionList = new List<String>();
            var tagList = new List<String>();
            var keywordList = new List<String>();
            
            var temp = searchText.Split('+');
            foreach (var str in temp)
            {
                if (String.IsNullOrEmpty(str)) continue;
                keywordList.Add(str);
            }

            if (keywordList.Count == 0 && String.Equals(searchText, "+"))
                keywordList.Add("+");

            var posIds = new List<String>();
            if (posId != "0")
            {
                posIds.Add(posId);
                if (UsePTSId(posId))
                    posIds.Add("PTS");
            }
            else
            {
                foreach (var pos in POSServer)
                {
                    posIds.Add(pos.Id);
                    if(UsePTSId(pos.Id))
                        posIds.Add("PTS");
                }
            }

            if (keywordList.Count > 0)
            {
                foreach (var posid in posIds)
                {
                    var pos = FindPOSById(posid);
                    if (pos == null) continue;

                    if (Exceptions.ContainsKey(pos.Exception))
                    {
                        var exception = Exceptions[pos.Exception];

                        //convert exception list
                        foreach (var obj in exception.Exceptions)
                        {
                            foreach (var keyword in keywordList)
                            {
                                if (!obj.Key.Equals(keyword, StringComparison.InvariantCultureIgnoreCase) &&
                                    !obj.Value.Equals(keyword, StringComparison.InvariantCultureIgnoreCase))
                                    continue;

                                keywordList.Remove(keyword);//keep in keyword list
                                if (!exceptionList.Contains(obj.Key))
                                    exceptionList.Add(obj.Key);
                                break;
                            }
                        }

                        //convert tag list
                        foreach (var obj in exception.Tags)
                        {
                            foreach (var keyword in keywordList)
                            {
                                if (!obj.Key.Equals(keyword, StringComparison.InvariantCultureIgnoreCase) &&
                                    !obj.Value.Equals(keyword, StringComparison.InvariantCultureIgnoreCase))
                                    continue;

                                keywordList.Remove(keyword);//keep in keyword list
                                if (!tagList.Contains(obj.Key))
                                    tagList.Add(obj.Key);
                                break;
                            }
                        }
                    }
                }
            }

            return ReadTransactionHeadByCondition(posIds.ToArray(), null, startutc, endutc, exceptionList.ToArray(), tagList.ToArray(), keywordList.ToArray(), pageIndex, count);
        }

        public POS_Exception.TransactionList ReadTransactionHeadByCondition(String[] posIds, String[] cashierIds, UInt64 startutc, UInt64 endutc,
            String[] exceptions, String[] tags, String[] keywords, Int32 pageIndex, UInt16 count)
        {
            CheckDateTimeDontGoToFar(ref startutc, ref endutc);

            return ReadTransactionHeadByCondition(Report.ReadTransactionHeadByCondition.ObtainCondition(posIds, cashierIds, startutc, endutc, exceptions, tags, keywords, pageIndex, count));
        }

        public POS_Exception.TransactionList ReadTransactionHeadByCondition(String transactionId, UInt64 startutc, UInt64 endutc, String[] exceptions)
        {
            CheckDateTimeDontGoToFar(ref startutc, ref endutc);

            return ReadTransactionHeadByCondition(Report.ReadTransactionHeadByCondition.ObtainCondition(transactionId, startutc, endutc, exceptions));
        }

        public POS_Exception.TransactionList ReadTransactionHeadByCondition(Int32 pageIndex, String condition)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(condition);

            var pageNode = Xml.GetFirstElementByTagName(xmlDoc, "Page");
            if (pageNode != null)
                pageNode.SetAttribute("index", pageIndex.ToString());

            return ReadTransactionHeadByCondition(xmlDoc);
        }

        protected POS_Exception.TransactionList ReadTransactionHeadByCondition(XmlDocument conditionXml)
        {
            var result = new POS_Exception.TransactionList();

            _watch.Reset();
            _watch.Start();

            var reportXml = Report.ReadTransactionHeadByCondition.Search(conditionXml, Server.Credential);
            
            _watch.Stop();
            Console.WriteLine(@"ReadTransactionHeadByCondition: " + _watch.Elapsed.TotalSeconds.ToString("0.00"));

            if (reportXml == null)
                return result;

            return Report.ReadTransactionHeadByCondition.Parse(reportXml, conditionXml, Server.Server.TimeZone);
        }
    }
}
