using AutoMapper;
using ConsoleAppAxisToken.model;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DPTPWebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BankFakeAPIController : ApiController
    {
        DP_TPEntities db = new DP_TPEntities();
        //// [JwtAuthentication]
        //// GET: api/BankFakeAPI
        // [JwtAuthentication]
        //[HttpGet]
        //public IQueryable<Bank_CustDedupeAPI> GetBank_CustDedupe()
        //{
        //    return db.Bank_CustDedupeAPI;
        //}
        //[ResponseType(typeof(Bank_CustDedupeAPIResponse))]
         [JwtAuthentication]
        [Route("api/CustomerDedupe")]
        [HttpPost]
        public HttpResponseMessage CustomerDedupe(HttpRequestMessage request, Bank_CustDedupeAPI bcd)
        {
            if (!ModelState.IsValid || bcd == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            ClsDeduperequest cdr = null;
            if (request.Headers.Contains("CallThisBankAPI"))
            {
                string bankAPIToCall = request.Headers.GetValues("CallThisBankAPI").First();
                if (bankAPIToCall == "Axis")
                {
                    cdr = new ClsDeduperequest();
                }
                if (bankAPIToCall == "Indus")
                {

                }
            }



            return request.CreateResponse(HttpStatusCode.OK, cdr.MethodClsDeduperequest(bcd));
        }
        //[JwtAuthentication]
        // POST: api/BankUpdateCFI
         [JwtAuthentication]
        [Route("api/BankUpdateCFI")]
        [HttpPost]
        public HttpResponseMessage BankUpdateCFI(HttpRequestMessage request, Bank_UpdateCIFAPI bucif)
        {
            if (!ModelState.IsValid || bucif == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            db.Bank_UpdateCIFAPI.Add(bucif);
            db.SaveChanges();
            return request.CreateResponse(HttpStatusCode.OK, bucif);
        }
        //[JwtAuthentication]
         [JwtAuthentication]
        // POST: api/Bank_Wallet
        [Route("api/Bank_Wallet")]
        [HttpPost]
        public HttpResponseMessage Bank_Wallet(HttpRequestMessage request, Bank_CreateWalletAPI bcwa)
        {
            if (!ModelState.IsValid || bcwa == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            clsWalletrequest wr = new clsWalletrequest();
            return request.CreateResponse(HttpStatusCode.OK, wr.MethodclsWalletrequest(bcwa));

        }
        //[JwtAuthentication]
         [JwtAuthentication]
        // POST: api/bankStatus
        [Route("api/bankStatus")]
        [HttpPost]
        public HttpResponseMessage bankStatus(HttpRequestMessage request, Bank_StatusEnquiryAPI bsea)
        {
            if (!ModelState.IsValid || bsea == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            ClsStatusenquiryrequest cser = new ClsStatusenquiryrequest();
            return request.CreateResponse(HttpStatusCode.OK, cser.MethodClsStatusenquiryrequest(bsea));
        }
         [JwtAuthentication]
        //[JwtAuthentication]
        // POST: api/bank_OTPValidation
        [HttpPost]
        [Route("api/bank_OTPGenerate")]
        public HttpResponseMessage bank_OTPGenerate(HttpRequestMessage request, Bank_OTPGenerate botpg)
        {
            if (!ModelState.IsValid || botpg == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            OTPGenerationRequest otpgr = new OTPGenerationRequest();

            return request.CreateResponse(HttpStatusCode.OK, otpgr.MethodOTPGenerationRequest(botpg));
        }
         [JwtAuthentication]
        [HttpPost]
        [Route("api/bank_rechargeWallet")]
        public HttpResponseMessage bank_rechargeWallet(HttpRequestMessage request, Bank_RechargeRequest rr)
        {
            if (!ModelState.IsValid || rr == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            ClsRechargeWalletRequest rwr = new ClsRechargeWalletRequest();
            return request.CreateResponse(HttpStatusCode.OK, rwr.MethodRechargeWalletRequest(rr));
        }
         [JwtAuthentication]
        [HttpPost]
        [Route("api/bank_walletBalance")]
        public HttpResponseMessage bank_rechargeStatusEnquiry(HttpRequestMessage request, Bank_RechargeStatusEnquiry wb)
        {
            if (!ModelState.IsValid || wb == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            clsWalletRequestStatus cwrs = new clsWalletRequestStatus();

            return request.CreateResponse(HttpStatusCode.OK, cwrs.MethodclsWalletRequestStatus(wb));
        }
         [JwtAuthentication]
        [HttpPost]
        [Route("api/bank_walletStatement")]
        public HttpResponseMessage bank_rechargeStatusEnquiry(HttpRequestMessage request, Bank_walletStatement ws)
        {
            if (!ModelState.IsValid || ws == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            db.Bank_walletStatement.Add(ws);
            db.SaveChanges();


            return request.CreateResponse(HttpStatusCode.OK, ws);
        }


        //[JwtAuthentication]
        // POST: api/bank_OTPValidation
         [JwtAuthentication]
        [Route("api/bank_OTPValidation")]
        [HttpPost]
        public HttpResponseMessage bank_OTPValidation(HttpRequestMessage request, Bank_OTPValidationAPI botp)
        {
            if (!ModelState.IsValid || botp == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            OTPValidationRequest ovr = new OTPValidationRequest();

            return request.CreateResponse(HttpStatusCode.OK, ovr.MethodOTPValidationRequest(botp));
        }



    }
}
