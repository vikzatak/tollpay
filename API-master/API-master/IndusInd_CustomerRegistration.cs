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
    
    public partial class IndusInd_CustomerRegistration
    {
        public int srno { get; set; }
        public Nullable<int> TollPayCustId { get; set; }
        public string TransactionID { get; set; }
        public Nullable<System.DateTime> TransactionDate { get; set; }
        public string TransactionFromIPAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<int> Gender { get; set; }
        public string EmailID { get; set; }
        public string DOB { get; set; }
        public string MobileNumber { get; set; }
        public string MotherName { get; set; }
        public string Address { get; set; }
        public Nullable<int> RegionID { get; set; }
        public Nullable<int> StateID { get; set; }
        public Nullable<int> CityID { get; set; }
        public Nullable<int> Pincode { get; set; }
        public string GST { get; set; }
        public string Aadhaarno { get; set; }
        public string PANNo { get; set; }
        public string Entity { get; set; }
        public string AddressProofID { get; set; }
        public string AddressProofImage { get; set; }
        public string KycProofID { get; set; }
        public string KycProofImage { get; set; }
        public Nullable<int> DistributorID { get; set; }
        public string isOTPVerified { get; set; }
        public string CIN { get; internal set; }
    }
}
