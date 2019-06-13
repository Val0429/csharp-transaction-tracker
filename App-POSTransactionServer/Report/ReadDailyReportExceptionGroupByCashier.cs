using System;
using System.Collections.Generic;
using Constant;

namespace App_POSTransactionServer
{
    public partial class POSManager
    {
        public List<POS_Exception.CashierExceptionCountList> ReadDailyReportExceptionGroupByCashier(UInt64 startutc, UInt64 endutc)
        {
            var result = new List<POS_Exception.CashierExceptionCountList>();

            _watch.Reset();
            _watch.Start();
            var conditionXml = Report.ReadDailyReportExceptionGroupByCashier.ObtainCondition(startutc, endutc);
            var reportXml = Report.ReadDailyReportExceptionGroupByCashier.Search(conditionXml, Server.Credential);

            _watch.Stop();
            Console.WriteLine(@"ReadDailyReportExceptionGroupByCashier: " + _watch.Elapsed.TotalSeconds.ToString("0.00"));

            if (reportXml == null)
                return result;

                return Report.ReadDailyReportExceptionGroupByCashier.Parse(reportXml, Server.Server.TimeZone);
        }
    }
}
