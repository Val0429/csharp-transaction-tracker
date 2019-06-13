using System;
using System.Collections.Generic;
using Constant;

namespace ServerProfile
{
    public partial class POSManager
    {
        public Dictionary<UInt16, List<POS_Exception.ExceptionCount>> ReadExceptionCalculationByDateGroupByRegister(UInt16[] posIds, String[] cashierIds, String[] cashiers, UInt64 startutc, UInt64 endutc, String[] exceptions)
        {
            CheckDateTimeDontGoToFar(ref startutc, ref endutc);

            _watch.Reset();
            _watch.Start();
            var conditionXml = Report.ReadExceptionCalculationByDateGroupByRegister.ObtainCondition(posIds, cashierIds, cashiers, startutc, endutc, exceptions, Server.Server.TimeZone);
            var reportXml = Report.ReadExceptionCalculationByDateGroupByRegister.Search(conditionXml, Server.Credential);
            _watch.Stop();
            Console.WriteLine(@"ReadExceptionCalculationByDateGroupByRegister: " + _watch.Elapsed.TotalSeconds.ToString("0.00"));

            if (reportXml == null)
                return new Dictionary<UInt16, List<POS_Exception.ExceptionCount>>();

            return Report.ReadExceptionCalculationByDateGroupByRegister.Parse(reportXml);
        }
    }
}
