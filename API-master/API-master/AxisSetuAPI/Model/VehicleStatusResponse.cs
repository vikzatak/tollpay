using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppAxisToken.model
{
 

    public class VehicleStatusResponse
    {
        public Response response { get; set; }
    }

    public class Response
    {
        public string statusCode { get; set; }
        public string responseMessage { get; set; }
        public string requestId { get; set; }
        public string npciMapperStatus { get; set; }
        public string bankStatus { get; set; }
    }

}
