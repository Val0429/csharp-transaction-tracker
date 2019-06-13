using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Constant;
using Interface;

namespace App_POSTransactionServer
{
    public partial class POSManager
    {
        private const String CgiLoadAllExceptionReport = @"cgi-bin/eventconfig?action=loadalleventhandler"; 
        private const String CgiSaveAllExceptionReport = @"cgi-bin/eventconfig?action=savealleventhandler";
        private const String CgiLoadScheduleReport = @"cgi-bin/reportconfig?action=load";
        private const String CgiSaveScheduleReport = @"cgi-bin/reportconfig?action=save";
        private const String CgiLoadTemplate = @"cgi-bin/reportconfig?action=loadtemplate";
        private const String CgiSaveTemplate = @"cgi-bin/reportconfig?action=savetemplate";

        
        public void LoadScheduleReport()
        {
            //<ScheduleReport>
            //<Report>
            //    <Form>		
            //        <CGI>ReadExceptionCumulationByDateGroupByRegister</CGI>
            //        <Format>WORD</Format>
            //        <Store id="1">
            //        <POS>1,2,3</POS>
            //        </Store>
            //        <Exception>VOID,CLEAR,LESS</Exception>
            //        <SendMail>
            //            <Recipient>egg032978@yahoo.com.tw</Recipient> 
            //            <Subject>Exception Daily Report</Subject> 
            //            <Body>POS: 1,2,3</Body> 
            //        </SendMail>
            //    </Form>
            //    <Schedule>
            //        <Type>Daily</Type>
            //        <DayOfWeek>1,2,3,4,5</DayOfWeek> //Mon,Tue,Wed,Thu,Fri
            //        <Time>43200</Time>
            //    </Schedule>
            //</Report>
            //</ScheduleReport>

            ScheduleReports.Clear();
            ScheduleReports.ReadyState = ReadyState.Ready;

            var xmlDoc = Xml.LoadXmlFromHttp(CgiLoadScheduleReport, Server.Credential);

            if (xmlDoc == null) return;

            XmlNodeList reportList = xmlDoc.GetElementsByTagName("Report");
            foreach (XmlElement reportNode in reportList)
            {
                var formNode = reportNode.SelectSingleNode("Form");
                var scheduleNode = reportNode.SelectSingleNode("Schedule");

                if (formNode == null || scheduleNode == null) continue;

                var scheduleReport = new ScheduleReport
                {
                    Period = ReportPeriods.ToIndex(Xml.GetFirstElementValueByTagName(scheduleNode, "Type")),
                    Time = Convert.ToInt32(Xml.GetFirstElementValueByTagName(scheduleNode, "Time")),
                    ReadyState = ReadyState.Ready
                };

                switch (scheduleReport.Period)
                {
                    case ReportPeriod.Daily:
                    case ReportPeriod.Weekly:
                        var days = Xml.GetFirstElementValueByTagName(scheduleNode, "DayOfWeek");
                        if (String.IsNullOrEmpty(days))
                            scheduleReport.Days.Clear();
                        else
                            scheduleReport.Days = days.Split(',').Select(id => Convert.ToUInt16(id)).ToList();

                        break;

                    case ReportPeriod.Monthly:
                        scheduleReport.Days =
                            Xml.GetFirstElementValueByTagName(scheduleNode, "DayOfMonth").Split(',').Select(
                                id => Convert.ToUInt16(id)).ToList();
                        break;
                }
                
                scheduleReport.Days.Sort();
                ParseReportFormFromXml(scheduleReport.ReportForm, formNode);
                ScheduleReports.Add(scheduleReport);
            }
        }

        
        public void LoadExceptionReport()
        {
            //<AllEventhandler>
            //<EventHandlerConfiguration id="1">
            //    <Report>
            //        <Form>
            //            <CGI>ReadExceptionCumulationByDateGroupByRegister</CGI>
            //            <POS>1</POS>
            //            <Exception>VOID,CLEAR,LESS</Exception>
            //            <SendMail>
            //                <Recipient>egg032978@yahoo.com.tw</Recipient> 
            //                <Subject>Exception Daily Report</Subject> 
            //                <Body>POS: 1,2,3</Body> 
            //            </SendMail>
            //        </Form>
            //        <Condition>
            //            <Exception>VOID</Exception>
            //            <Threshold>20</Threshold>
            //            <Increment>5</Increment>
            //        </Condition>
            //    </Report>
            //</EventHandlerConfiguration>
            //</AllEventhandler>
            foreach (IPOS pos in POSServer)
            {
                pos.ExceptionReports.ReadyState = ReadyState.Ready;
            }

            var xmlDoc = Xml.LoadXmlFromHttp(CgiLoadAllExceptionReport, Server.Credential);

            if (xmlDoc == null) return;
            var eventReportList = xmlDoc.GetElementsByTagName("EventHandlerConfiguration");

            foreach (XmlElement eventReportNode in eventReportList)
            {
                var id = Convert.ToUInt16(eventReportNode.GetAttribute("id"));
                var pos = FindPOSByLicenseId(id);
                if (pos == null) continue;

                pos.ExceptionReports.Clear();

                var reportList = eventReportNode.GetElementsByTagName("Report");
                foreach (XmlElement reportNode in reportList)
                {
                    var formNode = reportNode.SelectSingleNode("Form");
                    var conditionNode = reportNode.SelectSingleNode("Condition");

                    if (formNode == null || conditionNode == null) continue;

                    var exceptionReport = new ExceptionReport
                    {
                        Exception = Xml.GetFirstElementValueByTagName(conditionNode, "Exception"),
                        Threshold = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(conditionNode, "Threshold")),
                        Increment = Convert.ToUInt16(Xml.GetFirstElementValueByTagName(conditionNode, "Increment")),
                        ReadyState = ReadyState.Ready
                    };
                    
                    ParseReportFormFromXml(exceptionReport.ReportForm, formNode);
                    pos.ExceptionReports.Add(exceptionReport);
                }
            }
        }

        
        public void LoadTemplate()
        {
            TemplateConfigs.Clear();
            var xmlDoc = Xml.LoadXmlFromHttp(CgiLoadTemplate, Server.Credential);

            if (xmlDoc == null) return;
            ConvertXmlDocumentToTemplate(xmlDoc);
        }

