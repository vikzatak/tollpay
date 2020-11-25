using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DPTPWebAPI.Models
{
    public partial class TranP_BookTrip
    {
     
        public TranP_TruckTrips tranP_trucktrip { get; set; }
        public TranP_TruckTrip_Payment tranP_truckTrip_payment { get; set; }
       
    }
}