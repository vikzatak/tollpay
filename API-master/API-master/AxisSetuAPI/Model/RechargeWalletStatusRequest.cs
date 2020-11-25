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

    public class ClsRStatusenquiryrequest
    {
        public RStatusenquiryrequest StatusEnquiryRequest { get; set; }
    }

    public class RStatusenquiryrequest
    {
        public Subheader SubHeader { get; set; }
        public string StatusEnquiryRequestBodyEncrypted { get; set; }
    }


    public class clsRStatusenquiryresponse
    {
        public RStatusenquiryresponse StatusEnquiryResponse { get; set; }
    }

    public class RStatusenquiryresponse
    {
        public Subheader SubHeader { get; set; }
        public string StatusEnquiryResponseBodyEncrypted { get; set; }
    }

   


    public class clsWalletRequestStatus
    {
        Bank_RechargeStatusEnquiryResponse brser = new Bank_RechargeStatusEnquiryResponse();
        public Bank_RechargeStatusEnquiryResponse MethodclsWalletRequestStatus(Bank_RechargeStatusEnquiry brse)
        {
            DP_TPEntities db = new DP_TPEntities();
            clsKeyValue ckv = EncryptionLibrary.GetKeys();
            brse.uniquerefNo = ckv.SessionID.Substring(0, 19);

            db.Bank_RechargeStatusEnquiry.Add(brse);
            db.SaveChanges();

            ClsRStatusenquiryrequest Rser = new ClsRStatusenquiryrequest();
            Rser.StatusEnquiryRequest = new RStatusenquiryrequest();
            Rser.StatusEnquiryRequest.SubHeader = new Subheader()
            {
                channelId = "DIGI",
                requestUUID = ckv.SessionID,
                serviceRequestId = "RECHARGE-STATUS-ENQUIR",
                serviceRequestVersion = "1.0"
            };

            var jsonString = JsonConvert.SerializeObject(brse);

            Rser.StatusEnquiryRequest.StatusEnquiryRequestBodyEncrypted = EncryptionLibrary.encrypt(jsonString, ckv.Key);
            var client = new RestClient(ckv.AxisSetuAPIURL + "/api/gateway/api/v1/fastag/enquiry");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("X-IBM-Client-Id", ckv.XIBMClientId);
            request.AddHeader("X-IBM-Client-Secret", ckv.XIBMClientSecret);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", ckv.JWT);

            var requeststr = JsonConvert.SerializeObject(Rser);
            request.AddParameter("application/json", requeststr.ToString(), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            //Console.WriteLine("AS Customer Dedupe Encrypted Reponse" + response.Content);

            APIException apierror = JsonConvert.DeserializeObject<APIException>(response.Content);
            clsRStatusenquiryresponse json = JsonConvert.DeserializeObject<clsRStatusenquiryresponse>(response.Content);

            string ser = string.Empty;
            if (apierror.httpCode != 200 && apierror.httpCode != 0) //503
            {
                ser = JsonConvert.SerializeObject(apierror);
                brser.statusMessage = apierror.moreInformation + " | " + apierror.httpMessage; ;
                brser.statusCode = apierror.httpCode.ToString();

            }
            else if (response.Content != string.Empty)
            {
                ser = EncryptionLibrary.decrypt(json.StatusEnquiryResponse.StatusEnquiryResponseBodyEncrypted, ckv.Key);
                brser = JsonConvert.DeserializeObject<Bank_RechargeStatusEnquiryResponse>(ser);

                db.Bank_RechargeStatusEnquiryResponse.Add(brser);
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
                subheaderMethod= Rser.StatusEnquiryRequest.SubHeader.serviceRequestId,
                requestEncrypted = requeststr,
                responseEncrypted = response.Content,
                response = JsonConvert.SerializeObject(brser)

            });
            db.SaveChanges();

            return brser;

        }
    }
}
