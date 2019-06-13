using System;
using System.Linq;
using System.Xml;
using Constant;

namespace Report
{
    public class ReadTransactionByCondition
    {
        private const String CgiReadTransactionByCondition = @"cgi-bin/posdbquery?method=ReadTransactionByCondition";

        public static XmlDocument ObtainCondition(POS_Exception.AdvancedSearchCriteria criteria, UInt64 startutc, UInt64 endutc)
        {
            //<Request>
            //    <Page index=""1"">10</Page> 
            //    <StartUTC>1360281600000</StartUTC> 
            //    <EndUTC>1360313385000</EndUTC> 
            //    <DateTimeSet>Today</DateTimeSet>  //use for template
            //    <RegisterIds logic=""AND"">
            //        <RegisterId condition=""="">1</RegisterId> 
            //        <RegisterId condition=""="" logic=""OR"">2</RegisterId> 
            //    </RegisterIds>
            //    <CashierIds logic=""AND"">
            //        <CashierId condition=""="">1</CashierId> 
            //        <CashierId condition=""="" logic=""OR"">3</CashierId> 
            //    </CashierIds>
            //    <Cashiers logic=""AND"">
            //        <Cashier condition=""like"">Peter</Cashier> 
            //        <Cashier condition=""like"" logic=""OR"">Mary</Cashier> 
            //    </Cashiers>
            //    <Exceptions logic=""AND"">
            //        <Exception condition=""<>"">COUPON</Exception> 
            //        <Exception condition=""="" logic=""AND"" keyword=""Pizza"" keywordCondition=""like"">CLEAR</Exception> 
            //    </Exceptions>
            //    <ExceptionAmounts logic=""AND"">
            //        <ExceptionAmount exception=""VOID"" do=""SUM"" condition="">="">5</ExceptionAmount> 
            //        <ExceptionAmount exception=""VOID"" do=""COUNT"" condition="">="" logic=""AND"">1</ExceptionAmount> 
            //        <ExceptionAmount exception=""VOID"" do=""SUM"" condition="">="" logic=""AND"" keyword=""Tomato"" keywordCondition=""like"">2</ExceptionAmount> 
            //        <ExceptionAmount exception=""VOID"" do=""COUNT"" condition="">="" logic=""AND"" keyword=""Tomato"" keywordCondition=""like"">2</ExceptionAmount> 
            //    </ExceptionAmounts>
            //    <Tags logic=""AND"">
            //        <Tag tagName=""Total"" do=""SUM"" condition="">="">10</Tag> 
            //        <Tag tagName=""Total"" do=""SUM"" condition=""<="" logic=""AND"">50</Tag> 
            //        <Tag tagName=""TABLE"" condition=""="" logic=""AND"">1</Tag> 
            //    </Tags>
            //    <Keywords logic=""AND"">
            //        <Keyword condition=""like"">Juice</Keyword> 
            //        <Keyword condition=""like"" logic=""OR"">Milk</Keyword> 
            //        <Keyword condition=""unlike"" logic=""AND"">Beer</Keyword> 
            //    </Keywords>
            //</Request>

            var xmlDoc = new XmlDocument();
            XmlElement xmlRoot = xmlDoc.CreateElement("Request");
            xmlDoc.AppendChild(xmlRoot);

            if (criteria.PageIndex > 0)
            {
                var pageNode = Xml.CreateXmlElementWithText(xmlDoc, "Page", criteria.ResultPerPage.ToString()); //20
                pageNode.SetAttribute("index", criteria.PageIndex.ToString());
                xmlRoot.AppendChild(pageNode);
            }

            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "StartUTC", startutc));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "EndUTC", endutc));
            xmlRoot.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "DateTimeSet", criteria.DateTimeSet.ToString()));

            //---------------------------------------------------------------------
            if (criteria.POSCriterias.Count > 0)
            {
                XmlElement registerIdsNode = xmlDoc.CreateElement("RegisterIds");
                registerIdsNode.SetAttribute("logic", POS_Exception.Logics.ToString(POS_Exception.Logic.AND));
                xmlRoot.AppendChild(registerIdsNode);

                foreach (var posCondition in criteria.POSCriterias)
                {
                    XmlElement registerIdNode = Xml.CreateXmlElementWithText(xmlDoc, "RegisterId", posCondition.POSId);
                    registerIdNode.SetAttribute("condition", POS_Exception.Comparatives.ToString(posCondition.Equation));
                    registerIdsNode.AppendChild(registerIdNode);

                    //first confition always "AND"
                    if (posCondition == criteria.POSCriterias.First())
                        continue;

                    registerIdNode.SetAttribute("logic", POS_Exception.Logics.ToString(posCondition.Condition));
                }
            }
            //---------------------------------------------------------------------
            if (criteria.CashierIdCriterias.Count > 0)
            {
                XmlElement cashierIdsNode = xmlDoc.CreateElement("CashierIds");
                cashierIdsNode.SetAttribute("logic", POS_Exception.Logics.ToString(POS_Exception.Logic.AND));

                foreach (var cashierIdCondition in criteria.CashierIdCriterias)
                {
                    if(String.IsNullOrEmpty(cashierIdCondition.CashierId)) continue;

                    XmlElement cashierIdNode = Xml.CreateXmlElementWithText(xmlDoc, "CashierId", cashierIdCondition.CashierId);
                    cashierIdNode.SetAttribute("condition",
                                               POS_Exception.Comparatives.ToString(cashierIdCondition.Equation));
                    cashierIdsNode.AppendChild(cashierIdNode);

                    //first confition always "AND"
                    if (cashierIdCondition == criteria.CashierIdCriterias.First())
                        continue;

                    cashierIdNode.SetAttribute("logic", POS_Exception.Logics.ToString(cashierIdCondition.Condition));
                }

                if (cashierIdsNode.ChildNodes.Count > 0)
                    xmlRoot.AppendChild(cashierIdsNode);
            }
            //---------------------------------------------------------------------
            if (criteria.CashierCriterias.Count > 0)
            {
                XmlElement cashiersNode = xmlDoc.CreateElement("Cashiers");
                cashiersNode.SetAttribute("logic", POS_Exception.Logics.ToString(POS_Exception.Logic.AND));

                foreach (var cashierCondition in criteria.CashierCriterias)
                {
                    if (String.IsNullOrEmpty(cashierCondition.Cashier)) continue;

                    XmlElement cashierNode = Xml.CreateXmlElementWithText(xmlDoc, "Cashier", cashierCondition.Cashier);
                    cashierNode.SetAttribute("condition", POS_Exception.Comparatives.ToString(cashierCondition.Equation));
                    cashiersNode.AppendChild(cashierNode);

                    //first confition always "AND"
                    if (cashierCondition == criteria.CashierCriterias.First())
                        continue;

                    cashierNode.SetAttribute("logic", POS_Exception.Logics.ToString(cashierCondition.Condition));
                }

                if (cashiersNode.ChildNodes.Count > 0)
                    xmlRoot.AppendChild(cashiersNode);
            }
            //---------------------------------------------------------------------
            //if (criteria.ExceptionCriterias.Count > 0)
            //{
            //    XmlElement exceptionsNode = xmlDoc.CreateElement("Exceptions");
            //    exceptionsNode.SetAttribute("logic", POS_Exception.Logics.ToString(POS_Exception.Logic.AND));
            //    xmlRoot.AppendChild(exceptionsNode);

            //    foreach (var exceptionCondition in criteria.ExceptionCriterias)
            //    {
            //        XmlElement exceptionNode = Xml.CreateXmlElementWithText(xmlDoc, "Exception", exceptionCondition.Exception);
            //        exceptionNode.SetAttribute("condition", POS_Exception.Comparatives.ToString(exceptionCondition.Equation));
            //        exceptionsNode.AppendChild(exceptionNode);

            //        if (!String.IsNullOrEmpty(exceptionCondition.Keyword))
            //        {
            //            exceptionNode.SetAttribute("keyword", exceptionCondition.Keyword);
            //            exceptionNode.SetAttribute("keywordCondition", POS_Exception.Comparatives.ToString(exceptionCondition.KeywordEquation));
            //        }
            //        //first confition always "AND"
            //        if (exceptionCondition == criteria.ExceptionCriterias.First())
            //            continue;

            //        exceptionNode.SetAttribute("logic", POS_Exception.Logics.ToString(exceptionCondition.Condition));
            //    }
            //}
            //---------------------------------------------------------------------
            if (criteria.ExceptionAmountCriterias.Count > 0)
            {
                var exceptionAmountsNode = xmlDoc.CreateElement("ExceptionAmounts");
                exceptionAmountsNode.SetAttribute("logic", POS_Exception.Logics.ToString(POS_Exception.Logic.AND));
                xmlRoot.AppendChild(exceptionAmountsNode);

                foreach (var exceptionAmountCondition in criteria.ExceptionAmountCriterias)
                {
                    var amount = exceptionAmountCondition.Amount;
                    if (exceptionAmountCondition.Action == POS_Exception.Do.None)
                        amount = "";

                    var exceptionAmountNode = Xml.CreateXmlElementWithText(xmlDoc, "ExceptionAmount", amount);
                    exceptionAmountNode.SetAttribute("exceptionCondition", POS_Exception.Comparatives.ToString(exceptionAmountCondition.ExceptionEquation));
                    exceptionAmountNode.SetAttribute("exception", exceptionAmountCondition.Exception);

                    if (exceptionAmountCondition.Action != POS_Exception.Do.None)
                    {
                        exceptionAmountNode.SetAttribute("condition", POS_Exception.Comparatives.ToString(exceptionAmountCondition.Equation));
                        exceptionAmountNode.SetAttribute("do", POS_Exception.Dos.ToString(exceptionAmountCondition.Action));
                    }
                    exceptionAmountsNode.AppendChild(exceptionAmountNode);

                    if (!String.IsNullOrEmpty(exceptionAmountCondition.Keyword))
                    {
                        exceptionAmountNode.SetAttribute("keyword", exceptionAmountCondition.Keyword);
                        exceptionAmountNode.SetAttribute("keywordCondition", POS_Exception.Comparatives.ToString(exceptionAmountCondition.KeywordEquation));
                    }
                    //first confition always "AND"
                    if (exceptionAmountCondition == criteria.ExceptionAmountCriterias.First())
                        continue;

                    exceptionAmountNode.SetAttribute("logic", POS_Exception.Logics.ToString(exceptionAmountCondition.Condition));
                }
            }
            //---------------------------------------------------------------------
            if (criteria.TagCriterias.Count > 0)
            {
                XmlElement tagsNode = xmlDoc.CreateElement("Tags");
                tagsNode.SetAttribute("logic", POS_Exception.Logics.ToString(POS_Exception.Logic.AND));

                foreach (var tagCondition in criteria.TagCriterias)
                {
                    if (String.IsNullOrEmpty(tagCondition.TagName)) continue;

                    XmlElement tagNode = Xml.CreateXmlElementWithText(xmlDoc, "Tag", tagCondition.Value);
                    

                    var newException = new POS_Exception { Manufacture = tagCondition.Manufacture };
                    POS_Exception.SetDefaultTags(newException);

                    POS_Exception.TagCriteria condition = tagCondition;
                    var tag = newException.Tags.Where(x => x.Value == condition.TagName).First();
                    if(tag == null)
                    {
                        tagNode.SetAttribute("tagName", tagCondition.TagName);
                    }
                    else
                    {
                        tagNode.SetAttribute("tagName", tag.Key);
                    }

                    if (tagCondition.Equation != POS_Exception.Comparative.None)
                        tagNode.SetAttribute("condition", POS_Exception.Comparatives.ToString(tagCondition.Equation));
                    if (tagCondition.Action != POS_Exception.Do.None)
                        tagNode.SetAttribute("do", POS_Exception.Dos.ToString(tagCondition.Action));
                    tagsNode.AppendChild(tagNode);

                    //first confition always "AND"
                    if (tagCondition == criteria.TagCriterias.First())
                        continue;

                    tagNode.SetAttribute("logic", POS_Exception.Logics.ToString(tagCondition.Condition));
                }

                if (tagsNode.ChildNodes.Count > 0)
                    xmlRoot.AppendChild(tagsNode);
            }
            //---------------------------------------------------------------------
            if (criteria.KeywordCriterias.Count > 0)
            {
                XmlElement keywordsNode = xmlDoc.CreateElement("Keywords");
                keywordsNode.SetAttribute("logic", POS_Exception.Logics.ToString(POS_Exception.Logic.AND));

                foreach (var KeywordCriteria in criteria.KeywordCriterias)
                {
                    if (String.IsNullOrEmpty(KeywordCriteria.Keyword)) continue;

                    XmlElement keywordNode = Xml.CreateXmlElementWithText(xmlDoc, "Keyword", KeywordCriteria.Keyword);
                    keywordNode.SetAttribute("condition", POS_Exception.Comparatives.ToString(KeywordCriteria.Equation));
                    keywordsNode.AppendChild(keywordNode);

                    //first confition always "AND"
                    if (KeywordCriteria == criteria.KeywordCriterias.First())
                        continue;

                    keywordNode.SetAttribute("logic", POS_Exception.Logics.ToString(KeywordCriteria.Condition));
                }

                if (keywordsNode.ChildNodes.Count > 0)
                    xmlRoot.AppendChild(keywordsNode);
            }
            //---------------------------------------------------------------------
            if(criteria.TimeIntervalCriteria != null)
            {
                if(criteria.TimeIntervalCriteria.Enable)
                {
                    XmlElement timeIntervalNode = Xml.CreateXmlElementWithText(xmlDoc, "TimeInterval", criteria.TimeIntervalCriteria.Sec);
                    timeIntervalNode.SetAttribute("logic", POS_Exception.Logics.ToString(POS_Exception.Logic.AND));

                    xmlRoot.AppendChild(timeIntervalNode);
                }
            }
            //---------------------------------------------------------------------
            if (criteria.CountingCriteria != null)
            {
                if (criteria.CountingCriteria.Enable)
                {
                    XmlElement ountingDiscrepanciesNode = Xml.CreateXmlElementWithText(xmlDoc, "OuntingDiscrepancies", criteria.CountingCriteria.Piece);
                    ountingDiscrepanciesNode.SetAttribute("logic", POS_Exception.Logics.ToString(POS_Exception.Logic.AND));

                    xmlRoot.AppendChild(ountingDiscrepanciesNode);
                }
            }
            //---------------------------------------------------------------------

            return xmlDoc;
        }

        public static POS_Exception.TransactionList Parse(XmlDocument reportXml, XmlDocument conditionXml, Int32 timezone)
        {
            //<Result startUTC=""1360281600000"" endUTC=""1360313385000"" total=""1"" pages=""1"" index=""1"" status=""200"">
            //    <Transaction id=""20130208075401711-01394"" cashier=""Peter Frampton"" cashierId=""1"" registerId=""1"" dateTime=""1360310041711"">
            //        <Total>10.37</Total> 
            //        <ExceptionAmount>41.23</ExceptionAmount> 
            //    </Transaction>
            //</Result>
            //-------------------------------------------------------------

            var result = new POS_Exception.TransactionList {RawXml = reportXml};

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

            result.SearchCondition = conditionXml.InnerXml;

            XmlNodeList transactionList = reportXml.GetElementsByTagName("Transaction");
            foreach (XmlElement transactionNode in transactionList)
            {
                var posId = transactionNode.GetAttribute("registerId");
                var utc = Convert.ToUInt64(transactionNode.GetAttribute("dateTime"));
                var counting = Xml.GetFirstElementValueByTagName(transactionNode, "Counting");
                var itemsCount = Xml.GetFirstElementValueByTagName(transactionNode, "ItemsCount");
                result.Results.Add(
                    new POS_Exception.Transaction
                        {
                            Id = transactionNode.GetAttribute("id"),
                            CashierId = transactionNode.GetAttribute("cashierId"),
                            Cashier = transactionNode.GetAttribute("cashier"),
                            POSId = String.IsNullOrEmpty(posId) ?"0" :posId,
                            DateTime = DateTimes.ToDateTime(utc, timezone),
                            UTC = utc,
                            Total = Xml.GetFirstElementValueByTagName(transactionNode, "Total"),
                            ExceptionAmount = Xml.GetFirstElementValueByTagName(transactionNode, "ExceptionAmount"),
                            Counting = (ushort)(String.IsNullOrEmpty(counting) ? 0 : Convert.ToUInt16(counting)),
                            ItemsCount = (ushort)(String.IsNullOrEmpty(itemsCount) ? 0 : Convert.ToUInt16(itemsCount))
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
            return Query.Send(CgiReadTransactionByCondition, xmlDoc, credential);
        }
    }
}