        //call by reference
        private void CheckDateTimeDontGoToFar(ref UInt64 startutc, ref UInt64 endutc)
        {
            if (startutc > endutc)
            {
                var temp = startutc;
                startutc = endutc;
                endutc = temp;
            }

            var utcnow = DateTimes.ToUtc(Server.Server.DateTime, Server.Server.TimeZone);
            startutc = Math.Min(startutc, utcnow);
            endutc = Math.Min(endutc, utcnow);
        }

        private void SaveScheduleReport()
        {
            if (ScheduleReports.ReadyState == ReadyState.Ready) return;

            ScheduleReports.ReadyState = ReadyState.Ready;


            var xmlDoc = new XmlDocument();

            var xmlRoot = xmlDoc.CreateElement("ScheduleReport");
            xmlDoc.AppendChild(xmlRoot);

            foreach (var scheduleReport in ScheduleReports)
            {
                //if (scheduleReport.ReadyState == ReadyState.Ready) continue;

                var reportNode = ParseScheduleReportToXml(scheduleReport, Server);
                scheduleReport.ReadyState = ReadyState.Ready;

                if (reportNode.FirstChild != null)
                    xmlRoot.AppendChild(xmlDoc.ImportNode(reportNode.FirstChild, true));
            }

            //it dont save, how to delete ALL schedule report?
            //if(xmlRoot.ChildNodes.Count >0)
            Xml.PostXmlToHttp(CgiSaveScheduleReport, xmlDoc, Server.Credential);
        }

