using System;
using System.Collections.Generic;
using System.Xml;
using Constant;

namespace Report
{
    public class ReadTransactionHeadByCondition
    {
        private const String CgiReadTransactionHeadByCondition = @"cgi-bin/posdbquery?method=ReadTransactionHeadByCondition";

        public static XmlDocument ObtainCondition(ICollection<String> posIds, ICollection<String> cashierIds,
            UInt64 startutc, UInt64 endutc, String[] exceptions, String[] tags, String[] keywords, Int32 pageIndex, UInt16 count)
        {
            //<Page index="1" >20</Page>
            //<StoreId>1</StoreId>
            //<RegisterId>2</RegisterId>
            //<CashierId>1</CashierId>
            //<StartUTC>1333065600000</StartUTC>
            //<EndUTC>1333065600000</EndUTC>
            //<Exceptions>
            // <Exception>Void</Exception>
            //</Exceptions>
            //<Tags>
            //  <Tag>TAX</Tag>
            //</Tags>
            //<Keywords>
            //  <Keyword>Milk</Keyword>
            //</Keywords>

            var xmlDoc = new XmlDocument();
            XmlElement xmlRoot = xmlDoc.CreateElement("Request");
            xmlDoc.AppendChild(xmlRoot);

            if (pageIndex > 0)
            {
                var pageNode = Xml.CreateXmlElementWithText(xmlDoc, "Page", count.ToString()); //20
                pageNode.SetAttribute("index", pageIndex.ToString());
                xmlRoot.AppendChild(pageNode);
            }

            //xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "StoreId", "1"));
            if (cashierIds != null && cashierIds.Count > 0)
            {
                foreach (var cashierId in cashierIds)
                {
                    xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "CashierId", cashierId));
                }
            }

            if (posIds != null && posIds.Count > 0)
            {
                foreach (var posId in posIds)
                {
                    xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "RegisterId", posId));
                }
            }
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "StartUTC", startutc));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "EndUTC", endutc));

            //has exception
            if (exceptions != null && exceptions.Length > 0)
            {
                XmlElement exceptionsNode = xmlDoc.CreateElement("Exceptions");
                xmlRoot.AppendChild(exceptionsNode);

                foreach (var exception in exceptions)
                {
                    exceptionsNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Exception", exception));
                }
            }

            //has tag
            if (tags != null && tags.Length > 0)
            {
                XmlElement tagsNode = xmlDoc.CreateElement("Tags");
                xmlRoot.AppendChild(tagsNode);

                foreach (var tag in tags)
                {
                    tagsNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Tag", tag));
                }
            }
            
            //has keyword
            if (keywords != null && keywords.Length > 0)
            {
                XmlElement keywordsNode = xmlDoc.CreateElement("Keywords");
                xmlRoot.AppendChild(keywordsNode);

                foreach (var keyword in keywords)
                {
                    keywordsNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Keyword", keyword));
                }
            }

            return xmlDoc;
        }

        public static XmlDocument ObtainCondition(String transactionId, UInt64 startutc, UInt64 endutc, String[] exceptions)
        {
            //<Page index="1" >20</Page>
            //<TransactionId>20130807022139286-ed6b7</TransactionId>
            //<StartUTC>1333065600000</StartUTC>
            //<EndUTC>1333065600000</EndUTC>
            //<Exceptions>
            // <Exception>Void</Exception>
            //</Exceptions>

            var xmlDoc = new XmlDocument();
            XmlElement xmlRoot = xmlDoc.CreateElement("Request");
            xmlDoc.AppendChild(xmlRoot);

            //var pageNode = Xml.CreateXmlElementWithText(xmlDoc, "Page", "1"); //20
            //pageNode.SetAttribute("index", "1");
            //xmlRoot.AppendChild(pageNode);

            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "TransactionId", transactionId));

            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "StartUTC", startutc));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "EndUTC", endutc));

            //has exception
            if (exceptions != null && exceptions.Length > 0)
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

        public static POS_Exception.TransactionList Parse(XmlDocument reportXml, XmlDocument conditionXml, Int32 timezone)
        {
            //<Result total="691" pages="70" index="1" status="200">
            //    <Transaction id="20121214032314409-1355498594409135549859454800754" dateTime="1360832432069" registerId="3" cashier="Mary Chris" cashierId="3">
            //      <Total>159.55</Total> 
            //      <ExceptionAmount>16.90</ExceptionAmount> 
            //    </Transaction>
            //</Result>

            var result = new POS_Exception.TransactionList();

            var resultNode = (XmlElement)reportXml.SelectSingleNode("Result");
            if (resultNode == null) return result;

            var total = resultNode.GetAttribute("total");
            if (!String.IsNullOrEmpty(total))
                result.Count = Convert.ToUInt32(total);

            var pages = resultNode.GetAttribute("pages");
            if (!String.IsNullOrEmpty(pages))
                result.Pages = Convert.ToInt32(pages);

            var index = resultNode.GetAttribute("index");
            if (!String.IsNullOrEmpty(index))
                result.PageIndex = Convert.ToUInt16(index);

            result.SearchCondition = conditionXml.InnerXml;

            XmlNodeList transactionList = reportXml.GetElementsByTagName("Transaction");
            foreach (XmlElement transactionNode in transactionList)
            {
                var posId = transactionNode.GetAttribute("registerId");
                var utc = Convert.ToUInt64(transactionNode.GetAttribute("dateTime"));
                result.Results.Add(
                    new POS_Exception.Transaction
                    {
                        CashierId = transactionNode.GetAttribute("cashierId"),
                        Cashier = transactionNode.GetAttribute("cashier"),
                        Id = transactionNode.GetAttribute("id"),
                        POSId = String.IsNullOrEmpty(posId) ?"0" : posId,
                        DateTime = DateTimes.ToDateTime(utc, timezone),
                        UTC = utc,
                        Total = Xml.GetFirstElementValueByTagName(transactionNode, "Total"),
                        ExceptionAmount = Xml.GetFirstElementValueByTagName(transactionNode, "ExceptionAmount"),
                    });
            }

            if (result.Results.Count == 0)
            {
                Console.WriteLine(reportXml.InnerText);
            }

            return result;
        }

        public static XmlDocument Search(XmlDocument xmlDoc, ServerCredential credential)
        {
            return Query.Send(CgiReadTransactionHeadByCondition, xmlDoc, credential);
        }
    }
}
