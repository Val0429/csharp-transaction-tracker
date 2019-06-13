using System;
using System.Collections.Generic;
using System.Xml;
using Constant;

namespace Report
{
    public class SearchRecord
    {
        public String Cgi;
        public String ConditionXml;
        public XmlDocument Result;
    }

    public class Query
    {
        private static readonly Queue<SearchRecord> PreviousResult = new Queue<SearchRecord>();
        private const UInt16 MaximumKeepRecord = 0;
        private const UInt16 CgiTimeout = 300;
        public static XmlDocument Send(String cgi, XmlDocument xmlDoc,ServerCredential credential)
        {
            //foreach (var previou in PreviousResult)
            //{
            //    if (previou.Cgi == cgi)
            //    {
            //        if (previou.ConditionXml == xmlDoc.InnerXml)
            //        {
            //            return previou.Result;
            //        }
            //    }
            //}

            Log.Write("CGI: " + cgi, false, "pts.txt");
            Log.Write("Condition: " + xmlDoc.InnerXml, false, "pts.txt");

            var now = DateTime.Now.ToString("HH:mm:ss");
            var result = Xml.PostXmlToHttp(cgi, xmlDoc, credential, CgiTimeout, false);

            Log.Write("Elapsed Time: " + now + " -> " + DateTime.Now.ToString("HH:mm:ss"), false, "pts.txt");
            if (result != null)
                Log.Write("Result: " + result.InnerXml, false, "pts.txt");
            Log.Write("--------------------------------------------------------------------------------", false, "pts.txt");

            //success
            if (result != null && result.FirstChild != null && ((XmlElement)result.FirstChild).GetAttribute("status") == "200")
            {
                //only keep previous data record for maximum
                //if (PreviousResult.Count > MaximumKeepRecord)
                //    PreviousResult.Dequeue();

                //PreviousResult.Enqueue(new SearchRecord
                //{
                //    Cgi = cgi,
                //    ConditionXml = xmlDoc.InnerXml,
                //    Result = result
                //});
            }

            return result;
        }
    }
}
