using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Constant;

namespace Report
{
    public class ReadTransactionById
    {
        private const String CgiReadTransactionById = @"cgi-bin/posdbquery?method=ReadTransactionById";

        public static XmlDocument ObtainCondition(String transactionId)
        {
            //<TransactionId>2012110500004-DRYFSD</TransactionId>
            var xmlDoc = new XmlDocument();
            XmlElement xmlRoot = xmlDoc.CreateElement("Request");
            xmlDoc.AppendChild(xmlRoot);
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "TransactionId", transactionId));

            return xmlDoc;
        }

        public static POS_Exception.TransactionItemList Parse(XmlDocument reportXml, Int32 timezone)
        {
            //<Result status="200">
            //    <Transaction checkNo="1355498579626135549858001702546" startTime="1355498579626" endTime="1355498580017" transactionTime="FRI DECEMBER 14,2012 - 16:43:45" cashier="Peter Frampton" cashierId="1" cashRegisterId="1">
            //        <Combine relatedTag="TABLE" endTag="CHECK">1</Combine> 
            //        <Items>
            //            <Item datetime="1355498579672" value="9.95" valueType="$" quantity="1">01, 1 - 1/2 L. Red Wine: $9.95</Item> 
            //            <Item datetime="1355498579697" value="4.95" valueType="$" quantity="1">01, 1 - 1/4l. Red Wine: $4.95</Item> 
            //        </Items>
            //        <Exceptions /> 
            //        <Tags>
            //            <Tag name="TABLE" datetime="1355498579655">1</Tag> 
            //        </Tags>
            //        <Content>01,--Begin--`FRI DECEMBER 14,2012 - 16:43:45`Peter Frampton - 1 01, 01, TABLE : 1 01, 1 - 1/2 L. Red Wine: $9.95 01, 1 - 1/4l. Red Wine: $4.95 01, 1 - Gl.Red Wine: $3.95 01, 1 - 1/4l. White Wine: $4.95 01, 1 - Order Cheddar Ch: $0.95 01, 1 - Order Ham: $1.50 01, 1 - Bagel: $1.25 01, 1 - Banana Pancakes: $4.95 01, 1 - Cheese Omelet: $4.95 01, 1 - =>Wheat Bread: $0.00 01, 1 - Two Eggs With: $4.25 01, 1 - =>Poached: $0.00 01, 1 - =>Sausage: $0.00 01, 1 - =>Ham: $0.00 01, 1 - =>Bacon: $0.00 01, 1 - =>Rye Bread: $0.00 01, 01,--End--</Content> 
            //    </Transaction>
            //    <Transaction checkNo="1355498580035135549858019500968" startTime="1355498580035" endTime="1355498580195" transactionTime="FRI DECEMBER 14,2012 - 16:44:28" cashier="Peter Frampton" cashierId="1" cashRegisterId="1">
            //        <Combine relatedTag="TABLE" endTag="CHECK">1</Combine> 
            //        <Items /> 
            //        <Exceptions /> 
            //        <Tags>
            //            <Tag name="TABLE" datetime="1355498580076" value="1">01,    Table     1</Tag> 
            //            <Tag name="CHECK" datetime="1355498580090 value="100586">01,   CHECK    100586</Tag> 
            //        </Tags>
            //        <Content>01,--Begin--`FRI DECEMBER 14,2012 - 16:44:28`Peter Frampton - 1 01, 01, TABLE : 1 01, CHECK : 100586 01, TOTAL : $41.65 01, CASH: $0.00 01, CASH: $41.65 01, 01,--End--</Content> 
            //    </Transaction>
            //</Result>
            //-------------------------------------------------------------

            var result = new POS_Exception.TransactionItemList();

            XmlNodeList transactionList = reportXml.GetElementsByTagName("Transaction");

            foreach (XmlElement transactionNode in transactionList)
            {
                var temp = new List<POS_Exception.TransactionItem>();

                var startTime = Convert.ToUInt64(transactionNode.GetAttribute("startTime"));
                var endTime = Convert.ToUInt64(transactionNode.GetAttribute("endTime"));

                if (result.StartDateTime == 0 || startTime < result.StartDateTime)
                    result.StartDateTime = startTime;

                if (result.EndDateTime == 0 || endTime > result.EndDateTime)
                    result.EndDateTime = endTime;

                var transactionTime = transactionNode.GetAttribute("transactionTime");
                var cashier = transactionNode.GetAttribute("cashier");
                var cashierId = transactionNode.GetAttribute("cashierId");
                var posIdStr = transactionNode.GetAttribute("cashRegisterId");
                var posId = (posIdStr != "") ? posIdStr :"0";

                //-------------------------------------------------------------
                //var itemList = transactionNode.GetElementsByTagName("Item");
                var itemsNode = transactionNode.SelectSingleNode("Items");
                if (itemsNode != null)
                {
                    var itemList = itemsNode.SelectNodes("Item");
                    if (itemList != null)
                    {
                        foreach (XmlElement itemNode in itemList)
                        {
                            //if (String.IsNullOrEmpty(itemNode.InnerText)) continue;

                            var rows = itemNode.InnerText.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                            for (var i = 0; i < rows.Length; i++ )
                            {
                                rows[i] = rows[i].Replace("\t", "    ");
                            }
                            //var row = String.Join("    ", rows).Trim();
                            //if (String.IsNullOrEmpty(row)) continue;

                            var utc = Convert.ToUInt64(itemNode.GetAttribute("datetime"));
                            var valueType = itemNode.GetAttribute("valueType");
                            var seqStr = itemNode.GetAttribute("seq");
                            var seq = (String.IsNullOrEmpty(seqStr) ? 0 : Convert.ToInt32(seqStr));

                            //if SAME DateTime is exist, ignore this exception, the content of this exception is under it's item, and parse above
                            if (SameDateTimeAndSameSeq(temp, utc, seq)) continue;

                            //hide row text(dont display) but exception still count!
                            var dontDisplay = (itemNode.GetAttribute("display") == "false");
                            if (dontDisplay) continue;

                            //multi line
                            foreach (var row in rows)
                            {
                                if (String.IsNullOrEmpty(row)) continue;

                                temp.Add(new POS_Exception.TransactionItem
                                {
                                    POS = posId,
                                    TransactionTime = transactionTime,
                                    Cashier = cashier,
                                    CashierId = cashierId,
                                    UTC = utc,
                                    DateTime = DateTimes.ToDateTime(utc, timezone),
                                    Content = row,
                                    Seq = seq,
                                    ValueType = valueType
                                });
                            }
                        }
                    }
                }
                //-------------------------------------------------------------
                //var tagList = transactionNode.GetElementsByTagName("Tag");
                var tagsNode = transactionNode.SelectSingleNode("Tags");
                if (tagsNode != null)
                {
                    var tagList = tagsNode.SelectNodes("Tag");
                    if (tagList != null)
                    {
                        foreach (XmlElement tagNode in tagList)
                        {
                            var rows = tagNode.InnerText.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                            for (var i = 0; i < rows.Length; i++)
                            {
                                rows[i] = rows[i].Replace("\t", "    ");
                            }
                            //var row = String.Join("    ", rows).Trim();
                            //if (String.IsNullOrEmpty(row)) continue;

                            var utc = Convert.ToUInt64(tagNode.GetAttribute("datetime"));
                            var seqStr = tagNode.GetAttribute("seq");
                            var seq = (String.IsNullOrEmpty(seqStr) ? 0 : Convert.ToInt32(seqStr));

                            //if SAME DateTime is exist, ignore this exception, the content of this exception is under it's item, and parse above
                            if (SameDateTimeAndSameSeq(temp, utc, seq)) continue;

                            //hide row text(dont display) but exception still count!
                            var dontDisplay = (tagNode.GetAttribute("display") == "false");
                            if (dontDisplay) continue;

                            //multi line
                            foreach (var row in rows)
                            {
                                if (String.IsNullOrEmpty(row)) continue;

                                temp.Add(new POS_Exception.TransactionItem
                                {
                                    POS = posId,
                                    TransactionTime = transactionTime,
                                    Cashier = cashier,
                                    CashierId = cashierId,
                                    UTC = utc,
                                    DateTime = DateTimes.ToDateTime(utc, timezone),
                                    Content = row,
                                    Seq = seq
                                });
                            }
                        }
                    }
                }
                //-------------------------------------------------------------
                //var exceptionList = transactionNode.GetElementsByTagName("Exception");
                var exceptionsNode = transactionNode.SelectSingleNode("Exceptions");
                if (exceptionsNode != null)
                {
                    var exceptionList = exceptionsNode.SelectNodes("Exception");
                    if (exceptionList != null)
                    {
                        foreach (XmlElement exceptionNode in exceptionList)
                        {
                            var content = exceptionNode.GetAttribute("type");
                            if (String.IsNullOrEmpty(content)) continue;

                            //exceptionNode.InnerText
                            var utc = Convert.ToUInt64(exceptionNode.GetAttribute("datetime"));
                            var seqStr = exceptionNode.GetAttribute("seq");
                            var seq = (String.IsNullOrEmpty(seqStr) ? 0 : Convert.ToInt32(seqStr));

                            //hide row text(dont display) but exception still count!
                            var dontDisplay = (exceptionNode.GetAttribute("display") == "false");
                            if (dontDisplay) continue;

                            var itemList = exceptionNode.SelectNodes("Item");

                            //no items, write in innerText
                            if (itemList == null || itemList.Count == 0)
                            {
                                var rows = exceptionNode.InnerText.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                                for (var i = 0; i < rows.Length; i++)
                                {
                                    rows[i] = rows[i].Replace("\t", "    ");
                                }

                                //if SAME DateTime is exist, ignore this exception, the content of this exception is under it's item, and parse above)
                                if (SameDateTimeAndSameSeq(temp, utc, seq)) continue;

                                //multi line
                                foreach (var row in rows)
                                {
                                    if (String.IsNullOrEmpty(row)) continue;

                                    temp.Add(new POS_Exception.TransactionItem
                                    {
                                        POS = posId,
                                        TransactionTime = transactionTime,
                                        Cashier = cashier,
                                        CashierId = cashierId,
                                        UTC = utc,
                                        DateTime = DateTimes.ToDateTime(utc, timezone),
                                        Content = row,
                                        Seq = seq
                                    });
                                }
                            }
                            else
                            {
                                foreach (XmlElement itemNode in itemList)
                                {
                                    exceptionNode.RemoveChild(itemNode);
                                }

                                if (!String.IsNullOrEmpty(exceptionNode.InnerText))
                                {
                                    var rows = exceptionNode.InnerText.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                                    for (var i = 0; i < rows.Length; i++)
                                    {
                                        rows[i] = rows[i].Replace("\t", "    ");
                                    }

                                    foreach (var row in rows)
                                    {
                                        temp.Add(new POS_Exception.TransactionItem
                                        {
                                            POS = posId,
                                            TransactionTime = transactionTime,
                                            Cashier = cashier,
                                            CashierId = cashierId,
                                            UTC = utc,
                                            DateTime = DateTimes.ToDateTime(utc, timezone),
                                            Content = row,
                                            Seq = seq
                                        });
                                    }
                                }

                                foreach (XmlElement itemNode in itemList)
                                {
                                    if (String.IsNullOrEmpty(itemNode.InnerText)) continue;

                                    var seqStr2 = itemNode.GetAttribute("seq");
                                    var seq2 = (String.IsNullOrEmpty(seqStr2) ? 0 : Convert.ToInt32(seqStr2));

                                    //hide row text(dont display) but exception still count!
                                    var dontDisplay2 = (itemNode.GetAttribute("display") == "false");
                                    if (dontDisplay2) continue;

                                    var rows = itemNode.InnerText.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                                    for (var i = 0; i < rows.Length; i++)
                                    {
                                        rows[i] = rows[i].Replace("\t", "    ");
                                    }

                                    foreach (var row in rows)
                                    {
                                        temp.Add(new POS_Exception.TransactionItem
                                        {
                                            POS = posId,
                                            TransactionTime = transactionTime,
                                            Cashier = cashier,
                                            CashierId = cashierId,
                                            UTC = utc,
                                            DateTime = DateTimes.ToDateTime(utc, timezone),
                                            Content = row,
                                            Seq = seq2
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
                //-------------------------------------------------------------

                //temp.Sort((x, y) => x.Seq.CompareTo(y.Seq));
                var sort = (from s in temp orderby s.Seq select s);

                result.TransactionItems.AddRange(sort);
            }

            var hasSeq = false;
            foreach (var transactionItem in result.TransactionItems)
            {
                if (transactionItem.Seq > 0)
                {
                    hasSeq = true;
                    break;
                }
            }
            //no seq data, sort by datetime
            if (!hasSeq)
            {
                result.TransactionItems.Sort((x, y) => x.UTC.CompareTo(y.UTC));
            }

            return result;
        }

        private static Boolean SameDateTimeAndSameSeq(IEnumerable<POS_Exception.TransactionItem> list, UInt64 utc, Int32 seq)
        {
            return list.Any(transactionItem => transactionItem.UTC == utc && transactionItem.Seq == seq);
        }

        public static XmlDocument Search(XmlDocument xmlDoc, ServerCredential credential)
        {
            return Query.Send(CgiReadTransactionById, xmlDoc, credential);
        }
    }
}
