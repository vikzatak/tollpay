using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using DPTPWebAPI;
using IndusAPIMiddleWareAPI.Helperclasses;
using Newtonsoft.Json;
using RestSharp;

namespace DPTPWebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PaymentGatewayResponsesController : ApiController
    {
        private DP_TPEntities db = new DP_TPEntities();
        static string AESkey = "8468A97B4373415365537265744B6579746869834973215365637261745B6579";

         [JwtAuthentication]
        [Route("api/PostPaymentGateway")]
        public IHttpActionResult PostPaymentGatewayResponse([FromBody] ebusbuzzpaymentResponseIndusWalletRecharge ebpriwr)
        {
            clsPayGateWayWalletRecharge c = new clsPayGateWayWalletRecharge();
            c.paymentGatewayResponse = ebpriwr.paymentGatewayResponse;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            { 
                db.PaymentGatewayResponses.Add(ebpriwr.paymentGatewayResponse);
                db.SaveChanges();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }


            if (ebpriwr.paymentGatewayResponse.status == "success")
            {
                string strulr = "https://fastag.gitechnology.in/indusindAPI/api/BC/Tag/recharge_wallet";
                string methodtype = "POST";
                string thetoken = EncryptionsUtility.getheader(strulr, methodtype);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {


                    ClsRechargeWallet objRechargeWallet = ebpriwr.objRechargeWallet;
                    ClsRechargeWallet objWallet = new ClsRechargeWallet()
                    {
                        TransactionID = objRechargeWallet.TransactionID,
                        PaymentType = objRechargeWallet.PaymentType,
                        //CustomerID = objRechargeWallet.CustomerID,
                        TagAccountNo = objRechargeWallet.TagAccountNo,
                        MobileNumber = objRechargeWallet.MobileNumber,
                        Amount = objRechargeWallet.Amount,
                        RechargePercentage = objRechargeWallet.RechargePercentage,
                        TotalAmount = objRechargeWallet.TotalAmount
                    };
                    string serialInputWallet = JsonConvert.SerializeObject(objWallet);
                    string encryptedCityInput = EncryptionsUtility.AES_ENCRYPT(serialInputWallet, AESkey);
                    ClsRequestData objRequestData = new ClsRequestData() { RequestData = encryptedCityInput };
                    serialInputWallet = JsonConvert.SerializeObject(objRequestData);
                    Console.WriteLine(serialInputWallet);
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    var client = new RestClient(strulr);
                    client.Timeout = -1;
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("Authorization", thetoken);
                    request.AddHeader("Content-Type", "application/json");
                    request.AddParameter("application/json", serialInputWallet, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);

                    if (response == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        try
                        {
                            TokenResponse json = JsonConvert.DeserializeObject<TokenResponse>(response.Content);
                            var resData = json.ResponseData;// this extracts the encrypted requesttoken;
                            string decResult = EncryptionsUtility.AES_DECRYPT(resData, AESkey);
                            ResponseWalletRecharge objwalletrecharge = JsonConvert.DeserializeObject<ResponseWalletRecharge>(decResult);
                            c.objRespWalletResponse = objwalletrecharge;


                            return Ok(c);
                        }
                        catch (Exception ex)
                        {

                            return BadRequest(ex.Message);
                        }

                    }


                }

            }
            else
            {
                return Ok(c);
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PaymentGatewayResponseExists(int id)
        {
            return db.PaymentGatewayResponses.Count(e => e.srno == id) > 0;
        }
    }
}