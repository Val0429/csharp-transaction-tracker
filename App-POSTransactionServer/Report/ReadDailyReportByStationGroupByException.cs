﻿using System;
using System.Collections.Generic;
using Constant;

namespace App_POSTransactionServer
{
    public partial class POSManager
    {
        public POS_Exception.ExceptionCountList ReadDailyReportByStationGroupByException(String[] posIds, UInt64 startutc, UInt64 endutc, String[] exceptions)
        {
            CheckDateTimeDontGoToFar(ref startutc, ref endutc);

            var tempString = new List<String>();
            foreach (String posId in posIds)
            {
                tempString.Add(posId);
                if(UsePTSId(posId))
                    tempString.Add("PTS");
            }

            posIds = tempString.ToArray();

            var result = new POS_Exception.ExceptionCountList
            {
                POSIds = posIds,
                StartDateTime = startutc,
                EndDateTime = endutc,
            };
            
            _watch.Reset();
            _watch.Start();

            var conditionXml = Report.ReadDailyReportByStationGroupByException.ObtainCondition(posIds, startutc, endutc, exceptions);
            var reportXml = Report.ReadDailyReportByStationGroupByException.Search(conditionXml, Server.Credential);
                
            _watch.Stop();
            Console.WriteLine(@"ReadDailyReportByStationGroupByException: " + _watch.Elapsed.TotalSeconds.ToString("0.00"));

            if (reportXml == null)
                return result;

            result.ExceptionList = Report.ReadDailyReportByStationGroupByException.Parse(reportXml);

            return result;
        }
    }
}