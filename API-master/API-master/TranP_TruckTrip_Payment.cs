//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DPTPWebAPI
{
    using System;
    using System.Collections.Generic;
    
    public partial class TranP_TruckTrip_Payment
    {
        public int srno { get; set; }
        public Nullable<int> userno { get; set; }
        public Nullable<int> tripId { get; set; }
        public string VehicleRegNo { get; set; }
        public Nullable<decimal> TripTotalCost { get; set; }
        public string TripPaymentMode { get; set; }
        public Nullable<decimal> TripPaymentAmount { get; set; }
        public string TripPaymentReciptNo { get; set; }
        public string TripPaymentTransationId { get; set; }
        public string TripPaymentGateway { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}