        private void SaveExceptionReport()
        {
            var xmlDoc = new XmlDocument();

            var xmlRoot = xmlDoc.CreateElement("AllEventhandler");
            xmlDoc.AppendChild(xmlRoot);

            foreach (IPOS pos in POSServer)
            {
                if (pos.ExceptionReports.ReadyState == ReadyState.Ready) continue;
                pos.ExceptionReports.ReadyState = ReadyState.Ready;

                var eventReportNode = xmlDoc.CreateElement("EventHandlerConfiguration");
                eventReportNode.SetAttribute("id", pos.LicenseId.ToString());
                eventReportNode.SetAttribute("pos", pos.Id.ToString());
                xmlRoot.AppendChild(eventReportNode);

                foreach (var exceptionReport in pos.ExceptionReports)
                {
                    var reportNode = ParseExceptionReportToXml(exceptionReport, Server, pos);
                    exceptionReport.ReadyState = ReadyState.Ready;

                    if (reportNode.FirstChild != null)
                        eventReportNode.AppendChild(xmlDoc.ImportNode(reportNode.FirstChild, true));
                }
            }
            
            if (xmlRoot.ChildNodes.Count > 0)
                Xml.PostXmlToHttp(CgiSaveAllExceptionReport, xmlDoc, Server.Credential);
        }

        public void SaveTemplate()
        {
            var xmlDoc = ConvertTemplateToXmlDocument();
            //what if I want delete ALL template?
            //if (xmlDoc.FirstChild.ChildNodes.Count > 0)
            Xml.PostXmlToHttp(CgiSaveTemplate, xmlDoc, Server.Credential);
        }

