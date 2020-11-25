using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IndusAPIMiddleWareAPI.Helperclasses
{

    public class ClsTollPayTagRegistraion
    {
        public string MobileNo { get; set; } //
        public string OrderNo { get; set; }//
        public string PaymentType { get; set; }//
        public Vehicleinfo[] VehicleInfo { get; set; }//
        public string TagTotalAmount { get; set; }//
        public Shippingaddress ShippingAddress { get; set; }//
        public string AccountType { get; set; } //

        public string DistributorId { get; set; }
    }
    public class ClsInputTagRegistraion
    {
        public string MobileNo { get; set; }
        public string OrderNo { get; set; }
        public string PaymentType { get; set; }
        public Vehicleinfo[] VehicleInfo { get; set; }
        public string TagTotalAmount { get; set; }
        public Shippingaddress ShippingAddress { get; set; }
        public string AccountType { get; set; }
    }

    public class Shippingaddress
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string RegionID { get; set; }
        public string StateID { get; set; }
        public string CityID { get; set; }
        public string Pincode { get; set; }
    }

    public class Vehicleinfo
    {
        public string VehicleClassID { get; set; }
        public string NPCIClassID { get; set; }
        public string SerialNo { get; set; }
        public string VehicleNumber { get; set; }
        public string ChassisNumber { get; set; }
        public string CommercialType { get; set; }
        public string Securitydeposit { get; set; }
        public string CardCost { get; set; }
        public string MINIMUMTOPUP { get; set; }
        public string RCImage { get; set; }
    }


    public class ResposeTagRootobject
    {
        public string StatusCode { get; set; }
        public string Status { get; set; }
        public string MobileNo { get; set; }
        public string OrderNo { get; set; }
        public ResposeTagVehicleinfo[] VehicleInfo { get; set; }
    }

    public class ResposeTagVehicleinfo
    {
        public string StatusCode { get; set; }
        public string Status { get; set; }
        public string SerialNo { get; set; }
        public string VechileNo { get; set; }
    }


    //for requerytagreg

    public class ClsInputRequerytagreg
    {
        public string OrderNo { get; set; }
        public string SerialNo { get; set; }
    }

    //response for above class

    public class ResponseReqTagReg
    {
        public string StatusCode { get; set; }
        public string Status { get; set; }
        public Orderinfodetail[] OrderInfoDetails { get; set; }
    }

    public class Orderinfodetail
    {
        public string TransactionId { get; set; }
        public string CustomerId { get; set; }
        public string OrderId { get; set; }
        public string VehicleNumber { get; set; }
        public string SerialNo { get; set; }
        public string Status { get; set; }
    }

    //fortranhistory
    public class ClsInputTransactionHistory
    {
        public string FromDate { get; set; }
        public string MobileNumber { get; set; }
        public string ToDate { get; set; }
        public string TagID { get; set; }
    }

    //tranHistory

    public class ResponseTranHistory
    {
        public string StatusCode { get; set; }
        public string Status { get; set; }
        public Transactiondetail[] TransactionDetails { get; set; }
    }

    public class Transactiondetail
    {
        public string TransactionID { get; set; }
        public string Amount { get; set; }
        public string TransactionDate { get; set; }
        public string Paymentthrough { get; set; }
        public string TransactionType { get; set; }
        public string TransactionStatus { get; set; }
        public string VehicleNo { get; set; }
        public string TagID { get; set; }
    }


    class Demo
    {

    }


    public class ResponseWalletRecharge
    {
        public string StatusCode { get; set; }
        public string Status { get; set; }
        public string TransactionID { get; set; }
        public string RechargeAmount { get; set; }
        public string WalletBalance { get; set; }
        public string MobileNumber { get; set; }
        public string TagAccountNo { get; set; }
    }

}