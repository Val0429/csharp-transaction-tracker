using System;
using System.Collections.Generic;
using Constant;

namespace ServerProfile
{
    public partial class POSManager
    { 
        public Dictionary<UInt16, List<POS_Exception.ExceptionCount>> ReadExceptionCumulationByDateGroupByRegister(UInt16[] posIds, UInt64 startutc, UInt64 endutc, String[] exceptions)
        {
            CheckDateTimeDontGoToFar(ref startutc, ref endutc);

            _watch.Reset();
            _watch.Start();
            var conditionXml = Constant.Report.ReadExceptionCumulationByDateGroupByRegister.ObtainCondition(posIds, startutc, endutc, exceptions, Server.Server.TimeZone);
            var reportXml = Constant.Report.ReadExceptionCumulationByDateGroupByRegister.Search(conditionXml, Server.Credential);
            _watch.Stop();
            Console.WriteLine(@"ReadExceptionCumulationByDateGroupXmlByRegister: " + _watch.Elapsed.TotalSeconds.ToString("0.00"));

            if (reportXml == null)
                return new Dictionary<UInt16, List<POS_Exception.ExceptionCount>>();

            return Constant.Report.ReadExceptionCumulationByDateGroupByRegister.Parse(reportXml);
        }
    }
}
