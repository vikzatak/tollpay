
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ConsoleAppAxisToken.model
//{


//    public class TagIssuance
//    {
//        //public Statusenquiryrequest StatusEnquiryRequest { get; set; }
//        //public TagIssuance()
//        //{
//        //    string vguid = Convert.ToString(Guid.NewGuid());
//        //    StatusEnquiryRequest = new Statusenquiryrequest();
//        //    StatusEnquiryRequest.SubHeader = new Subheader()
//        //    {
//        //        //channelId = "DIGI",
//        //        //requestUUID = vguid,
//        //        //checksum = string

//            };
//        //    StatusEnquiryRequest.StatusEnquiryRequestBody = new Statusenquiryrequestbody()
//        //    {
//        //        //walletId = "11",
//        //        //subWalletId = "NA",
//        //        //operationId = "ISSUANCE",
//        //        //tagdispatch = "CORPORATE",
//        //        //tagSequenceId = "tagDispatch",
//        //        //postNumber = "0",
//        //        //productId = "0",
//        //        //paymentType = "0",
//        //        //vehicle = { },
//        //        //owner = { },
//        //        //document = { }
//        //};

//        }
//    }


//    //public class Rootobject
//    //{
//    //    public Header header { get; set; }
//    //    public TagIssuancePayload payload { get; set; }
//    }

  
//    public class TagIssuancePayload
//    {
//        public string walletId { get; set; }
//        public string subWalletId { get; set; }
//        public string operationId { get; set; }
//        public string tagdispatch { get; set; }
//        public string tagSequenceId { get; set; }
//        public string postNumber { get; set; }
//        public string productId { get; set; }
//        public string paymentType { get; set; }
//        public Vehicle vehicle { get; set; }
//        public Owner owner { get; set; }
//        public Document document { get; set; }
//    }

//    public class Vehicle
//    {
//        public string vehicleCategory { get; set; }
//        public string vehicleComm { get; set; }
//        public Registrationinfo registrationInfo { get; set; }
//    }

//    public class Registrationinfo
//    {
//        public string vehicleNumber { get; set; }
//        public string regisrationDate { get; set; }
//        public string authority { get; set; }
//        public string registrationState { get; set; }
//    }

//    public class Owner
//    {
//        public string mobileNumber { get; set; }
//        public string ownerName { get; set; }
//        public string emailId { get; set; }
//        public Custaddress custAddress { get; set; }
//    }

//    public class Custaddress
//    {
//        public string customerAddres { get; set; }
//        public string customerCity { get; set; }
//        public string customerState { get; set; }
//        public string pincode { get; set; }
//    }

//    public class Document
//    {
//        public string documentType { get; set; }
//        public string documentNumber { get; set; }
//    }


//}
