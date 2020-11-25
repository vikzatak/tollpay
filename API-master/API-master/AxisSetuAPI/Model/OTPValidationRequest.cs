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

    public class clsNhaicchvalidateotprequest
    {
        public Nhaicchvalidateotprequest NHAICCHValidateOTPRequest { get; set; }
    }

    public class Nhaicchvalidateotprequest
    {
        public string NHAICCHValidateOTPRequestBodyEncrypted { get; set; }
        public Subheader SubHeader { get; set; }
    }


    public class clsNhaicchvalidateotpresponse
    {
        public Nhaicchvalidateotpresponse NHAICCHValidateOTPResponse { get; set; }
    }

    public class Nhaicchvalidateotpresponse
    {
        public Subheader SubHeader { get; set; }
        public string NHAICCHValidateOTPResponseBodyEncrypted { get; set; }
    }


    public class clsOTPValidationResponse
    {
        public bool isOTPValidated { get; set; }
    }




    public class OTPValidationRequest
    {
        public clsNhaicchvalidateotprequest _clsNhaicchvalidateotprequest { get; set; }
        clsOTPValidationResponse covr = new clsOTPValidationResponse();
        public clsOTPValidationResponse MethodOTPValidationRequest(Bank_OTPValidationAPI bov)
        {
            DP_TPEntities db = new DP_TPEntities();
            clsKeyValue ckv = EncryptionLibrary.GetKeys();
           // bov.otpReferenceId = ckv.SessionID.Substring(0,19);

            try
            {
                //db.Bank_OTPValidationAPI.Add(bov);
                //db.SaveChanges();

                _clsNhaicchvalidateotprequest = new clsNhaicchvalidateotprequest();
                _clsNhaicchvalidateotprequest.NHAICCHValidateOTPRequest = new Nhaicchvalidateotprequest();
                _clsNhaicchvalidateotprequest.NHAICCHValidateOTPRequest.SubHeader = new Subheader()
                {
                    channelId = "DIGI",
                    requestUUID = bov.otpReferenceId,
                    serviceRequestId = "AE.NHAI.OTP.ESB.001",
                    serviceRequestVersion = "1.0"
                };
                var jsonString = JsonConvert.SerializeObject(bov);

                _clsNhaicchvalidateotprequest.NHAICCHValidateOTPRequest.NHAICCHValidateOTPRequestBodyEncrypted = EncryptionLibrary.encrypt(jsonString, ckv.Key);
                var client = new RestClient(ckv.AxisSetuAPIURL + "/api/gateway/api/v1/fastag/otp-validation");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("X-IBM-Client-Id", ckv.XIBMClientId);
                request.AddHeader("X-IBM-Client-Secret", ckv.XIBMClientSecret);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", ckv.JWT);

                var requeststr = JsonConvert.SerializeObject(_clsNhaicchvalidateotprequest);
                //Console.WriteLine("Request to API " + requeststr.ToString());
                request.AddParameter("application/json", requeststr.ToString(), ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                //Console.WriteLine("AS Customer Dedupe Encrypted Reponse" + response.Content);

                APIException apierror = JsonConvert.DeserializeObject<APIException>(response.Content);
                clsNhaicchvalidateotpresponse json = JsonConvert.DeserializeObject<clsNhaicchvalidateotpresponse>(response.Content);

                string ser = string.Empty;
                if (apierror.httpCode != 200 && apierror.httpCode != 0) //503
                {
                    ser = JsonConvert.SerializeObject(apierror);
                    covr.isOTPValidated = false;
                }
                else if (response.Content != string.Empty)
                {
                    ser = EncryptionLibrary.decrypt(json.NHAICCHValidateOTPResponse.NHAICCHValidateOTPResponseBodyEncrypted, ckv.Key);
                    covr = JsonConvert.DeserializeObject<clsOTPValidationResponse>(ser);
                    //db.Bank_OTPValidationAPI.Where(o=>o.)
                    //db.Bank_OTPGenerationResponse.Add(covr);
                    //db.SaveChanges();
                }
                else
                {
                    ser = "Error";
                }



                db.AxisSetuAPILoggers.Add(new AxisSetuAPILogger()
                {
                    RequestGuid = ckv.SessionID,
                    request = jsonString.ToString(),
                    requestEncrypted = requeststr,
                    responseEncrypted = response.Content,
                    response = JsonConvert.SerializeObject(covr)

                });
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                covr.isOTPValidated = false;
                 return covr;
            }

            return covr;
        }

    }
}


