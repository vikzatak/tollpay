using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppAxisToken.model
{


    public class VehicleStatusRequest
    {
        public Header header { get; set; }
        public VehiclePayload payload { get; set; }
    }

   

    public class VehiclePayload
    {
        public string tagId { get; set; }
        public string vehicleNumber { get; set; }
    }

}
