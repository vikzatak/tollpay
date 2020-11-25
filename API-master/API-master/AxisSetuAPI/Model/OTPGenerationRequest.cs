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



    public class clsNhaicchgenerateotpresponse
    {
        public Nhaicchgenerateotpresponse NHAICCHGenerateOTPResponse { get; set; }
    }

    public class Nhaicchgenerateotpresponse
    {
        public Subheader SubHeader { get; set; }
        public string NHAICCHGenerateOTPResponseBodyEncrypted { get; set; }
    }









    public class ClsNhaicchgenerateotprequest
    {
        public Nhaicchgenerateotprequest NHAICCHGenerateOTPRequest { get; set; }
    }

    public class Nhaicchgenerateotprequest
    {
        public Subheader SubHeader { get; set; }
        public string NHAICCHGenerateOTPRequestBodyEncrypted { get; set; }

    }







    public class OTPGenerationRequest
    {
        DP_TPEntities db = new DP_TPEntities();
        clsKeyValue ckv = EncryptionLibrary.GetKeys();
        public NHAICCHValidateOTPRequestBody NHAICCHGenerateOTPRequest { get; set; }
        public Bank_OTPGenerationResponse MethodOTPGenerationRequest(Bank_OTPGenerate botpg)
        {
            Bank_OTPGenerationResponse bcdar = new Bank_OTPGenerationResponse();
            string vguid = ckv.SessionID;
            //botpg.otpReferenceId = vguid;

            db.Bank_OTPGenerate.Add(botpg);
            db.SaveChanges();


            NHAICCHGenerateOTPRequest = new NHAICCHValidateOTPRequestBody();
            NHAICCHGenerateOTPRequest.SubHeader = new Subheader()
            {
                channelId = "DIGI",
                requestUUID = vguid,
                serviceRequestId = "AE.NHAI.OTP.ESB.001",
                serviceRequestVersion = "1.0"
            };

            NHAICCHGenerateOTPRequest.NHAICCHGenerateOTPRequestBody = new Nhaicchgenerateotprequestbody()
            {
                mobileNumber = Convert.ToString(botpg.mobileNumber),
                otpReferenceId = vguid

            };
            var jsonString = JsonConvert.SerializeObject(NHAICCHGenerateOTPRequest.NHAICCHGenerateOTPRequestBody);

           

            ClsNhaicchgenerateotprequest cnho = new ClsNhaicchgenerateotprequest();
            cnho.NHAICCHGenerateOTPRequest  = new Nhaicchgenerateotprequest();
            cnho.NHAICCHGenerateOTPRequest.SubHeader = NHAICCHGenerateOTPRequest.SubHeader;
            cnho.NHAICCHGenerateOTPRequest.NHAICCHGenerateOTPRequestBodyEncrypted = EncryptionLibrary.encrypt(jsonString, ckv.Key);

            var client = new RestClient(ckv.AxisSetuAPIURL +"/api/gateway/api/v1/fastag/otp-generation");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("X-IBM-Client-Id", ckv.XIBMClientId);
            request.AddHeader("X-IBM-Client-Secret", ckv.XIBMClientSecret);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", ckv.JWT);

            var requeststr = JsonConvert.SerializeObject(cnho);
            //Console.WriteLine("Request to API " + requeststr.ToString());
            request.AddParameter("application/json", requeststr.ToString(), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            //Console.WriteLine("AS Customer Dedupe Encrypted Reponse" + response.Content);

            APIException apierror = JsonConvert.DeserializeObject<APIException>(response.Content);
            clsNhaicchgenerateotpresponse json = JsonConvert.DeserializeObject<clsNhaicchgenerateotpresponse>(response.Content);

            string ser = string.Empty;
            if (apierror.httpCode != 200 && apierror.httpCode!=0) //503
            {
                ser = JsonConvert.SerializeObject(apierror);
                bcdar.otpstatus = apierror.moreInformation + " | " + apierror.httpMessage;
            }
            else if (response.Content != string.Empty)
            {
                ser = EncryptionLibrary.decrypt(json.NHAICCHGenerateOTPResponse.NHAICCHGenerateOTPResponseBodyEncrypted, ckv.Key);
                bcdar = JsonConvert.DeserializeObject<Bank_OTPGenerationResponse>(ser);
                db.Bank_OTPGenerationResponse.Add(bcdar);
                db.SaveChanges();
            }
            else
            {
                ser = "Error";
            }

            Console.WriteLine("API Plan Reponse" + ser);

            db.AxisSetuAPILoggers.Add(new AxisSetuAPILogger()
            {   RequestGuid = ckv.SessionID,
                request = jsonString.ToString(),
                subheaderMethod=cnho.NHAICCHGenerateOTPRequest.SubHeader.serviceRequestId,
                requestEncrypted = requeststr,
                responseEncrypted = response.Content,
                response = JsonConvert.SerializeObject(json)

        });
            db.SaveChanges();

            return bcdar;
        }




    }

    public class NHAICCHValidateOTPRequestBody
    {
        public Subheader SubHeader { get; set; }
        public Nhaicchgenerateotprequestbody NHAICCHGenerateOTPRequestBody { get; set; }
    }



    public class Nhaicchgenerateotprequestbody
    {
        public string mobileNumber { get; set; }
        public string otpReferenceId { get; set; }
    }

}
