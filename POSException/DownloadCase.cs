using System;

namespace POSException
{
    public sealed partial class TransactionDetail
    {
        public void DownloadCase(Object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(_transactionId)) return;
            
            var xmlDoc = Report.ReadTransactionById.Search(Report.ReadTransactionById.ObtainCondition(_transactionId), _pts.Credential);
            
            //if no xml dont need send to download case
            if (xmlDoc == null) return;

            App.DownloadCase(null, DateTime.Now, DateTime.Now, xmlDoc);
        }
    }
}
