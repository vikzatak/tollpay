using ConsoleAppAxisToken;
using DPTPWebAPI.paymentGateway;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
//using Com.CloudRail.SI.Services.PayPal;

namespace DPTPWebAPI.Controllers
{
    public class paymentdetails
    {
        public string amount { get; set; }
        public string firstname { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string productinfo { get; set; }
     
    }
     [EnableCors(origins: "*", headers: "*", methods: "*")]
     public class PaymentController : ApiController
    {
             //form the mandatory fields 
         [JwtAuthentication]
        [Route("api/easebuzzPayment")]
        [HttpPost]
        public HttpResponseMessage easebuzzPayment(HttpRequestMessage request, paymentdetails pd)
        {
            if (!ModelState.IsValid || pd == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            clsKeyValue ckv = EncryptionLibrary.GetKeys();
            string amount =pd.amount ;
        string firstname = pd.firstname;
        string email = pd.email;
        string phone = pd.phone;
        string productinfo = pd.productinfo;
           
        string surl = "https://payment.tollpay.in/success.aspx";  //Request.Form["surl"].Trim();

        string furl = "https://payment.tollpay.in/success.aspx"; //Request.Form["furl"].Trim();

        string Txnid =ckv.SessionID ;
            //call the object of class and start payment
            Easebuzz t = new Easebuzz(ckv.easebuzzsalt, ckv.easebuzzkey, ckv.easebuzzenv);

        string strForm = t.initiatePaymentAPI(amount, firstname, email, phone, productinfo, surl, furl, Txnid);

            return request.CreateResponse(HttpStatusCode.OK, strForm);

        }
    }
}
