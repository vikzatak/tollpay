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

namespace DPTPWebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ecom_RFIDController : ApiController
    {
        private DP_TPEntities db = new DP_TPEntities();

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
            var dbal = db.DistributorAndTeamStatistics(dt.distributorid.ToString(),dt.startdate,dt.enddate);
                              
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
        //[JwtAuthentication]
        //[Route("api/DistributorSalesPersonList")]
        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        //[HttpPost]
        //public HttpResponseMessage DistributorSalesPersonList(HttpRequestMessage request, string distributorid)
        //{
        //    if (string.Empty == distributorid)
        //    {
        //        return new HttpResponseMessage(HttpStatusCode.BadRequest);
        //    }


        //    db.DistributorAndTeamAvailableandSoldsTags(distributorid);

        //    return request.CreateResponse(HttpStatusCode.OK, db.DistributorAndTeamAvailableandSoldsTags(distributorid));
        //}


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
    }
}