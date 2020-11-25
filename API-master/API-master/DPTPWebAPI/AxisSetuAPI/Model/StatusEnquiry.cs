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

    public class clsStatusenquiryrequest
    {
        public Statusenquiryrequest StatusEnquiryRequest { get; set; }
    }

    public class Statusenquiryrequest
    {
        public Subheader SubHeader { get; set; }
        public string StatusEnquiryRequestBodyEncrypted { get; set; }
    }




    public class clsStatusenquiryresponse
    {
        public Statusenquiryresponse StatusEnquiryResponse { get; set; }
    }

    public class Statusenquiryresponse
    {
        public Subheader SubHeader { get; set; }
        public string StatusEnquiryResponseBodyEncrypted { get; set; }
    }



  



    public class ClsStatusenquiryrequest
    {
        DP_TPEntities db = new DP_TPEntities();
        clsKeyValue ckv = EncryptionLibrary.GetKeys();

        public Bank_StatusEnquiryAPIResponse MethodClsStatusenquiryrequest(Bank_StatusEnquiryAPI bsea)
        {
            string vguid = ckv.SessionID;
            bsea.refId = bsea.sessionId; // ckv.SessionID;
            Bank_StatusEnquiryAPIResponse bcdar = new Bank_StatusEnquiryAPIResponse();
            clsStatusenquiryrequest cser = new clsStatusenquiryrequest();
            cser.StatusEnquiryRequest = new Statusenquiryrequest();

            try
            {
                db.Bank_StatusEnquiryAPI.Add(bsea);
                db.SaveChanges();

                Subheader sub = new Subheader()
                {
                    serviceRequestVersion = "1.0",
                    serviceRequestId = "STATUS-ENQUIRY",
                    requestUUID = bsea.refId,
                    channelId = "DIGI"
                };
                cser.StatusEnquiryRequest.SubHeader = new Subheader();
                cser.StatusEnquiryRequest.SubHeader = sub;


                var jsonString = JsonConvert.SerializeObject(bsea);

                cser.StatusEnquiryRequest.StatusEnquiryRequestBodyEncrypted = EncryptionLibrary.encrypt(jsonString, ckv.Key);

                var client = new RestClient(ckv.AxisSetuAPIURL + "/api/gateway/api/v1/fastag/status-enquiry");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("X-IBM-Client-Id", ckv.XIBMClientId);
                request.AddHeader("X-IBM-Client-Secret", ckv.XIBMClientSecret);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", ckv.JWT);

                var requeststr = JsonConvert.SerializeObject(cser);
                //Console.WriteLine("Request to API " + requeststr.ToString());
                request.AddParameter("application/json", requeststr.ToString(), ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                //Console.WriteLine("AS Customer Dedupe Encrypted Reponse" + response.Content);

                APIException apierror = JsonConvert.DeserializeObject<APIException>(response.Content);
                clsStatusenquiryresponse json = JsonConvert.DeserializeObject<clsStatusenquiryresponse>(response.Content);
                string ser = string.Empty;
                if (apierror.httpCode != 200 && apierror.httpCode != 0) //503
                {
                    ser = JsonConvert.SerializeObject(apierror);
                    bcdar.statuscode = apierror.errorCode;
                    bcdar.statusmessage = apierror.moreInformation + " | " + apierror.httpMessage;
                }
                else if (response.Content != string.Empty)
                {
                    ser = EncryptionLibrary.decrypt(json.StatusEnquiryResponse.StatusEnquiryResponseBodyEncrypted, ckv.Key);
                    bcdar = JsonConvert.DeserializeObject<Bank_StatusEnquiryAPIResponse>(ser);
                    //update tollpay ecom and customer dudupe
                    if(bcdar.txnSeqNo.Length>0 && bcdar.walletId.Length>0)
                    {
                         //db.ecom_RFID.Where(x=>x.ecom_RFIDTagSrNo==bcdar.t)

                    }

                    db.Bank_StatusEnquiryAPIResponse.Add(bcdar);
                    db.SaveChanges();
                }
                else
                {//EncryptionLibrary.DecryptText(response.Content, akey);
                    ser = "Error";
                }

                db.AxisSetuAPILoggers.Add(new AxisSetuAPILogger()
                {
                    RequestGuid = bsea.refId,
                    subheaderMethod=cser.StatusEnquiryRequest.SubHeader.serviceRequestId,
                    Currenttime = DateTime.Now,
                    request = jsonString,
                    requestEncrypted = requeststr,
                    responseEncrypted = response.Content,
                    response = ser
                });
                db.SaveChanges();
                return bcdar;
            }
            catch (Exception ex)
            {

                bcdar.statusmessage = ex.Message;
                return bcdar;


            }
            

            
        }
    }






}
