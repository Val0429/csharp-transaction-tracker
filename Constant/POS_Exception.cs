
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml;

namespace Constant
{
    public class POS_Exception
    {
        public UInt16 Id;
        public String Name = "";
        public String Separator = "";
        public ReadyState ReadyState;
        public String Manufacture = "UNKNOWN";
        public UInt16 Worker = 4; //fixed
        public UInt16 Buffer = 5120; //fixed
        public List<Exception> Exceptions;
        public List<Segment> Segments;
        public List<Tag> Tags;
        public UInt16 TransactionType; //1 – Single, 2 – Mutli, 3 – Raw
        private static Dictionary<String, List<Exception>> _exceptionList = new Dictionary<String, List<Exception>>();
        private static Dictionary<String, List<Tag>> _tagList = new Dictionary<String, List<Tag>>();
        protected const String ExceptionPath = @"./PosData/exception.xml";
        protected const String TagPath = @"./PosData/tag.xml";
        public Boolean IsCapture;
        public Boolean IsSupportPOSId;
        private const String CgiLoadAllException = @"cgi-bin/posconfig?action=loadallexception";
        public ServerCredential Credential { get; set; }


        public POS_Exception()
        {
            ReadyState = ReadyState.New;

            Exceptions = new List<Exception>();

            Segments = new List<Segment>();

            Tags = new List<Tag>();

            TransactionType = 0;

            IsCapture = false;
            IsSupportPOSId = true;

            var exceptionPath =
                String.Format(System.Reflection.Assembly.GetExecutingAssembly().Location.Replace("Constant.dll", "")) + ExceptionPath;

            if (!File.Exists(exceptionPath))
            {
                exceptionPath = ExceptionPath;
            }

            if (File.Exists(exceptionPath))
            {
                XmlDocument node = Xml.LoadXmlFromFile(exceptionPath);
                if(node != null)
                {
                    var brands = node.SelectNodes("Exceptions/Brand");
                    if (brands != null)
                    {
                        foreach (XmlElement brandNode in brands)
                        {
                            var brand = brandNode.GetAttribute("value");
                            if(_exceptionList.ContainsKey(brand)) continue;

                            var isEditable = brandNode.GetAttribute("editable") == "true";
                            var list = new List<Exception>();
                            var exceptions = brandNode.SelectNodes("Exception");
                            if(exceptions != null)
                            {
                                foreach (XmlElement exceptionNode in exceptions)
                                {
                                    var key = exceptionNode.GetAttribute("key");
                                    var dir = exceptionNode.GetAttribute("dir");
                                    var valueType = exceptionNode.GetAttribute("valueType");

                                    var exception = new Exception
                                                        {
                                                            Key = key,
                                                            Dir = dir,
                                                            ValueType = valueType,
                                                            Editable = isEditable,
                                                            Value = exceptionNode.InnerText
                                                        };

                                    list.Add(exception);
                                }
                            }

                            _exceptionList.Add(brand, list);
                        }
                    }
                }
            }

           

            var tagPath =
                String.Format(System.Reflection.Assembly.GetExecutingAssembly().Location.Replace("Constant.dll", "")) + TagPath;

            if (!File.Exists(tagPath))
            {
                tagPath = TagPath;
            }

            if (File.Exists(tagPath))
            {
                XmlDocument node = Xml.LoadXmlFromFile(tagPath);
                if (node != null)
                {
                    var brands = node.SelectNodes("Tags/Brand");
                    if (brands != null)
                    {
                        foreach (XmlElement brandNode in brands)
                        {
                            var brand = brandNode.GetAttribute("value");
                            if (_tagList.ContainsKey(brand)) continue;

                            var isEditable = brandNode.GetAttribute("editable") == "true";
                            var list = new List<Tag>();
                            var tags = brandNode.SelectNodes("Tag");
                            if (tags != null)
                            {
                                foreach (XmlElement tagNode in tags)
                                {
                                    var key = tagNode.GetAttribute("key");
                                    var dir = tagNode.GetAttribute("dir");
                                    var valueType = tagNode.GetAttribute("valueType");

                                    var tag = new Tag
                                    {
                                        Key = key,
                                        Dir = dir,
                                        ValueType = valueType,
                                        Editable = isEditable,
                                        Value = tagNode.InnerText
                                    };

                                    list.Add(tag);
                                }
                            }

                            _tagList.Add(brand, list);
                        }
                    }
                }
            }
        }

        public override String ToString()
        {
            return Id.ToString().PadLeft(2, '0') + @" " + Name;
        }


     

        //----------------------------------------------------------------------------------------

        public static String ToDisplay(String manufacture)
        {
            switch (manufacture)
            {
                case "MaitreD":
                    return "Maitre'D";

                default:
                    return manufacture;
            }
        }

        public static String ToIndex(String manufacture)
        {
            switch (manufacture)
            {
                case "Maitre'D":
                    return "MaitreD";

                default:
                    return manufacture;
            }
        }

        //----------------------------------------------------------------------------------------
        public static String[] Manufactures = new[] { "ActiveMQ", "BHG", "MaitreD", "Micros", "Oracle", "Radiant", "Retalix", "everrich", "Oracle Demo", "Generic", "NEC" };//Menards
        
