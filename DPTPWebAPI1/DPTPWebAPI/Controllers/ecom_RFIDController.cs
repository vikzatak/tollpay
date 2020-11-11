using System;
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
        [AllowAnonymous]
        [Route("api/FASTagsSeqList")]
        public HttpResponseMessage GetFASTagList(HttpRequestMessage request, string distributorid)
        {
            //r.ecom_CustomerId==0 mean available to sell
            if (string.Empty == distributorid)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            var ecom = db.ecom_RFID.Where(r => r.ecom_DistributionID == distributorid && r.ecom_CustomerId==0).Select(x => new { FASTagSeq = x.ecom_RFIDTagSrNo }).ToList();
            return request.CreateResponse(HttpStatusCode.OK, ecom);


        }

        // GET: api/ecom_RFID1
        [AllowAnonymous]
        [Route("api/FASTagsCustomerList")]
        public HttpResponseMessage GetFASTagListbyDistibutor(HttpRequestMessage request, string distributorid)
        {
            if (string.Empty==distributorid)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            var ecom = db.ecom_RFID.Where(r => r.ecom_DistributionID == distributorid).Select(x => new { CustMobNo = x.ecom_CustomerMobNo, CustVRN = x.ecom_CustomerVehicleNo,CustWallet=x.ecom_axis_WalletID}).ToList();
            return request.CreateResponse(HttpStatusCode.OK,  ecom);
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