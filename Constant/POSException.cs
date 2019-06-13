
using System;
using System.Collections.Generic;

namespace Constant
{
    public class POSException
    {
        public UInt16 Id;
        public String Name = "";
        public String Manufacture = "UNKNOWN";
        public UInt16 Worker = 4; //fixed
        public UInt16 Buffer = 512; //fixed
        public List<Exception> Exceptions;
        public List<Segment> Segments;
        public List<Tag> Tags;

        public POSException()
        {
            Exceptions = new List<Exception>();

            Segments = new List<Segment>();

            Tags = new List<Tag>();
        }

        public override String ToString()
        {

            return Id + @" " + Name;
        }

        //----------------------------------------------------------------------------------------
        public static String[] Manufactures = new[] { "MaitreD" };
        public static String[] ExceptionList = new[] { "VOID", "CLEAR", "LESS", "COUPON", "NO SALE" };

        public static void SetDefaultExceptions(POSException posException)
        {
            switch (posException.Manufacture)
            {
                case "MaitreD":
                    posException.Exceptions = new List<Exception>
                    {
                        new Exception {Key = "VOID", Dir = "++", Value = "VOID"},
                        new Exception {Key = "CLEAR", Dir = "--", Value = "CLEAR"},
                        new Exception {Key = "LESS", Dir = "+", Value = "LESS"},
                        new Exception {Key = "COUPON", Dir = "=", Value = "COUPON"},
                        new Exception {Key = "NO SALE", Dir = "", Value = "NO SALE"},
                    };
                    break;
            }
        }

        public static void SetDefaultSegments(POSException posException)
        {
            switch (posException.Manufacture)
            {
                case "MaitreD":
                    posException.Segments = new List<Segment>
                    {
                        new Segment {Key="ID", Value = ","},
                        new Segment {Key="BEGIN", Value = "--Begin--"},
                        new Segment {Key="END", Value = "--End--"},
                    };
                    break;
            }
        }

        public static void SetDefaultTags(POSException posException)
        {
            switch (posException.Manufacture)
            {
                case "MaitreD":
                    posException.Tags = new List<Tag>
                    {
                        new Tag {Key = "VISA", Value = "VISA"},
                        new Tag {Key = "MASTER", Value = "MATER CARD"},
                        new Tag {Key = "CASH", Value = "CASH"},
                        new Tag {Key = "CHECK", Value = "CHECK"},
                        new Tag {Key = "TOTAL", Value = "TOTAL"},
                        new Tag {Key = "TABLE", Value = "TABLE"},
                        new Tag {Key = "ORDER", Value = "ORDER"},
                    };
                    break;
            }
        }

        public class Exception
        {
            public String Dir = "";
            public String Key = ""; //VOID
            public String Value = ""; //V.O.I.D
        }

        public class Segment
        {
            public String Key = "";
            public String Value = "";
        }

        public class Tag
        {
            public String Key = "";
            public String Value = "";
        }
        //-------------------------------------------------------------------
        public class ExceptionDailyList
        {
            public UInt16 POSId;
            public UInt64 StartDateTime;
            public UInt64 EndDateTime;
            public Dictionary<Exception, UInt16> ExceptionList = new Dictionary<Exception, UInt16>();
        }
        //-------------------------------------------------------------------
        public class ExceptionDetail
        {
            public String Id;
            public UInt16 POSId;
            public DateTime DateTime;
            public String Amount;
            public String ExceptionAmount;
        }

        public class ExceptionDetailList
        {
            public UInt32 Count;
            public UInt16 Pages;
            public UInt16 PageIndex;
            public String SearchCondition; //XML
            public TimeSpan Elapsed;
            public String[] Keywords;
            public List<ExceptionDetail> Results = new List<ExceptionDetail>();
        }
        //-------------------------------------------------------------------
        public class TransactionDetail
        {
            public POS POS;
            public DateTime DateTime;
            public String Content;
        }
    }
}