        public static String FindExceptionValueByKey(String key)
        {
            //var configPath =
            //   String.Format(System.Reflection.Assembly.GetExecutingAssembly().Location.Replace("Constant.dll", "")) + ExceptionPath;

            //Log.Write("file Path: " + configPath);
            //Log.Write("file exists: " + File.Exists(configPath));

            var result = key;

            foreach (KeyValuePair<string, List<Exception>> exceptions in _exceptionList)
            {
                foreach (Exception exception in exceptions.Value)
                {
                    if(exception.Key == key)
                    {
                        result = exception.Value;
                        break;
                    }
                }
            }

            return result;
        }

        public static void SetDefaultExceptions(POS_Exception posException)
        {
            if (_exceptionList.ContainsKey(posException.Manufacture))
            {
                posException.Exceptions = new List<Exception>();
                foreach (Exception exception in _exceptionList[posException.Manufacture])
                {
                    posException.Exceptions.Add(new Exception
                                                        {
                                                            Key = exception.Key,
                                                            Dir = exception.Dir,
                                                            ValueType = exception.ValueType,
                                                            Editable = exception.Editable,
                                                            Value = exception.Value
                                                        });
                }
                
                posException.Exceptions.Sort((x, y) => (y.Key.CompareTo(x.Key)));
                return;
            }
           
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
                        new Exception {Key = "200DISCO", Dir = "=", Value = "2.00 $ DISCO"},
                        new Exception {Key = "15DISCOUNT", Dir = "=", Value = "15DISCOUNT"},
                        new Exception {Key = "QUALITY DISC", Dir = "=", Value = "QUALITY DISC"},
                        new Exception {Key = "ADMINISTRATI", Dir = "=", Value = "ADMINISTRATI"},
                        new Exception {Key = "WAITER DISC.", Dir = "=", Value = "WAITER DISC."},
                        new Exception {Key = "Charge Disco", Dir = "=", Value = "Charge Disco"},
                        new Exception {Key = "Free 4 Wings", Dir = "=", Value = "Free 4 Wings"},
                        new Exception {Key = "PROMO BEER", Dir = "=", Value = "PROMO BEER"},
                        new Exception {Key = "Gratificatio", Dir = "=", Value = "Gratificatio"},
                        new Exception {Key = "MANAGER DISC", Dir = "=", Value = "MANAGER DISC"},
                        new Exception {Key = "COUTERSY DIS", Dir = "=", Value = "COUTERSY DIS"},
                        new Exception {Key = "OFFER BY MAN", Dir = "=", Value = "OFFER BY MAN"},
                    };
                    break;

                case "ActiveMQ":
                    posException.Exceptions = new List<Exception>
                    {
                        new Exception {Key = "CERTIFICATE BARCODED", Dir="=", Value = "CERTIFICATE BARCODED", ValueType = "$", Editable = false},
                        new Exception {Key = "CHARGE SALE TRANSACTION", Dir = "=", Value = "CHARGE SALE TRANSACTION", ValueType = "$", Editable = false},
                        new Exception {Key = "COUPON", Dir = "=", Value = "COUPON", ValueType = "$", Editable = false},
                        new Exception {Key = "DAMAGED PRODUCT", Dir = "+", Value = "DAMAGED PRODUCT", ValueType = "$", Editable = false},
                        new Exception {Key = "DISCOUNT", Dir = "+", Value = "DISCOUNT", ValueType = "$", Editable = false},
                        new Exception {Key = "ISSUE BAR-CODED CK", Dir = "=", Value = "ISSUE BAR-CODED CK", ValueType = "$", Editable = false},
                        new Exception {Key = "MENARD REBATE", Dir = "=", Value = "MENARD REBATE", ValueType = "$", Editable = false},
                        new Exception {Key = "MERCHANDISE RETURN", Dir = "+", Value = "MERCHANDISE RETURN", ValueType = "$", Editable = false},
                        new Exception {Key = "NO SALE", Dir = "+", Value = "NO SALE", ValueType = "$", Editable = false},
                        new Exception {Key = "OPEN SKU SALES", Dir="=", Value = "OPEN SKU SALES", ValueType = "$", Editable = false},
                        new Exception {Key = "PRICE OVERRIDE", Dir = "+", Value = "Price Override", ValueType = "$", Editable = false},
                        new Exception {Key = "RETURN CHARGE", Dir = "+", Value = "RETURN CHARGE", ValueType = "$", Editable = false},
                        new Exception {Key = "SALE NO RECEIPT RETURN", Dir = "+", Value = "SALE NO RECEIPT RETURN", ValueType = "$", Editable = false},
                        new Exception {Key = "SALE TRANSACTION", Dir="=", Value = "SALE TRANSACTION", ValueType = "$", Editable = false},
                        new Exception {Key = "VOID", Dir = "+", Value = "VOID", ValueType = "$", Editable = false},
                        new Exception {Key = "VOID SALE", Dir = "+", Value = "VOID SALE", ValueType = "$", Editable = false},
                    };
                    break;
                    
                case "Micros":
                    posException.Exceptions = new List<Exception>
                    {
                        new Exception {Key = "15% GRATUITY", Dir = "==", Value = "15% GRATUITY", ValueType = "$", Editable = false},
                        new Exception {Key = "EMPLOYEE", Dir = "==", Value = "EMPLOYEE", ValueType = "$", Editable = false},
                        new Exception {Key = "OPEN $ FOOD", Dir = "==", Value = "OPEN $ FOOD", ValueType = "$", Editable = false},
                        new Exception {Key = "NO SALE", Dir = "", Value = "NO SALE", Editable = false},
                        new Exception {Key = "PAID IN", Dir = "=", Value = "PAID IN", ValueType = "$", Editable = false},
                        new Exception {Key = "PAID OUT", Dir = "=", Value = "PAID OUT", ValueType = "$", Editable = false},
                        new Exception {Key = "VOID", Dir = "++", Value = "-V", ValueType = "$", Editable = false},
                        new Exception {Key = "CANCELLED", Dir = "=", Value = "CANCELLED", ValueType = "$", Editable = false},
                    };
                    break;