        private XmlDocument ParseScheduleReportToXml(ScheduleReport scheduleReport, IServer server)
        {
            //<Report>
            //    <Form ...>
            //    <Schedule>
            //        <Type>Daily</Type>
            //        <DayOfWeek>1,2,3,4,5</DayOfWeek> //Mon,Tue,Wed,Thu,Fri
            //        <Time>43200</Time>
            //    </Schedule>
            //</Report>
            var xmlDoc = new XmlDocument();

            var xmlRoot = xmlDoc.CreateElement("Report");
            xmlDoc.AppendChild(xmlRoot);

            //-------------------------------------------------------------------------
            var formNode = xmlDoc.CreateElement("Form");
            xmlRoot.AppendChild(formNode);
            ParseReportFormToXml(xmlDoc, formNode, scheduleReport.ReportForm, server);
            //-------------------------------------------------------------------------
            var scheduleNode = xmlDoc.CreateElement("Schedule");
            xmlRoot.AppendChild(scheduleNode);

            scheduleNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Type", ReportPeriods.ToString(scheduleReport.Period)));

            String tag = "";
            
            switch (scheduleReport.Period)
            {
                case ReportPeriod.Daily:
                case ReportPeriod.Weekly: tag = "DayOfWeek";
                    break;

                case ReportPeriod.Monthly:
                    tag = "DayOfMonth";
                    break;
            }

            scheduleNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, tag, String.Join(",", scheduleReport.Days.Select(day => day.ToString()).ToArray())));
            scheduleNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Time", scheduleReport.Time.ToString()));
            //-------------------------------------------------------------------------

            return xmlDoc;
        }


        private XmlDocument ParseExceptionReportToXml(ExceptionReport exceptionReport, IServer server, IPOS pos)
        {
        //<Report>
        //    <Form ...>
        //    <Condition>
        //        <POS>1</POS>
        //        <Exception>VOID</Exception>
        //        <Threshold>20</Threshold>
        //        <Increment>5</Increment>
        //    </Condition>
        //</Report>
            var xmlDoc = new XmlDocument();

            var xmlRoot = xmlDoc.CreateElement("Report");
            xmlDoc.AppendChild(xmlRoot);

            //-------------------------------------------------------------------------
            var formNode = xmlDoc.CreateElement("Form");
            xmlRoot.AppendChild(formNode);
            ParseReportFormToXml(xmlDoc, formNode, exceptionReport.ReportForm, server);
            //-------------------------------------------------------------------------
            var conditionNode = xmlDoc.CreateElement("Condition");
            xmlRoot.AppendChild(conditionNode);

            conditionNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Exception", exceptionReport.Exception));
            conditionNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Threshold", exceptionReport.Threshold));
            conditionNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Increment", exceptionReport.Increment));
            //-------------------------------------------------------------------------

            return xmlDoc;
        }

        private void ParseReportFormToXml(XmlDocument xmlDoc, XmlNode formNode, ReportForm reportForm, IServer server)
        {
            //    <Form>
            //        <CGI>ReadExceptionCumulationByDateGroupByRegister</CGI>
            //        <POS>1</POS>
            //        <Exception>VOID,CLEAR,LESS</Exception>
            //        <SendMail>
            //            <Recipient>egg032978@yahoo.com.tw</Recipient> 
            //            <Subject>Exception Daily Report</Subject> 
            //            <Body>POS: 1,2,3</Body> 
            //        </SendMail>
            //    </Form>
            formNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "CGI", reportForm.CGI));
            formNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Format", ReportFormats.ToString(reportForm.Format)));
            //var licenseId = new List<UInt16>();
            //foreach (var id in reportForm.POS)
            //{
            //    var pos = FindPOSById(id);
            //    if(pos == null) continue;
            //    licenseId.Add(pos.LicenseId);
            //}
            //formNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "POS", String.Join(",", licenseId.Select(id => id.ToString()).ToArray())));
            //var registerId = new List<String>(reportForm.POS);
            //registerId.Sort();
            //var posIds = String.Join(",", registerId.Select(id => id.ToString()).ToArray());
            //formNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "POS", posIds));

            foreach (int id in reportForm.Store)
            {
                var store = FindStoreById(id);
                var storeNode = xmlDoc.CreateElement("Store");
                storeNode.SetAttribute("id", store.Id.ToString());
                formNode.AppendChild(storeNode);

                storeNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "POS", String.Join(",", store.Pos.Keys.Select(k=>k.ToString()).ToArray())));
            }

            //var storeId = new List<Int32>(reportForm.Store);
            //var storeIds = String.Join(",", storeId.Select(id => id.ToString()).ToArray());
            //formNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Store", storeIds));

            var exceptionStrings = new List<String>();
            var exceptions = new List<String>();
            foreach (var exception in reportForm.Exceptions)
            {
                if(String.IsNullOrEmpty(POS_Exception.FindExceptionValueByKey(exception))) continue;
                exceptionStrings.Add(POS_Exception.FindExceptionValueByKey(exception));
                exceptions.Add(exception);
            }
            exceptions.Sort();
            formNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Exception", String.Join(",", exceptions.ToArray())));

            String body = String.Join(",", exceptionStrings.ToArray()) + Environment.NewLine;

            if (reportForm.Store.Count > 0)
                body += "(Store: " + String.Join(",", reportForm.Store.Select(k => k.ToString()).ToArray()) + ")" + Environment.NewLine;

            body += " (Server " + server.Credential.Domain + ")";

            var sendMailNode = xmlDoc.CreateElement("SendMail");
            formNode.AppendChild(sendMailNode);

            sendMailNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Recipient", reportForm.MailReceiver));
            sendMailNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Subject", reportForm.Subject));

            sendMailNode.AppendChild(Xml.CreateXmlElementWithText(xmlDoc, "Body", body));
        }

        private void ParseReportFormFromXml(ReportForm reportForm, XmlNode formNode)
        {

            //    <Form>		
            //        <CGI>ReadExceptionCumulationByDateGroupByRegister</CGI>
            //        <Format>WORD</Format>
            //        <Store id="1">
            //        <POS>1,2,3</POS>
            //        </Store>
            //        <Exception>VOID,CLEAR,LESS</Exception>
            //        <SendMail>
            //            <Recipient>egg032978@yahoo.com.tw</Recipient> 
            //            <Subject>Exception Daily Report</Subject> 
            //            <Body>POS: 1,2,3</Body> 
            //        </SendMail>
            //    </Form>


            var sendMailNode = formNode.SelectSingleNode("SendMail");

            reportForm.Format = ReportFormats.ToIndex(Xml.GetFirstElementValueByTagName(formNode, "Format"));
            reportForm.POS.Clear();

            //var licenseId = Xml.GetFirstElementValueByTagName(formNode, "POS").Split(',').Where(id => !String.IsNullOrEmpty(id)).Select(id => Convert.ToUInt16(id));
            //foreach (var id in licenseId)
            //{
            //    var pos = FindPOSByLicenseId(id);
            //    if(pos == null) continue;
            //    reportForm.POS.Add(pos.Id);
            //}

            var stores = formNode.SelectNodes("Store");

            foreach (XmlNode storeNode in stores)
            {
                var id = Convert.ToInt32(storeNode.Attributes["id"].Value);

                var store = FindStoreById(id);
                reportForm.Store.Add(store.Id);
            }

            //var registerId = Xml.GetFirstElementValueByTagName(formNode, "POS").Split(',').Where(id => !String.IsNullOrEmpty(id)).Select(Convert.ToString);
            //foreach (var id in registerId)
            //{
            //    var pos = FindPOSById(id);
            //    if (pos == null) continue;
            //    reportForm.POS.Add(pos.Id);
            //}
            
            //var storeId = Xml.GetFirstElementValueByTagName(formNode, "Store").Split(',').Where(id => !String.IsNullOrEmpty(id)).Select(id => Convert.ToInt32(id));
            //foreach (var id in storeId)
            //{
            //    var store = FindStoreById(id);
            //    if (storeId == null) continue;
            //    reportForm.Store.Add(store.Id);
            //}
            
            var exceptions = Xml.GetFirstElementValueByTagName(formNode, "Exception").Split(',').ToList();
            reportForm.Exceptions = new List<String>();
            foreach (var exception in exceptions)
            {
                if(String.IsNullOrEmpty(exception)) continue;
                reportForm.Exceptions.Add(exception);    
            }

            if (sendMailNode == null) return;

            reportForm.MailReceiver = Xml.GetFirstElementValueByTagName(sendMailNode, "Recipient");
            reportForm.Subject = Xml.GetFirstElementValueByTagName(sendMailNode, "Subject");
            //reportForm.Body = Xml.GetFirstElementValueByTagName(sendMailNode, "Body");
        }

        public XmlDocument ConvertTemplateToXmlDocument()
        {
            var xmlDoc = new XmlDocument();
            var xmlRoot = xmlDoc.CreateElement("Xml");
            xmlDoc.AppendChild(xmlRoot);

            foreach (var config in TemplateConfigs)
            {

                var conditionXml = Report.ReadTransactionByCondition.ObtainCondition(config, config.StartDateTime, config.EndDateTime);
                //var dateTimeSetNode = Xml.CreateXmlElementWithText(conditionXml, "DateTimeSet", config.DateTimeSet.ToString());
                //conditionXml.FirstChild.AppendChild(dateTimeSetNode);

                if (conditionXml.FirstChild != null)
                    xmlRoot.AppendChild(xmlDoc.ImportNode(conditionXml.FirstChild, true));
            }

            return xmlDoc;
        }

        public void ConvertXmlDocumentToTemplate(XmlDocument xmlDoc)
        {
            var requestNodeList = xmlDoc.GetElementsByTagName("Request");
            foreach (XmlNode requestNode in requestNodeList)
            {
                var config = new POS_Exception.TemplateConfig();
                //<Request>
                //    <DateTimeSet>Today</DateTimeSet> 
                var dateTimeSetNode = requestNode.SelectSingleNode("DateTimeSet");
                if (dateTimeSetNode != null)
                {
                    var dateTimeSet = dateTimeSetNode.InnerText;

                    config.DateTimeSet = DateTimeSet.None;
                    switch (dateTimeSet)
                    {
                        case "Today":
                            config.DateTimeSet = DateTimeSet.Today;
                            break;

                        case "Yesterday":
                            config.DateTimeSet = DateTimeSet.Yesterday;
                            break;

                        case "DayBeforeYesterday":
                            config.DateTimeSet = DateTimeSet.DayBeforeYesterday;
                            break;

                        case "ThisWeek":
                            config.DateTimeSet = DateTimeSet.ThisWeek;
                            break;

                        case "ThisMonth":
                            config.DateTimeSet = DateTimeSet.ThisMonth;
                            break;

                        case "LastMonth":
                            config.DateTimeSet = DateTimeSet.LastMonth;
                            break;

                        case "Daily":
                            config.DateTimeSet = DateTimeSet.Daily;
                            break;

                        case "Weekly":
                            config.DateTimeSet = DateTimeSet.Weekly;
                            break;

                        case "Monthly":
                            config.DateTimeSet = DateTimeSet.Monthly;
                            break;

                        default:
                            config.DateTimeSet = DateTimeSet.None;
                            var startUTCNode = requestNode.SelectSingleNode("StartUTC");
                            var endUTCNode = requestNode.SelectSingleNode("EndUTC");
                            if (startUTCNode != null)
                                config.StartDateTime = Convert.ToUInt64(startUTCNode.InnerText);
                            if (endUTCNode != null)
                                config.EndDateTime = Convert.ToUInt64(endUTCNode.InnerText);
                            break;
                    }
                }

                if (config.DateTimeSet != DateTimeSet.None)
                {
                    var range = DateTimes.UpdateStartAndEndDateTime(Server.Server.DateTime, Server.Server.TimeZone, config.DateTimeSet);
                    config.StartDateTime = range[0];
                    config.EndDateTime = range[1];
                }

                //    <RegisterIds logic=""AND"">
                //        <RegisterId condition=""="">1</RegisterId> 
                //        <RegisterId condition=""="" logic=""OR"">2</RegisterId> 
                //    </RegisterIds>
                var registerIdsNode = requestNode.SelectSingleNode("RegisterIds");
                if (registerIdsNode != null)
                {
                    foreach (XmlElement childNode in registerIdsNode.ChildNodes)
                    {
                        var posCriteria = new POS_Exception.POSCriteria
                        {
                            POSId = childNode.InnerText,
                            Condition = (String.IsNullOrEmpty(childNode.GetAttribute("logic")))
                                ? POS_Exception.Logic.OR
                                : POS_Exception.Logics.ToIndex(childNode.GetAttribute("logic")),
                        };
                        switch (childNode.GetAttribute("condition"))
                        {
                            case "=":
                                posCriteria.Equation = POS_Exception.Comparative.Equal;
                                break;

                            case "<>":
                                posCriteria.Equation = POS_Exception.Comparative.NotEqual;
                                break;
                        }

                        if (FindPOSById(posCriteria.POSId) != null)
                            config.POSCriterias.Add(posCriteria);
                    }
                }

                //    <CashierIds logic=""AND"">
                //        <CashierId condition=""="">1</CashierId> 
                //        <CashierId condition=""="" logic=""OR"">3</CashierId> 
                //    </CashierIds>
                var cashierIdsNode = requestNode.SelectSingleNode("CashierIds");
                if (cashierIdsNode != null)
                {
                    foreach (XmlElement childNode in cashierIdsNode.ChildNodes)
                    {
                        var cashierIdCriteria = new POS_Exception.CashierIdCriteria
                        {
                            CashierId = childNode.InnerText,
                            Condition = (String.IsNullOrEmpty(childNode.GetAttribute("logic")))
                                ? POS_Exception.Logic.OR
                                : POS_Exception.Logics.ToIndex(childNode.GetAttribute("logic")),
                            //Condition = POS_Exception.Logics.ToIndex(childNode.GetAttribute("logic"));
                        };

                        switch (childNode.GetAttribute("condition"))
                        {
                            case "=":
                                cashierIdCriteria.Equation = POS_Exception.Comparative.Equal;
                                break;

                            case "<>":
                                cashierIdCriteria.Equation = POS_Exception.Comparative.NotEqual;
                                break;
                        }
                        config.CashierIdCriterias.Add(cashierIdCriteria);
                    }
                }

                //    <Cashiers logic=""AND"">
                //        <Cashier condition=""like"">Peter</Cashier> 
                //        <Cashier condition=""like"" logic=""OR"">Mary</Cashier> 
                //    </Cashiers>
                var cashiersNode = requestNode.SelectSingleNode("Cashiers");
                if (cashiersNode != null)
                {
                    foreach (XmlElement childNode in cashiersNode.ChildNodes)
                    {
                        var cashierCriteria = new POS_Exception.CashierCriteria
                        {
                            Cashier = childNode.InnerText,
                            Condition = (String.IsNullOrEmpty(childNode.GetAttribute("logic")))
                                ? POS_Exception.Logic.OR
                                : POS_Exception.Logics.ToIndex(childNode.GetAttribute("logic")),
                        };

                        switch (childNode.GetAttribute("condition"))
                        {
                            case "like":
                                cashierCriteria.Equation = POS_Exception.Comparative.Include;
                                break;

                            case "unlike":
                                cashierCriteria.Equation = POS_Exception.Comparative.Exclude;
                                break;
                        }
                        config.CashierCriterias.Add(cashierCriteria);
                    }
                }

                //    <ExceptionAmounts logic=""AND"">
                //        <ExceptionAmount exception=""VOID"" do=""SUM"" condition="">="">5</ExceptionAmount> 
                //        <ExceptionAmount exception=""VOID"" do=""COUNT"" condition="">="" logic=""AND"">1</ExceptionAmount> 
                //        <ExceptionAmount exception=""VOID"" do=""SUM"" condition="">="" logic=""AND"" keyword=""Tomato"" keywordCondition=""like"">2</ExceptionAmount> 
                //        <ExceptionAmount exception=""VOID"" do=""COUNT"" condition="">="" logic=""AND"" keyword=""Tomato"" keywordCondition=""like"">2</ExceptionAmount> 
                //    </ExceptionAmounts>
                var exceptionAmountsNode = requestNode.SelectSingleNode("ExceptionAmounts");
                if (exceptionAmountsNode != null)
                {
                    foreach (XmlElement childNode in exceptionAmountsNode.ChildNodes)
                    {
                        var exceptionAmountCriteria = new POS_Exception.ExceptionAmountCriteria
                        {
                            Amount = childNode.InnerText,
                            Exception = childNode.GetAttribute("exception"),
                            Equation = POS_Exception.Comparatives.ToIndex(childNode.GetAttribute("condition")),
                            Action = POS_Exception.Dos.ToIndex(childNode.GetAttribute("do")),
                            Condition = (String.IsNullOrEmpty(childNode.GetAttribute("logic")))
                                ? POS_Exception.Logic.AND
                                : POS_Exception.Logics.ToIndex(childNode.GetAttribute("logic")),
                            Keyword = childNode.GetAttribute("keyword"),
                        };

                        switch (childNode.GetAttribute("exceptionCondition"))
                        {
                            case "=":
                                exceptionAmountCriteria.Equation = POS_Exception.Comparative.Exists;
                                break;

                            case "<>":
                                exceptionAmountCriteria.Equation = POS_Exception.Comparative.NotExists;
                                break;
                        }

                        switch (childNode.GetAttribute("keywordCondition"))
                        {
                            case "to":
                                exceptionAmountCriteria.KeywordEquation = POS_Exception.Comparative.To;
                                break;

                            case "like":
                                exceptionAmountCriteria.KeywordEquation = POS_Exception.Comparative.Include;
                                break;

                            case "unlike":
                                exceptionAmountCriteria.KeywordEquation = POS_Exception.Comparative.Exclude;
                                break;
                        }

                        config.ExceptionAmountCriterias.Add(exceptionAmountCriteria);
                    }
                }

                //    <Tags logic=""AND"">
                //        <Tag tagName=""Total"" do=""SUM"" condition="">="">10</Tag> 
                //        <Tag tagName=""Total"" do=""SUM"" condition=""<="" logic=""AND"">50</Tag> 
                //        <Tag tagName=""TABLE"" condition=""="" logic=""AND"">1</Tag> 
                //    </Tags>
                var tagsNode = requestNode.SelectSingleNode("Tags");
                if (tagsNode != null)
                {
                    foreach (XmlElement childNode in tagsNode.ChildNodes)
                    {
                        var tagCriteria = new POS_Exception.TagCriteria
                        {
                            TagName = childNode.GetAttribute("tagName"),
                            Value = childNode.InnerText,
                            Equation = POS_Exception.Comparatives.ToIndex(childNode.GetAttribute("condition")),
                            Action = POS_Exception.Dos.ToIndex(childNode.GetAttribute("do")),
                            Condition = (String.IsNullOrEmpty(childNode.GetAttribute("logic")))
                                ? POS_Exception.Logic.AND
                                : POS_Exception.Logics.ToIndex(childNode.GetAttribute("logic")),
                        };

                        config.TagCriterias.Add(tagCriteria);
                    }
                }

                //    <Keywords logic=""AND"">
                //        <Keyword condition=""like"">Juice</Keyword> 
                //        <Keyword condition=""like"" logic=""OR"">Milk</Keyword> 
                //        <Keyword condition=""unlike"" logic=""AND"">Beer</Keyword> 
                //    </Keywords>
                var keywordsNode = requestNode.SelectSingleNode("Keywords");
                if (keywordsNode != null)
                {
                    foreach (XmlElement childNode in keywordsNode.ChildNodes)
                    {
                        var keywordCriteria = new POS_Exception.KeywordCriteria
                        {
                            Keyword = childNode.InnerText,
                            Condition = (String.IsNullOrEmpty(childNode.GetAttribute("logic")))
                                ? POS_Exception.Logic.AND
                                : POS_Exception.Logics.ToIndex(childNode.GetAttribute("logic")),
                        };

                        switch (childNode.GetAttribute("condition"))
                        {
                            case "like":
                                keywordCriteria.Equation = POS_Exception.Comparative.Include;
                                break;

                            case "unlike":
                                keywordCriteria.Equation = POS_Exception.Comparative.Exclude;
                                break;

                            case "=":
                                keywordCriteria.Equation = POS_Exception.Comparative.Equal;
                                break;

                            case "<>":
                                keywordCriteria.Equation = POS_Exception.Comparative.NotEqual;
                                break;

                        }
                        config.KeywordCriterias.Add(keywordCriteria);
                    }
                }

                //    <TimeInterval logic=""AND"">5</TimeInterval>
                var timeInterval = Xml.GetFirstElementValueByTagName(requestNode, "TimeInterval");
                if (!String.IsNullOrEmpty(timeInterval))
                {
                    config.TimeIntervalCriteria.Sec = Convert.ToUInt16(timeInterval);
                }

                //    <CountingDiscrepancies logic=""AND""></CountingDiscrepancies>
                var ountingDiscrepancies = Xml.GetFirstElementValueByTagName(requestNode, "CountingDiscrepancies");
                if (!String.IsNullOrEmpty(ountingDiscrepancies))
                {
                    config.CountingCriteria.Piece = Convert.ToUInt16(ountingDiscrepancies);
                }

                //</Request>

                TemplateConfigs.Add(config);
            }
        }
    }
}
