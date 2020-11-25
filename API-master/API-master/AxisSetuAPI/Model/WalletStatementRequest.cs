using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppAxisToken.model
{
    

    public class WalletStatementRequest
    {
        public string response_code { get; set; }
        public string response_desc { get; set; }
        public string count { get; set; }
        public WalletStatementResponse response { get; set; }
    }

    public class WalletStatementResponse
    {
        public string vehcileNumber { get; set; }
        public string tagId { get; set; }
        public string tollTxnRef { get; set; }
        public string vehicleClass { get; set; }
        public string avcClass { get; set; }
        public string transactionAmount { get; set; }
        public string plazaCode { get; set; }
        public string plazaName { get; set; }
        public string readerDateTime { get; set; }
        public string txnDateTime { get; set; }
        public string walletId { get; set; }
        public string status { get; set; }
        public string walletBalance { get; set; }
        public string txnType { get; set; }
    }

}