                case "Oracle":
                case "Oracle Demo":
                    posException.Exceptions = new List<Exception>
                    {
                        new Exception {Key = "CLOSE TRANSACTION", Dir = "+", Value = "CLOSE", ValueType="", Editable = false},
                        new Exception {Key = "DISCOUNT", Dir = "+", Value = "DISCOUNT", ValueType="$", Editable = false},
                        new Exception {Key = "OPEN TRANSACTION", Dir = "+", Value = "OPEN", ValueType="", Editable = false},
                        new Exception {Key = "PULL TRANSACTION", Dir = "+", Value = "PULL", ValueType="", Editable = false},
                        new Exception {Key = "RETURN", Dir = "+", Value = "(Return)", ValueType="$", Editable = false},
                        new Exception {Key = "RETURN TRANSACTION", Dir = "+", Value = "RETURN", ValueType="", Editable = false},
                        new Exception {Key = "VOID", Dir = "+", Value = "(Void)", ValueType="$", Editable = false},
                        new Exception {Key = "VOID TRANSACTION", Dir = "+", Value = "VOID", ValueType="", Editable = false},
                    };
                    break;

                case "Radiant":
                    posException.Exceptions = new List<Exception>
                    {
                        new Exception {Key = "SECURITY INTERFACE ONLINE", Dir = "", Value = "SECURITY INTERFACE ONLINE", Editable = false},
                        new Exception {Key = "VOID", Dir = "", Value = "VOID", Editable = false},
                        new Exception {Key = "PRICE OVERRIDE", Dir = "", Value = "PRICE OVERRIDE", Editable = false},
                        new Exception {Key = "QUANTITY CHANGE", Dir = "", Value = "QUANTITY CHANGE", Editable = false},
                        new Exception {Key = "COUPON", Dir = "", Value = "COUPON", Editable = false},
                        new Exception {Key = "DISCOUNT", Dir = "", Value = "DISCOUNT", Editable = false},
                        new Exception {Key = "COUPON DELETION", Dir = "", Value = "COUPON DELETION", Editable = false},
                        new Exception {Key = "DISCOUNT DELETION", Dir = "", Value = "DISCOUNT DELETION", Editable = false},
                        new Exception {Key = "TERMINAL LOCK", Dir = "", Value = "TERMINAL LOCK", Editable = false},
                        new Exception {Key = "TERMINAL UNLOCK", Dir = "", Value = "TERMINAL UNLOCK", Editable = false},
                        new Exception {Key = "SAFE DROP REQUIRED", Dir = "", Value = "SAFE DROP REQUIRED", Editable = false},
                        new Exception {Key = "DRAWER OPEN", Dir = "", Value = "DRAWER OPEN", Editable = false},
                        new Exception {Key = "DRAWER CLOSE", Dir = "", Value = "DRAWER CLOSE", Editable = false},
                        new Exception {Key = "DRAWER OPEN TOO LONG", Dir = "", Value = "DRAWER OPEN TOO LONG", Editable = false},
                        new Exception {Key = "POS RESTART", Dir = "", Value = "POS RESTART", Editable = false},
                        new Exception {Key = "SERVER OFFLINE", Dir = "", Value = "SERVER OFFLINE", Editable = false},
                        new Exception {Key = "MONEY ORDER OFFLINE", Dir = "", Value = "MONEY ORDER OFFLINE", Editable = false},
                        new Exception {Key = "VERIFIED CUSTOMER AGE", Dir = "", Value = "VERIFIED CUSTOMER AGE", Editable = false},
                        new Exception {Key = "PAYMENT", Dir = "", Value = "PAYMENT", Editable = false},
                    };
                    break;

                case "Retalix":
                    posException.Exceptions = new List<Exception>
                    {
                        new Exception {Key = "VOID", Dir = "", Value = "VOID", Editable = false},
                        new Exception {Key = "COUPON", Dir = "", Value = "COUPON", Editable = false},
                        new Exception {Key = "NO SALE", Dir = "", Value = "NO SALE", Editable = false},
                        new Exception {Key = "REFUND", Dir = "", Value = "REFUND", Editable = false},
                        new Exception {Key = "WASTE", Dir = "", Value = "WASTE", Editable = false},
                        new Exception {Key = "VOID TRANSACTION", Dir = "", Value = "VOID TRANSACTION", Editable = false},
                        new Exception {Key = "PRICE OVERRIDE", Dir = "", Value = "PRICE OVERRIDE", Editable = false},
                        new Exception {Key = "OPEN BANK", Dir = "", Value = "OPEN BANK", Editable = false},
                        new Exception {Key = "CLOSE BANK", Dir = "", Value = "CLOSE BANK", Editable = false},
                        new Exception {Key = "FLOAT", Dir = "", Value = "FLOAT", Editable = false},
                        new Exception {Key = "DROP", Dir = "", Value = "DROP", Editable = false},
                        new Exception {Key = "ITEM QTY SETUP", Dir = "", Value = "ITEM QTY SETUP", Editable = false},
                        new Exception {Key = "LINE DISCOUNT", Dir = "", Value = "LINE DISCOUNT", Editable = false},
                    };
                    break;

