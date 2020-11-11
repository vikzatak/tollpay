using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppAxisToken.model
{


 
    

    public class Statusenquiryresponsebody
    {
        public string sessionId { get; set; }
        public string refId { get; set; }
        public string statuscode { get; set; }
        public string statusmessage { get; set; }
        public string channelId { get; set; }
        public string walletId { get; set; }
        public string field1 { get; set; }
        public string field2 { get; set; }
        public string field3 { get; set; }
        public string field4 { get; set; }
        public string field5 { get; set; }
        public string checksum { get; set; }
    }

}
