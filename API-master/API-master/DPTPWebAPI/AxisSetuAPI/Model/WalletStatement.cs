using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppAxisToken.model
{

    public class WalletStatement
    {
        public Header header { get; set; }
        public WalletStatementPayload payload { get; set; }
    }

   

    public class WalletStatementPayload
    {
        public string vehicleNumber { get; set; }
        public string tagId { get; set; }
        public string walletId { get; set; }
        public string startTimeStamp { get; set; }
        public string endTimeStamp { get; set; }
    }

}