                case "BHG":
                    posException.Exceptions = new List<Exception>
                    {
                        new Exception {Key = "COUPON", Dir = "", Value = "立减券", Editable = false},
                    };
                    break;

                case "everrich":
                    posException.Exceptions = new List<Exception>
                    {
                        new Exception {Key = "DRAWER OPEN", Dir = "", Value = "DRAWER OPEN", Editable = false},
                        new Exception {Key = "DRAWER CLOSE", Dir = "", Value = "DRAWER CLOSE", Editable = false},
                    };
                    break;
            }
            posException.Exceptions.Sort((x, y) => (y.Key.CompareTo(x.Key)));
        }

        public static void SetDefaultSegments(POS_Exception posException)
        {
            switch (posException.Manufacture)
            {
                case "MaitreD":
                case "Generic":
                    posException.Segments = new List<Segment>
                    {
                        new Segment {Key="ID", Value = ","},
                        new Segment {Key="BEGIN", Value = "--Begin--"},
                        new Segment {Key="END", Value = "--End--"},
                    };
                    break;

                default:
                    posException.Segments = new List<Segment>();
                    break;
            }
        }

        public static void SetDefaultTags(POS_Exception posException)
        {
            if (_tagList.ContainsKey(posException.Manufacture))
            {
                posException.Tags = new List<Tag>();
                foreach (Tag tag in _tagList[posException.Manufacture])
                {
                    posException.Tags.Add(new Tag
                    {
                        Key = tag.Key,
                        Dir = tag.Dir,
                        ValueType = tag.ValueType,
                        Editable = tag.Editable,
                        Value = tag.Value
                    });
                }
                
                posException.Tags.Sort((x, y) => (y.Key.CompareTo(x.Key)));
                return;
            }

            switch (posException.Manufacture)
            {
                case "MaitreD":
                    posException.Tags = new List<Tag>
                    {
                        new Tag {Key = "VISA", Value = "VISA"},
                        new Tag {Key = "MASTER", Value = "MASTER CARD"},
                        new Tag {Key = "CASH", Value = "CASH"},
                        new Tag {Key = "CHECK", Value = "CHECK"},
                        new Tag {Key = "TOTAL", Value = "TOTAL"},
                        new Tag {Key = "TABLE", Value = "TABLE"},
                        new Tag {Key = "ORDER", Value = "ORDER"},
                    };
                    break;

                case "ActiveMQ":
                    posException.Tags = new List<Tag>
                    {
                        new Tag {Key = "AMERICAN EXPRESS", Dir="=", Value = "AMERICAN EXPRESS", ValueType = "$", Editable = false},
                        new Tag {Key = "CASH", Dir="=", Value = "CASH", ValueType = "$", Editable = false},
                        new Tag {Key = "CHANGE", Dir="=", Value = "CHANGE", ValueType = "$", Editable = false},
                        new Tag {Key = "CHARGE", Dir="=", Value = "CHARGE", ValueType = "$", Editable = false},
                        new Tag {Key = "CHECK", Dir="=", Value = "CHECK", ValueType = "$", Editable = false},
                        new Tag {Key = "DEBIT", Dir="=", Value = "DEBIT", ValueType = "$", Editable = false},
                        new Tag {Key = "DISCOVER NETWORK", Dir="=", Value = "DISCOVER NETWORK", ValueType = "$", Editable = false},
                        new Tag {Key = "EMPLOYEE NUMBER", Dir="=", Value = "EMPLOYEE NUMBER", ValueType = "", Editable = false},
                        new Tag {Key = "MASTER CARD", Dir="=", Value = "MASTER CARD", ValueType = "$", Editable = false},
                        new Tag {Key = "MENARD CARD PAYMENT TRANSACTION", Dir="=", Value = "MENARD CARD PAYMENT TRANSACTION", ValueType = "$", Editable = false},
                        new Tag {Key = "MENARD CONTRACTOR CARD", Dir="=", Value = "MENARD CONTRACTOR CARD", ValueType = "$", Editable = false},
                        new Tag {Key = "MENARD GIFT CARD", Dir="=", Value = "MENARD GIFT CARD", ValueType = "$", Editable = false},
                        new Tag {Key = "MERCHANDISE RETURN TRANSACTION", Dir="=", Value = "MERCHANDISE RETURN TRANSACTION", ValueType = "$", Editable = false},
                        new Tag {Key = "MISC. SALES", Dir="=", Value = "MISC. SALES", ValueType = "$", Editable = false},
                        new Tag {Key = "NO RECEIPT RETURN TRANSACTION", Dir="=", Value = "NO RECEIPT RETURN TRANSACTION", ValueType = "$", Editable = false},
                        new Tag {Key = "PAYMENT", Dir="=", Value = "PAYMENT", ValueType = "$", Editable = false},
                        new Tag {Key = "PAYMENT TRANSACTION", Dir="=", Value = "PAYMENT TRANSACTION", ValueType = "$", Editable = false},
                        new Tag {Key = "REGISTER ID", Dir="=", Value = "REGISTER ID", ValueType = "$", Editable = false},
                        new Tag {Key = "RETURN CHARGE TRANSACTION", Dir="=", Value = "RETURN CHARGE TRANSACTION", ValueType = "$", Editable = false},
                        new Tag {Key = "STORE NUMBER", Dir="=", Value = "STORE NUMBER", ValueType = "$", Editable = false},
                        new Tag {Key = "TAX", Dir="=", Value = "TAX", ValueType = "$", Editable = false},
                        new Tag {Key = "TENDER OUTSIDE SALE TRANSACTION", Dir="=", Value = "TENDER OUTSIDE SALE TRANSACTION", ValueType = "$", Editable = false},
                        new Tag {Key = "TOTAL", Dir="=", Value = "TOTAL", ValueType = "$", Editable = false},
                        new Tag {Key = "TOTAL SALE", Dir="=", Value = "TOTAL SALE", ValueType = "$", Editable = false},
                        new Tag {Key = "TRANSACTION NUMBER", Dir="=", Value = "TRANSACTION NUMBER", ValueType = "$", Editable = false},
                        new Tag {Key = "VISA", Dir="=", Value = "VISA", ValueType = "$", Editable = false},
                        new Tag {Key = "VOID PAYMENT TRANSACTION", Dir="=", Value = "VOID PAYMENT TRANSACTION", ValueType = "$", Editable = false},
                        new Tag {Key = "VOID SALE TRANSACTION", Dir="=", Value = "VOID SALE TRANSACTION", ValueType = "$", Editable = false},
                    };
                    break;

                case "Micros":
                    posException.Tags = new List<Tag>
                    {
                        new Tag {Key = "AMEX", Dir="=", Value = "AMEX", ValueType = "$", Editable = false},
                        new Tag {Key = "CASH", Dir="=", Value = "CASH", ValueType = "$", Editable = false},
                        new Tag {Key = "CASH ADVANCE", Dir="=", Value = "CASH ADVANCE", ValueType = "$", Editable = false},
                        new Tag {Key = "CHANGE DUE", Dir="=", Value = "CHANGE DUE", ValueType = "$", Editable = false},
                        new Tag {Key = "CHECK", Dir="=", Value = "CHECK", Editable = false},
                        new Tag {Key = "DISCOVER", Dir="=", Value = "DISCOVER", ValueType = "$", Editable = false},
                        new Tag {Key = "HOUSE CHARGE", Dir="=", Value = "HOUSE CHARGE", ValueType = "$", Editable = false},
                        new Tag {Key = "MASTERCARD", Dir="=", Value = "MASTERCARD", ValueType = "$", Editable = false},
                        new Tag {Key = "ORDER",Value = "ORDER", Editable = false},
                        new Tag {Key = "TOTAL DUE", Dir="=", Value = "TOTAL DUE", ValueType = "$", Editable = false},
                        new Tag {Key = "SUBTOTAL", Dir="=", Value = "SUBTOTAL", ValueType = "$", Editable = false},
                        new Tag {Key = "TABLE", Dir="=", Value = "TABLE", Editable = false},
                        new Tag {Key = "TAX", Dir="=", Value = "TAX", ValueType = "$", Editable = false},
                        new Tag {Key = "TIP", Dir="=", Value = "TIP", ValueType = "$", Editable = false},
                        new Tag {Key = "TO GO", Value = "TO GO", Editable = false},
                        new Tag {Key = "AMOUNT PAID", Dir="=", Value = "AMOUNT PAID", ValueType = "$", Editable = false},
                        new Tag {Key = "VISA", Dir="=", Value = "VISA", ValueType = "$", Editable = false},
                    };
                    break;

                case "Oracle":
                case "Oracle Demo":
                    posException.Tags = new List<Tag>
                    {
                        new Tag {Key = "AMEX",Dir="=", Value = "AmEx",  ValueType = "$", Editable = false},
                        new Tag {Key = "CASH",Dir="=", Value = "Cash",  ValueType = "$", Editable = false},
                        new Tag {Key = "CASHIER ID", Dir="=",Value = "Cashier ID",  ValueType = "", Editable = false},
                        new Tag {Key = "CHANGE", Dir="=",Value = "Change Due",  ValueType = "$", Editable = false},
                        new Tag {Key = "CHECK",Dir="=", Value = "Check",  ValueType = "$", Editable = false},
                        new Tag {Key = "DEBIT CARD",Dir="=", Value = "Debit Card",  ValueType = "$", Editable = false},
                        new Tag {Key = "DINERS CLUB",Dir="=", Value = "DinersClub",  ValueType = "$", Editable = false},
                        new Tag {Key = "DISCOVER",Dir="=", Value = "DISCOVER",  ValueType = "$", Editable = false},
                        new Tag {Key = "HOUSE CARD",Dir="=", Value = "HouseCard",  ValueType = "$", Editable = false},
                        new Tag {Key = "JCB",Dir="=", Value = "JCB",  ValueType = "$", Editable = false},
                        new Tag {Key = "MASTERCARD",Dir="=", Value = "MasterCard",  ValueType = "$", Editable = false},
                        new Tag {Key = "REGISTER",Dir="=", Value = "Reg.",  ValueType = "$", Editable = false},
                        new Tag {Key = "SALE TRANSACTION",Dir="=", Value = "SALE",  ValueType = "", Editable = false},
                        new Tag {Key = "STORE",Dir="=", Value = "Store",  ValueType = "", Editable = false},
                        new Tag {Key = "SUBTOTAL",Dir="=", Value = "Subtotal",  ValueType = "$", Editable = false},
                        new Tag {Key = "TAX",Dir="=", Value = "Tax",  ValueType = "$", Editable = false},
                        new Tag {Key = "TOTAL",Dir="=", Value = "Total",  ValueType = "$", Editable = false},
                        new Tag {Key = "TOTAL DISCOUNT",Dir="=", Value = "Total Discount",  ValueType = "$", Editable = false},
                        new Tag {Key = "TOTAL TAX",Dir="=", Value = "Total Tax",  ValueType = "$", Editable = false},
                        new Tag {Key = "TOTAL TENDER",Dir="=", Value = "Total Tender",  ValueType = "$", Editable = false},
                        new Tag {Key = "TOTAL TRANSACTION",Dir="=", Value = "TOTAL",  ValueType = "", Editable = false},
                        new Tag {Key = "TRANSACTION NUMBER",Dir="=", Value = "Trans.",  ValueType = "", Editable = false},
                        new Tag {Key = "VISA",Dir="=", Value = "VISA",  ValueType = "$", Editable = false},
                    };
                    break;

                case "Radiant":
                    posException.Tags = new List<Tag>
                    {
                        new Tag {Key = "TOTAL", Value = "TOTAL", Editable = false},
                        new Tag {Key = "TAX TOTAL", Value = "TAX TOTAL", Editable = false},
                        new Tag {Key = "SUBTOTAL", Value = "SUBTOTAL", Editable = false},
                        new Tag {Key = "SIGN OUT", Value = "SIGN OUT", Editable = false},
                        new Tag {Key = "SIGN IN", Value = "SIGN IN", Editable = false},
                        new Tag {Key = "ORDER", Value = "ORDER", Editable = false},
                        new Tag {Key = "CHANGE", Value = "CHANGE", Editable = false},
                        new Tag {Key = "CASH", Value = "CASH", Editable = false},
                        new Tag {Key = "CREDIT", Value = "CREDIT", Editable = false},
                        new Tag {Key = "DEBIT", Value = "DEBIT", Editable = false},
                    };
                    break;

                case "Retalix":
                    posException.Tags = new List<Tag>
                    {
                        new Tag {Key = "CHECK", Value = "CHECK", Editable = false},
                        new Tag {Key = "CASH", Value = "CASH", Editable = false},
                        new Tag {Key = "CHANGE", Value = "CHANGE", Editable = false},
                        new Tag {Key = "ORDER", Value = "ORDER", Editable = false},
                        new Tag {Key = "SUBTOTAL", Value = "SUBTOTAL", Editable = false},
                        new Tag {Key = "TOTAL", Value = "TOTAL", Editable = false},
                        new Tag {Key = "TAX", Value = "TAX", Editable = false},
                        new Tag {Key = "TAX TOTAL", Value = "TAX TOTAL", Editable = false},
                        new Tag {Key = "SIGN IN", Value = "SIGN IN", Editable = false},
                        new Tag {Key = "SIGN OUT", Value = "SIGN OUT", Editable = false},
                        new Tag {Key = "OPERATOR SELECT", Value = "OPERATOR SELECT", Editable = false},
                        new Tag {Key = "END OF SHIFT", Value = "END OF SHIFT", Editable = false},
                        new Tag {Key = "CREDIT CARD", Value = "CREDIT CARD", Editable = false},
                    };
                    break;

                case "BHG":
                    posException.Tags = new List<Tag>
                    {
                        new Tag {Key = "TRANSACTION NUMBER", Value = "票號", Editable = false},
                        new Tag {Key = "TOTAL", Value = "總金額", Editable = false},
                        new Tag {Key = "CASHIER ID", Value = "收銀員", Editable = false},
                        new Tag {Key = "POS ID", Value = "款台", Editable = false},
                    };
                    break;

                case "everrich":
                    posException.Tags = new List<Tag>
                    {
                        new Tag {Key = "TRANSACTION NUMBER", Value = "交易序號", Editable = false},
                        new Tag {Key = "TOTAL", Value = "交易總金額", Editable = false},
                        new Tag {Key = "TICKET NUMBER", Value = "單據號碼", Editable = false},
                        new Tag {Key = "CASH", Value = "現金", Editable = false},
                        new Tag {Key = "CHANGE", Value = "找零", Editable = false},
                        new Tag {Key = "CREDIT", Value = "信用卡", Editable = false},
                        new Tag {Key = "DEPARTURE", Value = "離境班機號碼", Editable = false},
                        new Tag {Key = "POS ID", Value = "機台編號", Editable = false},
                        new Tag {Key = "SALES ID", Value = "售貨員代碼", Editable = false},
                        new Tag {Key = "STORE ID", Value = "庫別", Editable = false},
                    };
                    break;
            }
            posException.Tags.Sort((x, y) => (y.Key.CompareTo(x.Key)));
        }

        public class Exception
        {
            public String Dir = ""; // =
            public String Key = ""; //VOID
            public String Value = ""; //V.O.I.D
            public String ValueType = ""; //$
            public Boolean Editable = true;
            public String TagEnd = ""; //,
        }

        public class Segment
        {
            public String Key = "";
            public String Value = "";
            public Boolean Editable = true;
            public String TagEnd = ""; //,
        }

        public class Tag
        {
            public String Dir = ""; // =
            public String Key = "";
            public String Value = "";
            public String ValueType = ""; //$
            public Boolean Editable = true;
            public String TagEnd = ""; //,
        }
        //-------------------------------------------------------------------
        public class ExceptionThreshold
        {
            public Int32 ThresholdValue1 = 5;
            public Int32 ThresholdValue2 = 20;
            public Color Color;
        }
        //-------------------------------------------------------------------
        public class ExceptionCount
        {
            public String Exception;
            public Int32 Count;
            public String DateTime; //yyyy-MM-DD
        }
        public class CashierCount
        {
            public String CashierId;
            public String Cashier;
            public Int32 Count;
        }
        //-------------------------------------------------------------------
        public class ExceptionCountWithThreshold : ExceptionCount
        {
            public ExceptionThreshold Threshold;
        }
        //-------------------------------------------------------------------
        public class ExceptionCountList
        {
            public String[] POSIds;
            public UInt64 StartDateTime;
            public UInt64 EndDateTime;
            public List<ExceptionCount> ExceptionList = new List<ExceptionCount>();
        }
        //-------------------------------------------------------------------
        public class CashierExceptionCountList
        {
            public String CashierId;
            public String Cashier;
            public UInt64 StartDateTime;
            public UInt64 EndDateTime;
            public List<ExceptionCount> ExceptionList = new List<ExceptionCount>();
        }
        //-------------------------------------------------------------------
        public class ExceptionCountWithThresholdList
        {
            public String[] POSIds;
            public UInt64 StartDateTime;
            public UInt64 EndDateTime;
            public List<ExceptionCountWithThreshold> ExceptionList = new List<ExceptionCountWithThreshold>();
        }
        //-------------------------------------------------------------------
        public class Transaction
        {
            public String Id;
            public String POSId;
            public DateTime DateTime;
            public UInt64 UTC;
            public String Total;
            public String ExceptionAmount;
            public String CashierId;
            public String Cashier;
            public UInt16 Counting;
            public UInt16 ItemsCount;
        }

        //-------------------------------------------------------------------
        public class TransactionItem
        {
            public String POS;
            public UInt64 UTC;
            public DateTime DateTime;
            public String Content;
            public String TransactionTime;
            public String Cashier;
            public String CashierId;
            public String ValueType;
            public Int32 Seq;//for sorting
        }

        public class TransactionList
        {
            public UInt32 Count;
            public Int32 Pages = 1;
            public Int32 PageIndex = 1;
            public String SearchCondition; //XML
            public XmlDocument RawXml;
            public List<Transaction> Results = new List<Transaction>();
        }
        //-------------------------------------------------------------------
        public class TransactionItemList
        {
            public UInt64 StartDateTime; //= 0;
            public UInt64 EndDateTime;//= 0;
            public List<TransactionItem> TransactionItems = new List<TransactionItem>();
        }
        //-------------------------------------------------------------------
        public class ExceptionDetail
        {
            public String TransactionId;
            public String Type;
            public String Cashier;
            public String CashierId;
            public String POSId;
            public DateTime DateTime;
            public UInt64 UTC;
            public String ExceptionAmount;
            public String TotalTransactionAmount;
        }

        public class ExceptionDetailList
        {
            public UInt32 Count;
            public Int32 Pages;
            public Int32 PageIndex;
            public String SearchCondition; //XML
            public TimeSpan Elapsed;
            public String[] Keywords;
            public XmlDocument RawXml;
            public List<ExceptionDetail> Results = new List<ExceptionDetail>();
        }
        //-------------------------------------------------------------------
        public class DateTimeCriteria
        {
            public UInt64 StartDateTime;// = 0;
            public UInt64 EndDateTime;//= 0;
            public DateTimeSet DateTimeSet = DateTimeSet.None;// Today, Yesterday, DayBeforeYesterday, ThisWeek
        }

        public class SearchCriteria : DateTimeCriteria
        {
            public readonly List<Int32> Store = new List<Int32>();
            public readonly List<String> POS = new List<String>();
            public readonly List<String> Cashiers = new List<String>();
            public readonly List<String> CashierIds = new List<String>();
            public readonly List<String> Exceptions = new List<String>();
        }

        //-------------------------------------------------------------------
        public class AdvancedSearchCriteria : DateTimeCriteria
        {
            public Int32 PageIndex = 1;
            public UInt16 ResultPerPage = 20;
            public List<POSCriteria> POSCriterias = new List<POSCriteria>();

            public List<CashierIdCriteria> CashierIdCriterias = new List<CashierIdCriteria>();
            public List<CashierCriteria> CashierCriterias = new List<CashierCriteria>();
            public List<ExceptionCriteria> ExceptionCriterias = new List<ExceptionCriteria>();
            public List<ExceptionAmountCriteria> ExceptionAmountCriterias = new List<ExceptionAmountCriteria>();
            public List<TagCriteria> TagCriterias = new List<TagCriteria>();
            public List<KeywordCriteria> KeywordCriterias = new List<KeywordCriteria>();
            public TimeIntervalCriteria TimeIntervalCriteria = new TimeIntervalCriteria();
            public CountingCriteria CountingCriteria = new CountingCriteria();
        }

        //-------------------------------------------------------------------

        public class POSCriteria
        {
            public String POSId;
            public Logic Condition = Logic.OR;
            public Comparative Equation = Comparative.Equal; //=, <>
        }

        //-------------------------------------------------------------------

        public class CashierIdCriteria
        {
            public String CashierId;
            public Logic Condition = Logic.OR;
            public Comparative Equation = Comparative.Equal; //=, <>
        }

        //-------------------------------------------------------------------

        public class CashierCriteria
        {
            public String Cashier;
            public Logic Condition = Logic.OR;
            public Comparative Equation = Comparative.Include; //=, <>, like, unlike
        }

        //-------------------------------------------------------------------

        public class ExceptionCriteria
        {
            public String Exception;
            public Logic Condition = Logic.AND;
            public Comparative Equation = Comparative.Exists; //=, <>
            public String Keyword;
            public Comparative KeywordEquation = Comparative.Include; //like, unlike
        }

        //-------------------------------------------------------------------

        public class ExceptionAmountCriteria
        {
            public Comparative ExceptionEquation = Comparative.Exists; //=, <>
            public String Exception;
            public String Amount;
            public Do Action = Do.SUM; //SUM, COUNT
            public Logic Condition = Logic.AND;
            public Comparative Equation = Comparative.Greater; //=, <>,  >, >=, <, <=
            public String Keyword;
            public Comparative KeywordEquation = Comparative.Include; //like, unlike
        }

        //-------------------------------------------------------------------

        public class TagCriteria
        {
            public String Manufacture;
            public String TagName;
            public String Value;
            public Do Action = Do.SUM; //SUM, COUNT
            public Logic Condition = Logic.AND;
            public Comparative Equation = Comparative.Equal; //=, <>,  >, >=, <, <=
        }

        //-------------------------------------------------------------------

        public class KeywordCriteria
        {
            public String Keyword;
            public Logic Condition = Logic.AND;
            public Comparative Equation = Comparative.Equal; //=, <>,  like, unlike
        }

        //-------------------------------------------------------------------

        public class TimeIntervalCriteria
        {
            public Boolean Enable;
            public UInt16 Sec;
            public Logic Condition = Logic.AND;
        }

        //-------------------------------------------------------------------

        public class CountingCriteria
        {
            public Boolean Enable;
            public UInt16 Piece;
            public Logic Condition = Logic.AND;
        }

        //-------------------------------------------------------------------

        public class TemplateConfig : AdvancedSearchCriteria
        {
            public ReadyState ReadyState = ReadyState.New;
        }
        
        //-------------------------------------------------------------------
        public enum Logic : short
        {
            None, //
            AND, //AND
            OR, //OR
        }

        public class Logics
        {
            public static Logic ToIndex(String value)
            {
                foreach (KeyValuePair<Logic, String> keyValuePair in List)
                {
                    if (String.Equals(value, keyValuePair.Value))
                        return keyValuePair.Key;
                }

                return Logic.None;
            }

            public static String ToString(Logic index)
            {
                foreach (KeyValuePair<Logic, String> keyValuePair in List)
                {
                    if (index == keyValuePair.Key)
                        return keyValuePair.Value;
                }

                return "";
            }

            public static readonly Dictionary<Logic, String> List = new Dictionary<Logic, String>
                                                             {
                                                                 { Logic.AND, "AND" },
                                                                 { Logic.OR, "OR" },
                                                             };
        }

        public enum Do : short
        {
            None, //
            SUM, //SUM
            COUNT, //COUNT
        }

        public class Dos
        {
            public static Do ToIndex(String value)
            {
                foreach (KeyValuePair<Do, String> keyValuePair in List)
                {
                    if (String.Equals(value, keyValuePair.Value))
                        return keyValuePair.Key;
                }

                return Do.None;
            }

            public static String ToString(Do index)
            {
                foreach (KeyValuePair<Do, String> keyValuePair in List)
                {
                    if (index == keyValuePair.Key)
                        return keyValuePair.Value;
                }

                return "";
            }

            public static readonly Dictionary<Do, String> List = new Dictionary<Do, String>
                                                             {
                                                                 { Do.None, "" },
                                                                 { Do.SUM, "SUM" },
                                                                 { Do.COUNT, "COUNT" },
                                                             };
        }

        public enum Comparative : short
        {
            None, //
            Equal, //  =
            NotEqual, // <>
            Exists, //  =
            NotExists, // <>
            Greater, // >
            GreaterOrEqual, // >=
            Less, //<
            LessOrEqual, //<=

            To, //to
            Like, //like
            Unlink, //unlink
            Include, //like
            Exclude, //unlink
        }

        public class Comparatives
        {
            public static Comparative ToIndex(String value)
            {
                foreach (KeyValuePair<Comparative, String> keyValuePair in List)
                {
                    if (String.Equals(value, keyValuePair.Value))
                        return keyValuePair.Key;
                }

                return Comparative.None;
            }

            public static String ToString(Comparative index)
            {
                foreach (KeyValuePair<Comparative, String> keyValuePair in List)
                {
                    if (index == keyValuePair.Key)
                        return keyValuePair.Value;
                }

                return "";
            }

            public static readonly Dictionary<Comparative, String> List = new Dictionary<Comparative, String>
                                                             {
                                                                 { Comparative.Equal, "=" },
                                                                 { Comparative.NotEqual, "<>" },
                                                                 { Comparative.Exists, "=" },
                                                                 { Comparative.NotExists, "<>" },
                                                                 { Comparative.Greater, ">" },
                                                                 { Comparative.GreaterOrEqual, ">=" },
                                                                 { Comparative.Less, "<" },
                                                                 { Comparative.LessOrEqual, "<=" },
                                                                 { Comparative.To, "to" },
                                                                 { Comparative.Like, "like" },
                                                                 { Comparative.Unlink, "unlike" },
                                                                 { Comparative.Include, "like" },
                                                                 { Comparative.Exclude, "unlike" },
                                                             };
        }
    }
}
