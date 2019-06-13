
using System;
using System.Collections.Generic;

namespace Constant
{
    public class ReportForm
    {
        public String CGI = "ReadExceptionCumulationByDateGroupByRegister";
        public ReportFormat Format = ReportFormat.PDF;
        public List<String> POS = new List<String>(); //1,2,3
        public List<Int32> Store = new List<Int32>(); //1,2,3
        public List<String> Exceptions = new List<String>();//VOID,CLEAR,LESS
        public String MailReceiver;//people@mail.com
        public String Subject;//Exception Daily Report
        //public String Body;//123
    }

    public class ScheduleReport
    {
        public ReportForm ReportForm = new ReportForm();

        public ReadyState ReadyState = ReadyState.New;
        public ReportPeriod Period = ReportPeriod.Daily;
        public List<UInt16> Days = new List<UInt16> { 0, 1, 2, 3, 4, 5, 6 };//full week sending daily report
        public Int32 Time  = 43200; //12:00
    }

    public class ExceptionReports : List<ExceptionReport>
    {
        public ReadyState ReadyState = ReadyState.New;
    }
    public class ScheduleReports : List<ScheduleReport>
    {
        public ReadyState ReadyState = ReadyState.New;
    }
    
    public class ExceptionReport
    {
        public ReportForm ReportForm = new ReportForm();

        public ReadyState ReadyState = ReadyState.New;
        public String Exception;
        public UInt16 Threshold = 20;
        public UInt16 Increment = 5;
    }

    public enum ReportFormat : ushort 
    {
        PDF,
        Word,
        Excel
    }

    public static class ReportFormats
    {
        public static ReportFormat ToIndex(String value)
        {
            foreach (KeyValuePair<ReportFormat, String> keyValuePair in List)
                if (String.Equals(value, keyValuePair.Value))
                    return keyValuePair.Key;

            return 0;
        }

        public static String ToString(ReportFormat index)
        {
            foreach (KeyValuePair<ReportFormat, String> keyValuePair in List)
            {
                if (index == keyValuePair.Key)
                    return keyValuePair.Value;
            }

            return "";
        }

        public static readonly Dictionary<ReportFormat, String> List = new Dictionary<ReportFormat, String>
                                                             {
                                                                 { ReportFormat.PDF, "PDF" },
                                                                 { ReportFormat.Word, "WORD" },
                                                                 { ReportFormat.Excel, "EXCEL" },
                                                             };
    }

    public enum ReportPeriod : ushort
    {
        Daily,
        Weekly,
        Monthly
    }

    public static class ReportPeriods
    {
        public static ReportPeriod ToIndex(String value)
        {
            foreach (KeyValuePair<ReportPeriod, String> keyValuePair in List)
                if (String.Equals(value, keyValuePair.Value))
                    return keyValuePair.Key;

            return 0;
        }

        public static String ToString(ReportPeriod index)
        {
            foreach (KeyValuePair<ReportPeriod, String> keyValuePair in List)
            {
                if (index == keyValuePair.Key)
                    return keyValuePair.Value;
            }

            return "";
        }

        public static readonly Dictionary<ReportPeriod, String> List = new Dictionary<ReportPeriod, String>
                                                             {
                                                                 { ReportPeriod.Daily, "Daily" },
                                                                 { ReportPeriod.Weekly, "Weekly" },
                                                                 { ReportPeriod.Monthly, "Monthly" },
                                                             };
    }
}
