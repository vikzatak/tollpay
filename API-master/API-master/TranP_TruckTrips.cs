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
    
    public partial class TranP_TruckTrips
    {
        public int srno { get; set; }
        public Nullable<int> userid { get; set; }
        public string userVechicleNumber { get; set; }
        public string VehicleType { get; set; }
        public string VehicleName { get; set; }
        public string tripStartLocation { get; set; }
        public string tripEndLocation { get; set; }
        public Nullable<System.DateTime> tripDate { get; set; }
        public Nullable<System.DateTime> tripEndDate { get; set; }
        public string MaterialType { get; set; }
        public Nullable<double> Weight { get; set; }
        public Nullable<double> Height { get; set; }
        public Nullable<double> Length { get; set; }
        public Nullable<double> Width { get; set; }
        public Nullable<decimal> FareCost { get; set; }
        public string DeliveryType { get; set; }
        public string MsgForTransporter { get; set; }
        public string DriverMobileNo { get; set; }
        public string DriverOwnerMobNo { get; set; }
        public string loadingby { get; set; }
        public string tripStatus { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}