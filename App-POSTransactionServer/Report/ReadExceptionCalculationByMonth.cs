using System;
using System.Collections.Generic;
using Constant;

namespace App_POSTransactionServer
{
    public partial class POSManager
    {
        public List<POS_Exception.ExceptionCount> ReadExceptionCalculationByMonth(String[] posIds, UInt64 startutc, UInt64 endutc, String[] exceptions)
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

            var result = new List<POS_Exception.ExceptionCount>();

            _watch.Reset();
            _watch.Start();
            var conditionXml = Constant.Report.ReadExceptionCalculationByMonth.ObtainCondition(posIds, startutc, endutc, exceptions, Server.Server.TimeZone);
            var reportXml = Constant.Report.ReadExceptionCalculationByMonth.Search(conditionXml, Server.Credential);

            _watch.Stop();
            Console.WriteLine(@"ReadExceptionCalculationByMonth: " + _watch.Elapsed.TotalSeconds.ToString("0.00"));

            if (reportXml == null)
                return result;

            return Constant.Report.ReadExceptionCalculationByMonth.Parse(reportXml);
        }
    }
}
