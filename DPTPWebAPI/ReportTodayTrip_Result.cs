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
    
    public partial class ReportTodayTrip_Result
    {
        public int srno { get; set; }
        public Nullable<int> userid { get; set; }
        public Nullable<decimal> totalCost { get; set; }
        public string userVechicleNumber { get; set; }
        public string tripStartLocation { get; set; }
        public string tripEndLocation { get; set; }
        public Nullable<System.DateTime> tripDate { get; set; }
        public Nullable<System.DateTime> tripEndDate { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string DriverMobileNo { get; set; }
    }
}
