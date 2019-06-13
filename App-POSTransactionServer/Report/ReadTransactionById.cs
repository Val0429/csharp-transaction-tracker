using System;
using Constant;

namespace App_POSTransactionServer
{
    public partial class POSManager
    {
        public POS_Exception.TransactionItemList ReadTransactionById(String transactionId)
        {
            var result = new POS_Exception.TransactionItemList();

            _watch.Reset();
            _watch.Start();
            var conditionXml = Report.ReadTransactionById.ObtainCondition(transactionId);
            var reportXml = Report.ReadTransactionById.Search(conditionXml, Server.Credential);

            _watch.Stop();
            Console.WriteLine(@"ReadTransactionById: " + _watch.Elapsed.TotalSeconds.ToString("0.00"));

            if (reportXml == null)
                return result;

            return Report.ReadTransactionById.Parse(reportXml, Server.Server.TimeZone);
        }
    }
}
