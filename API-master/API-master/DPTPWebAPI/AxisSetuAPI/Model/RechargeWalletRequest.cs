using DPTPWebAPI;
using DPTPWebAPI.AxisSetuAPI.Model;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppAxisToken.model
{

    public class clsRechargerequest
    {
        public Rechargerequest RechargeRequest { get; set; }
    }

    public class Rechargerequest
    {
        public string RechargeRequestBodyEncrypted { get; set; }
        public Subheader SubHeader { get; set; }
    }


    public class clsRechargeresponse
    {
        public Rechargeresponse RechargeResponse { get; set; }
    }

    public class Rechargeresponse
    {
        public Subheader SubHeader { get; set; }
        public string RechargeResponseBodyEncrypted { get; set; }
    }



    


    public class ClsRechargeWalletRequest
    {
        public Rechargerequest RechargeRequest { get; set; }
        public Bank_RechargeResponse MethodRechargeWalletRequest(Bank_RechargeRequest brr)
        {
            DP_TPEntities db = new DP_TPEntities();
            clsKeyValue ckv = EncryptionLibrary.GetKeys();
            string vguid = Convert.ToString(Guid.NewGuid());
            clsRechargerequest crg = new clsRechargerequest();
            crg.RechargeRequest = new Rechargerequest();
            Bank_RechargeResponse brra = new Bank_RechargeResponse();
            try
            {
              
                crg.RechargeRequest.SubHeader = new Subheader()
                {
                    channelId = "DIGI",
                    requestUUID = vguid,
                    serviceRequestId = "RECHARGE",
                    serviceRequestVersion = "1.0"
                };

                var jsonString = JsonConvert.SerializeObject(brr);



                crg.RechargeRequest.RechargeRequestBodyEncrypted = EncryptionLibrary.encrypt(jsonString, ckv.Key);

                var client = new RestClient(ckv.AxisSetuAPIURL + "/api/gateway/api/v1/fastag/recharge");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("X-IBM-Client-Id", ckv.XIBMClientId);
                request.AddHeader("X-IBM-Client-Secret", ckv.XIBMClientSecret);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", ckv.JWT);

                var requeststr = JsonConvert.SerializeObject(crg);
                //Console.WriteLine("Request to API " + requeststr.ToString());
                request.AddParameter("application/json", requeststr.ToString(), ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                //Console.WriteLine("AS Customer Dedupe Encrypted Reponse" + response.Content);

                APIException apierror = JsonConvert.DeserializeObject<APIException>(response.Content);
                clsRechargeresponse json = JsonConvert.DeserializeObject<clsRechargeresponse>(response.Content);

                string ser = string.Empty;
                if (apierror.httpCode != 200 && apierror.httpCode != 0) //503
                {
                    ser = JsonConvert.SerializeObject(apierror);
                    brra.statusMessage = apierror.moreInformation + " | " + apierror.httpMessage;
                }
                else if (response.Content != string.Empty)
                {
                    ser = EncryptionLibrary.decrypt(json.RechargeResponse.RechargeResponseBodyEncrypted, ckv.Key);
                    brra = JsonConvert.DeserializeObject<Bank_RechargeResponse>(ser);
                    db.Bank_RechargeResponse.Add(brra);
                    db.SaveChanges();
                }
                else
                {
                    ser = "Error";
                }

                Console.WriteLine("API Plan Reponse" + ser);

                db.AxisSetuAPILoggers.Add(new AxisSetuAPILogger()
                {
                    RequestGuid = ckv.SessionID,
                    request = jsonString.ToString(),
                    subheaderMethod=crg.RechargeRequest.SubHeader.serviceRequestId,
                    requestEncrypted = requeststr,
                    responseEncrypted = response.Content,
                    response = JsonConvert.SerializeObject(ser)

                });
                db.SaveChanges();

            }
            catch (Exception ex)
            {
                brra.statusMessage = ex.Message;
                return brra;
            }
            return brra;
        }

    }
    }





  


