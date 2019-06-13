using System;
using System.Collections.Generic;
using Constant;

namespace Interface
{
    public class LiveParameter
    {
        public INVR Nvr;
        public IDevice Device;
    }

    public class PlaybackParameter
    {
        public PlaybackParameter()
        {

        }

        public PlaybackParameter(IDevice device, ulong timestamp)
        {
            Device = device;
            Timecode = timestamp;
        }

        public INVR Nvr;
        public IDevice Device;
        public UInt64 Timecode;
        public TimeUnit TimeUnit;
    }

    //----------------------------------------------------------

    public class ExceptionReportParameter
    {
        public List<String> POS;
        public List<String> Exceptions;
        public UInt64 StartDateTime;
        public UInt64 EndDateTime;
        public DateTimeSet DateTimeSet = DateTimeSet.None;
    }

    public class CashierExceptionReportParameter
    {
        public String CashierId;
        public String Cashier;
        public List<String> Exceptions;
        public UInt64 StartDateTime;
        public UInt64 EndDateTime;
        public DateTimeSet DateTimeSet = DateTimeSet.None;
    }

    public class ExceptionListParameter
    {
        public Int32 Index;
        public List<String> POS;
        public POS_Exception.ExceptionDetail ExceptionDetail;
        public POS_Exception.ExceptionDetailList ExceptionDetailList;
        public List<String> Exceptions;
        public UInt64 StartDateTime;
        public UInt64 EndDateTime;
    }

    public class TransactionListParameter
    {
        public Int32 Index;
        public List<String> POS;
        public POS_Exception.Transaction Transaction;
        public POS_Exception.TransactionList TransactionList;
        public List<String> Exceptions;
        public UInt64 StartDateTime;
        public UInt64 EndDateTime;
        public UInt64 ExceptionDateTime;//normal = StartDateTime, but when focus on exception search list, it exception time
        public POS_Exception.AdvancedSearchCriteria SearchCriteria;
    }
}