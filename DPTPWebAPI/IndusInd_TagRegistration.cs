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
    
    public partial class IndusInd_TagRegistration
    {
        public int srno { get; set; }
        public string CustomerMobileNo { get; set; }
        public string CustomerOrderNo { get; set; }
        public string CustomerPaymentType { get; set; }
        public string CustomerPaymentDetails { get; set; }
        public decimal CustomerPaymentAmount { get; set; }
        public string CustomerVehicleTagDetails { get; set; }
        public string CustomerShippingAddress { get; set; }
        public string CustomerAccountType { get; set; }
        public Nullable<int> DistributorId { get; set; }
        public Nullable<System.DateTime> OrderDateTime { get; set; }
        public string OrderStatus { get; set; }
        public string BankStatus { get; set; }
    }
}
