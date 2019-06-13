using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Constant;

namespace App_POSTransactionServer
{
    public partial class POSManager
    {
        public XmlDocument ReadTransactionByCondition(POS_Exception.AdvancedSearchCriteria criteria, UInt64 startutc, UInt64 endutc, ref XmlDocument conditionXml)
        {
            CheckDateTimeDontGoToFar(ref startutc, ref endutc);

            var cleatpos = false;
            //add all pos
            if (criteria.POSCriterias.Count == 0)
            {
                cleatpos = true;
                foreach (var pos in POSServer)
                {
                    criteria.POSCriterias.Add(new POS_Exception.POSCriteria { POSId = pos.Id });
                    if (UsePTSId(pos.Id))
                        criteria.POSCriterias.Add(new POS_Exception.POSCriteria { POSId = "PTS" });
                }
            }
            else
            {
                var tempString = new List<String>();
                foreach (POS_Exception.POSCriteria posCriteria in criteria.POSCriterias)
                {
                    if (UsePTSId(posCriteria.POSId))
                        tempString.Add("PTS");
                }

                if (tempString.Count > 0)
                {
                    var exist = criteria.POSCriterias.Any(posCriteria => posCriteria.POSId == "PTS");
                    if(!exist)
                        criteria.POSCriterias.Add(new POS_Exception.POSCriteria { POSId = "PTS" });
                }

            }

            conditionXml = Report.ReadTransactionByCondition.ObtainCondition(criteria, startutc, endutc);
            var reportXml = Report.ReadTransactionByCondition.Search(conditionXml, Server.Credential);

            if (cleatpos)
                criteria.POSCriterias.Clear();

            return reportXml;
        }

        public POS_Exception.TransactionList ReadTransactionByCondition(POS_Exception.AdvancedSearchCriteria criteria)
        {
            var result = new POS_Exception.TransactionList();

            _watch.Reset();
            _watch.Start();

            var conditionXml = new XmlDocument();
            var reportXml = ReadTransactionByCondition(criteria, criteria.StartDateTime, criteria.EndDateTime, ref conditionXml);

            _watch.Stop();
            Console.WriteLine(@"ReadTransactionByCondition: " + _watch.Elapsed.TotalSeconds.ToString("0.00"));

            if (reportXml == null)
                return result;

            return Report.ReadTransactionByCondition.Parse(reportXml, conditionXml, Server.Server.TimeZone);
        }

        public XmlDocument ReadTransactionByCondition(XmlDocument conditionXml)
        {
            //var result = new POS_Exception.TransactionList();

            _watch.Reset();
            _watch.Start();

            var reportXml = Report.ReadTransactionByCondition.Search(conditionXml, Server.Credential);

            _watch.Stop();
            Console.WriteLine(@"ReadTransactionByCondition: " + _watch.Elapsed.TotalSeconds.ToString("0.00"));

            return reportXml;
            //if (reportXml == null)
            //    return result;

            //return Constant.Report.ReadTransactionByCondition.Parse(reportXml, conditionXml, Server.Server.TimeZone, _watch.Elapsed);
        }
    }
}
