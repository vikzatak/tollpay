using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppAxisToken.model
{
  

    public class WalletRepsonse
    {
        public Responsestatus responseStatus { get; set; }
        public Response response { get; set; }
    }

    public class Responsestatus
    {
        public string statusCode { get; set; }
        public string responseMessage { get; set; }
        public string requestId { get; set; }
    }

    public class WalletResponse
    {
        public string balance { get; set; }
        public string clearBalance { get; set; }
        public string unclearBalance { get; set; }
        public string ODBalance { get; set; }
        public string thresholdAmount { get; set; }
        public string SDBalance { get; set; }
    }

}
