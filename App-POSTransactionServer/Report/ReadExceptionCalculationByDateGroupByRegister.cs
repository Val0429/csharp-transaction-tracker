using System;
using System.Collections.Generic;
using Constant;

namespace App_POSTransactionServer
{
    public partial class POSManager
    {
        public Dictionary<String, List<POS_Exception.ExceptionCount>> ReadExceptionCalculationByDateGroupByRegister(String[] posIds, String[] cashierIds, String[] cashiers, UInt64 startutc, UInt64 endutc, String[] exceptions)
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

            _watch.Reset();
            _watch.Start();
            var conditionXml = Report.ReadExceptionCalculationByDateGroupByRegister.ObtainCondition(posIds, cashierIds, cashiers, startutc, endutc, exceptions, Server.Server.TimeZone);
            var reportXml = Report.ReadExceptionCalculationByDateGroupByRegister.Search(conditionXml, Server.Credential);
            _watch.Stop();
            Console.WriteLine(@"ReadExceptionCalculationByDateGroupByRegister: " + _watch.Elapsed.TotalSeconds.ToString("0.00"));

            if (reportXml == null)
                return new Dictionary<String, List<POS_Exception.ExceptionCount>>();

            return Report.ReadExceptionCalculationByDateGroupByRegister.Parse(reportXml);
        }
    }
}
