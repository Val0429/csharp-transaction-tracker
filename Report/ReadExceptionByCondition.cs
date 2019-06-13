using System;
using System.Collections.Generic;
using System.Xml;
using Constant;

namespace Report
{
    public class ReadExceptionByCondition
    {
        private const String CgiReadExceptionByCondition = @"cgi-bin/posdbquery?method=ReadExceptionByCondition";

        public static XmlDocument ObtainCondition(ICollection<String> posIds, ICollection<String> cashierIds, ICollection<String> cashiers,
            UInt64 startutc, UInt64 endutc, ICollection<String> exceptions, ICollection<String> keywords, Int32 pageIndex, UInt16 count)
        {
            //<Page index=""1"" >10</Page>
            //<StoreId>1</StoreId>
            //<RegisterId>2</RegisterId>
            //<CashierId>1</CashierId> 
            //<CashierId>2</CashierId>
            //<Cashier>Leo</Cashier>
            //<Cashier>Deray</Cashier>
            //<StartUTC>1333065600000</StartUTC>
            //<EndUTC>1333065600000</EndUTC>
            //<Exceptions>
            //    <Exception>Void</Exception>
            //</Exceptions>
            //<Tags>
            //    <Tag tagName=""Total"" do=""SUM"" condition="">="">10</Tag>
            //    <Tag tagName=""Total"" do=""SUM"" condition=""<="">50</Tag>
            //    <Tag tagName=""Table"" condition=""=="">50</Tag>
            //</Tags>
            //<Keywords>
            //    <Keyword>Chicken</Keyword>
            //</Keywords>
            //<ExceptionAmounts>
            //    <ExceptionAmount exception=""VOID"" condition="">="">10</ExceptionAmount> 
            //</ExceptionAmounts>

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
                    //if(String.IsNullOrEmpty(cashierId)) continue;
                    if(cashierId == null) continue;

                    xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "CashierId", cashierId));
                }
            }
            if (cashiers != null && cashiers.Count > 0)
            {
                foreach (var cashier in cashiers)
                {
                    //if (String.IsNullOrEmpty(cashier)) continue;
                    if (cashier == null) continue;

                    xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Cashier", cashier));
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
            if (exceptions.Count > 0)
            {
                XmlElement exceptionsNode = xmlDoc.CreateElement("Exceptions");
                xmlRoot.AppendChild(exceptionsNode);

                foreach (var exception in exceptions)
                {
                    if (String.IsNullOrEmpty(exception)) continue;

                    exceptionsNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Exception", exception));
                }
            }

            //has keyword
            if (keywords.Count > 0)
            {
                XmlElement keywordsNode = xmlDoc.CreateElement("Keywords");
                xmlRoot.AppendChild(keywordsNode);

                foreach (var keyword in keywords)
                {
                    if (String.IsNullOrEmpty(keyword)) continue;

                    keywordsNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Keyword", keyword));
                }
            }

            return xmlDoc;
        }

        public static POS_Exception.ExceptionDetailList Parse(XmlDocument reportXml, Int32 timezone)
        {
            var result = new POS_Exception.ExceptionDetailList();

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
                result.PageIndex = Convert.ToInt32(index);

            XmlNodeList resultList = reportXml.GetElementsByTagName("Exception");
            foreach (XmlElement exceptionNode in resultList)
            {
                var posId = exceptionNode.GetAttribute("registerId");
                var utc =  Convert.ToUInt64(exceptionNode.GetAttribute("dateTime"));

                //var type = "$";
                //if(exceptionNode.HasAttribute("amountType"))
                //{
                //    type = exceptionNode.GetAttribute("amountType");
                //    if (type == "qty")
                //        type = "";
                //}

                //<Exception transactionId="20130704161650943-92bb9" type="COUPON" cashier="" cashierId="27" registerId="3" dateTime="1372954615489" amount="72.75" quantity="0" transactionAmount="72.24" />

                //no cashier id & name, ignore this record (will cause search exception list issue)
                //if (String.IsNullOrEmpty(exceptionNode.GetAttribute("cashierId")) && String.IsNullOrEmpty(exceptionNode.GetAttribute("cashier")))
                //    continue;

                result.Results.Add(
                    new POS_Exception.ExceptionDetail
                    {
                        TransactionId = exceptionNode.GetAttribute("transactionId"),
                        Type = POS_Exception.FindExceptionValueByKey(exceptionNode.GetAttribute("type")),
                        Cashier = exceptionNode.GetAttribute("cashier"),
                        CashierId = exceptionNode.GetAttribute("cashierId"),
                        POSId = String.IsNullOrEmpty(posId) ? "0" : posId,
                        UTC = utc,
                        DateTime = DateTimes.ToDateTime(utc, timezone),
                        ExceptionAmount = exceptionNode.GetAttribute("amount"),
                        TotalTransactionAmount = exceptionNode.GetAttribute("transactionAmount"),
                    });
            }

            //result.Results.Sort((x, y) => x.DateTime.CompareTo(y.DateTime));

            return result;
        }

        public static XmlDocument Search(XmlDocument xmlDoc, ServerCredential credential)
        {
            return Query.Send(CgiReadExceptionByCondition, xmlDoc, credential);
        }
    }
}
