using Amazon.Runtime.Internal.Util;
using DPTPWebAPI.Models;
using IndusAPIMiddleWareAPI.Helperclasses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApplicationTP.DAL;

namespace DPTPWebAPI.Controllers
{

    public class clsVehicleTypePlusTag
    {
     
        public List<GetDistributorTags_Result> lstTags { get; set; }
        public string VehicleClassID { get; set; }
    }
    public class clsDIDorVID
    {
        public string DistID { get; set; }
        public string VehicleRegNo { get; set; }
    }
    public class clsTagClosure
    {
        public int custMobNo { get; set; }
        public string tagSerialNo { get; set; }       


    }

    public class clsTagReplacement
    {
        public string custMobNo { get; set; }
        public string vehicleNo { get; set; }
        public string oldTagSrNo { get; set; }
        public string reason { get; set; }
        public string vehicleClass { get; set; }
        public int OTP { get; set; }
        public int SalesPersonID { get; set; }
        public int distID { get; set; }
        public string newTagSrNo { get; set; }
        public string tagAcctNo { get; set; }

    }
    public class clsRequestDistributorTags
    {
        public Class1[] Property1 { get; set; }
    }

    public class Class1
    {
        public int DistId { get; set; }
        public string TagTypeID { get; set; }
        public int Qty { get; set; }
    }

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ecom_RFIDController : ApiController
    {
        private DP_TPEntities db = new DP_TPEntities();
        Notifications ns = new Notifications();

        // GET: api/ecom_RFID1
        [JwtAuthentication]
        [Route("api/GetDistributorTags")]
        [HttpPost]
        public HttpResponseMessage GetFASTagList(HttpRequestMessage request, DistributorTags dt)
        {
            //r.ecom_CustomerId==0 mean available to sell
            if (dt == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            IndusInd_VehicleClassDetails ivt = db.IndusInd_VehicleClassDetails.Where(v => v.VehicleType == (dt.VehicleType)).FirstOrDefault();
            if (ivt != null)
            {
                double vSecuritydeposit = Convert.ToDouble(ivt.SecurityDeposit);
                double vCardCost = Convert.ToDouble(ivt.CardCost);
                double vMINIMUMTOPUP = Convert.ToDouble(ivt.Amount);
                double vTagTotalAmount = vSecuritydeposit + vCardCost + vMINIMUMTOPUP;

                //var ecom = db.ecom_RFID.Where(r => r.ecom_DistributionID == dt.DistributorID && r.ecom_CustomerVehicleNo == null && dt.VehicleType==(dt.VehicleType)).Select(x => new
                //{   FASTagSeq = x.ecom_RFIDTagSrNo,
                //    FASTTagSrNo = x.Serial_Number,
                //    Securitydeposit = vSecuritydeposit,
                //    CardCost = vCardCost,
                //    MINIMUMTOPUP = vMINIMUMTOPUP,
                //    TagTotalAmount = vTagTotalAmount
                //}).ToList();

                var ecom = db.GetDistributorTags(dt.DistributorID, dt.VehicleType).Select(x => new
                {
                    FASTagSeq = x.ecom_RFIDTagSrNo,
                    FASTTagSrNo = x.Serial_Number,
                    Securitydeposit = vSecuritydeposit,
                    CardCost = vCardCost,
                    MINIMUMTOPUP = vMINIMUMTOPUP,
                    TagTotalAmount = vTagTotalAmount
                }).ToList();

                return request.CreateResponse(HttpStatusCode.OK, ecom);
            }
            else
                return request.CreateResponse(HttpStatusCode.NotFound);

        }

        [JwtAuthentication]
        [Route("api/GetDistributorStatByDistributorId")]
        [HttpPost]
        public HttpResponseMessage GetDistributorStast(HttpRequestMessage request, disttransactions dt)

        {
            var jsonObject = new JObject();
            var dbal = db.DistributorAndTeamStatistics(dt.distributorid.ToString(), dt.startdate, dt.enddate);

            return request.CreateResponse(HttpStatusCode.OK, dbal);


        }

        // GET: api/ecom_RFID1
        [JwtAuthentication]
        [Route("api/FASTagsCustomerList")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpPost]
        public HttpResponseMessage GetFASTagListbyDistibutor(HttpRequestMessage request, string distributorid)
        {
            int did = Convert.ToInt32(distributorid);
            if (string.Empty == distributorid)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            // IndusInd_CustomerRegistration ecust = db.IndusInd_CustomerRegistration.Where(r => r.DistributorID == did).Select(x => new { TransactionID = x.TransactionID, MobileNo = x.MobileNumber, Name = x.FirstName + " " + x.LastName, OTPStatus = x.isOTPVerified }).ToList();

            var ecom = db.ecom_RFID.Where(r => r.ecom_DistributionID == did).Select(x => new { CustMobNo = x.ecom_CustomerMobNo, CustVRN = x.ecom_CustomerVehicleNo, CustFASTag = x.ecom_RFIDTagSrNo, CustTagSrNo = x.Serial_Number, CustTagStatus = x.ecom_RFIDStatus, CustWallet = x.ecom_axis_WalletID }).ToList();
            return request.CreateResponse(HttpStatusCode.OK, ecom);
        }

        // GET: api/ecom_RFID1
        [JwtAuthentication]
        [Route("api/DistributorSalesPersonList")]
        
        [HttpPost]
        public HttpResponseMessage DistributorSalesPersonList(HttpRequestMessage request, string distributorid)
        {
            if (string.Empty == distributorid)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            return request.CreateResponse(HttpStatusCode.OK, db.DistributorAndTeamUnSoldsTags(distributorid));

        }//method ends

        [JwtAuthentication]
        [Route("TPapi/DistributorUnsoldTagsCountVehicleClassWise")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpPost]
        public HttpResponseMessage DistributorUnsoldTagsCountVehicleClassWise(HttpRequestMessage request, dist obj)
        {

            try
            {

                if (String.IsNullOrEmpty(obj.distributorid))
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }
                else
                {
                    return request.CreateResponse(HttpStatusCode.OK, db.DistributorUnSoldStock(obj.distributorid));
                }

            }
            catch (Exception ex)
            {
                return request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }//method ends


        [JwtAuthentication]
        [Route("api/RequestDistributorTags")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpPost]
        public HttpResponseMessage RequestDistributorTags(HttpRequestMessage request, clsRequestDistributorTags crdt)
        {
            if (crdt==null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            foreach (var item in crdt.Property1)
            {
                Distributor_Tag_Request dtr = new Distributor_Tag_Request();
                dtr.DistId = item.DistId;
                dtr.Qty = item.Qty;
                dtr.TagTypeID = item.TagTypeID;
                dtr.RequestDate = DateTime.Now;
                dtr.ApprovalStatus = "Pending";
                db.Distributor_Tag_Request.Add(dtr);
                db.SaveChanges();
            }




            return request.CreateResponse(HttpStatusCode.OK, crdt);
        }

        // GET: api/ecom_RFID1
        [JwtAuthentication]
        [Route("api/DistributorToSalesPersonFastTags")]
        [HttpPost]
        public HttpResponseMessage DistributorToSalesPersonFastTags(HttpRequestMessage request, DistributorToSalesPersonTags[] dtspt)
        {
            clslststatuscode css = new clslststatuscode();
            clsstatuscode scd = css.getstatuscodes().Where(s => s.statuscode == 001).FirstOrDefault();
            if (dtspt == null)
            {
                return request.CreateResponse(HttpStatusCode.BadRequest, scd);

            }

            try
            {
                foreach (var item in dtspt)
                {
                    if (!string.IsNullOrEmpty(item.TagNo))
                    {
                        int DId = Convert.ToInt32(item.DistributorUserID);
                        int SId = Convert.ToInt32(item.SalesUserID);
                        db.ecom_RFID.Where(ec => ec.Serial_Number == item.TagNo && ec.ecom_DistributionID == DId).FirstOrDefault().ecom_DistributionID = SId;
                        db.ecom_RFID.Where(ec => ec.Serial_Number == item.TagNo && ec.ecom_DistributionID == DId).FirstOrDefault().ecom_CreatedDate = DateTime.Now;
                        db.SaveChanges();
                        //update inward table
                        IndusInd_TagStockDetails iits = db.IndusInd_TagStockDetails.Where(x => x.Serial_Number == item.TagNo).FirstOrDefault();
                        iits.SalesPerson_Id = SId;
                        iits.SalesPerson_Assign_Date = DateTime.Now;
                        db.SaveChanges();

                    }
                }

            }
            catch (Exception ex)
            {
                return request.CreateResponse(HttpStatusCode.InternalServerError, scd);
            }


            scd = css.getstatuscodes().Where(s => s.statuscode == 000).FirstOrDefault();

            return request.CreateResponse(HttpStatusCode.OK, scd);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ecom_RFIDExists(int id)
        {
            return db.ecom_RFID.Count(e => e.ecom_srno == id) > 0;
        }

        [JwtAuthentication]
        [Route("TPapi/TagReplacementRequest")]
        [HttpPost]
        public HttpResponseMessage TagReplacementRequest(HttpRequestMessage request, clsTagReplacement ctr)
        {
           
            if (ctr == null)
            {
                return request.CreateResponse(HttpStatusCode.BadRequest, ctr);

            }

            try
            {
                TagReplacementReq trr = new TagReplacementReq();
                trr.ReqDate = DateTime.Now;
                trr.OldSrNo = ctr.oldTagSrNo;
                //trr.CustID = ctr.custMobNo;
                
                trr.VehicleNo = ctr.vehicleNo;
                trr.VehicleClass = ctr.vehicleClass;
                Random rnd = new Random();
                int value = rnd.Next(100000, 999999);
                trr.OTP = value;        
                trr.Reason = ctr.reason;
                trr.SalesPersonID = ctr.SalesPersonID;
                trr.DistID = ctr.distID;
                db.TagReplacementReqs.Add(trr);
                db.SaveChanges();
                trr.SrNo = trr.SrNo;
                return request.CreateResponse(HttpStatusCode.OK, trr);

            }
            catch (Exception ex)
            {
               return request.CreateResponse(HttpStatusCode.InternalServerError, ctr);
            }

            return request.CreateResponse(HttpStatusCode.OK, ctr);

        }


        [JwtAuthentication]
        [Route("TPapi/SalesPersonTagReplacementRequest")]
        [HttpPost]
        public HttpResponseMessage SalesPersonTagReplacementRequest(HttpRequestMessage request, clsTagReplacement ctr)
        {

            if (ctr == null)
            {
                return request.CreateResponse(HttpStatusCode.BadRequest, ctr);

            }

            try
            {
                TagReplacementReq trr = new TagReplacementReq();
                
                trr.ReqDate = DateTime.Now;
                trr.OldSrNo = ctr.oldTagSrNo;
                trr.CustMobNo = ctr.custMobNo;
                trr.VehicleNo = ctr.vehicleNo;
                trr.VehicleClass = ctr.vehicleClass;
                Random rnd = new Random();
                int value = rnd.Next(100000, 999999);
                trr.OTP = value;
                trr.Reason = ctr.reason;
                trr.SalesPersonID = ctr.SalesPersonID;
                trr.DistID = ctr.distID;
                db.TagReplacementReqs.Add(trr);
                db.SaveChanges();

                ecom_RFID ecr = db.ecom_RFID.Where(x => x.Serial_Number == ctr.oldTagSrNo).FirstOrDefault();
                ecr.ecom_RFIDStatus = "r";
                db.SaveChanges();
                ecom_RFID ecrn = db.ecom_RFID.Where(x => x.Serial_Number == ctr.newTagSrNo).FirstOrDefault();
                ecrn.ecom_CustomerMobNo = ctr.custMobNo;
                ecrn.ecom_CustomerVehicleNo = ctr.vehicleNo;
                ecrn.ecom_RFIDStatus = "a";
                db.SaveChanges();
                decimal amt = -100;
                DistributorCreditAccount dcr = db.DistributorCreditAccounts.FirstOrDefault();
                dcr.Amount = Convert.ToDecimal("-100");
                dcr.DepositDate = DateTime.Now;
                string msg = dcr.DepositedVia;
                string tagSrNoNew = ctr.newTagSrNo;
                int len =tagSrNoNew.Length;
                dcr.DepositedVia = "For tag replacement with serial no "+ msg.Substring(len-4) ;
                db.DistributorCreditAccounts.Add(dcr);
                
                db.SaveChanges();
                //update ecomRFID table row for old serial no. with status R
                //update ecomRFID table row for new serial no. with status A with customer's mob and vehicle no.
                //insert into DistributorCreditAccount a debit of -100 for tag replacement with srno.(last four character)
                trr.SrNo = trr.SrNo;
                return request.CreateResponse(HttpStatusCode.OK, trr);
                

            }
            catch (Exception ex)
            {
                return request.CreateResponse(HttpStatusCode.InternalServerError, ctr);
            }

            return request.CreateResponse(HttpStatusCode.OK, ctr);

        }


        [JwtAuthentication]
        [Route("TPapi/TagClosureRequest")]
        [HttpPost]
        public HttpResponseMessage TagClosureRequest(HttpRequestMessage request, clsTagClosure ctr)
        {

            if (ctr == null)
            {
                return request.CreateResponse(HttpStatusCode.BadRequest, ctr);

            }

            try
            {
                TagReplacementReq trr = new TagReplacementReq();
                trr.ReqDate = DateTime.Now;
                //trr.OldSrNo = ctr.oldTagSrNo;

                //return request.CreateResponse(HttpStatusCode.OK, db.TagReplacementReq);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return request.CreateResponse(HttpStatusCode.InternalServerError, ctr);
            }

            return request.CreateResponse(HttpStatusCode.OK, ctr);
        }

        [JwtAuthentication]
        [Route("TPapi/GetVehicleClassAndTagsDetailsByDistIDorVehicleID")]
        [HttpPost]
        public HttpResponseMessage GetVehicleClassAndTagsDetailsByDistIDorVehicleID(HttpRequestMessage request, clsDIDorVID cdv)
        {

            if (cdv == null)
            {
                return request.CreateResponse(HttpStatusCode.BadRequest, cdv);

            }

            try
            {
                
                int DID = Convert.ToInt32(cdv.DistID);
                 if(String.IsNullOrEmpty(cdv.VehicleRegNo))
                {
                    return request.CreateResponse(HttpStatusCode.OK, db.VTP_GetVehicleClassDetailsByDistID(DID));
                }
                else
                {
                    ecom_RFID efrid = db.ecom_RFID.Where(x => x.ecom_CustomerVehicleNo == cdv.VehicleRegNo).FirstOrDefault();

                    string VehicleType = efrid.Vehicle_Type;
                    string custMobNo = efrid.ecom_CustomerMobNo;
                    Random rnd = new Random();
                    int value = rnd.Next(100000, 999999);
                    
                    string strmsg = "Tollpay OTP for tag replacement of VRN "+cdv.VehicleRegNo+" is: "+ value.ToString();
                    TagReplacementReq tRR = new TagReplacementReq();
                    tRR.SalesPersonID = Convert.ToInt32(cdv.DistID);
                    tRR.VehicleNo = cdv.VehicleRegNo;
                    tRR.CustMobNo = custMobNo;
                    tRR.OTP = value;
                    tRR.ReqDate = DateTime.Now;
                    db.TagReplacementReqs.Add(tRR);
                    db.SaveChanges();
                    ns.sendsms("Tollpay",custMobNo,strmsg); 
                    clsVehicleTypePlusTag VTT = new clsVehicleTypePlusTag();
                    VTT.lstTags = db.GetDistributorTags(cdv.DistID, VehicleType).ToList();
                    VTT.VehicleClassID = db.IndusInd_VehicleClassDetails.Where(v => v.VehicleType == VehicleType).FirstOrDefault().VehicleClassId.ToString();
                    
                    return request.CreateResponse(HttpStatusCode.OK, VTT);
                }

            }
            catch (Exception ex)
            {
                return request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            
        }
    }
}