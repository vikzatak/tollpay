using AutoMapper;
using DPTPWebAPI;
using DPTPWebAPI.AxisSetuAPI.Model;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppAxisToken.model
{

    public class ClsEDeduperequest
    {
        public Deduperequest DedupeRequest { get; set; }
    }

    public class clsDeduperesponse
    {
        public Deduperesponse DedupeResponse { get; set; }
    }

    public class Deduperesponse
    {
        public Subheader SubHeader { get; set; }
        public string DedupeResponseBodyEncrypted { get; set; }
    }



    public class ClsDeduperequest
    {
        DP_TPEntities db = new DP_TPEntities();
        clsKeyValue ckv = EncryptionLibrary.GetKeys();
        public Deduperequest DedupeRequest { get; set; }

        public Bank_CustDedupeAPIResponse MethodClsDeduperequest(Bank_CustDedupeAPI cd)
        {
            string vguid = ckv.SessionID;
            cd.sessionId = ckv.SessionID;
            cd.refId = null;
            //cd.refId = ckv.SessionID; //RefID Should be blank
            Bank_CustDedupeAPIResponse bcdar = new Bank_CustDedupeAPIResponse();
            try
            {
                //var config = new MapperConfiguration(cfg => {
                //    cfg.CreateMap<Bank_CustDedupeAPI, CustomerDuplex>();
                //});
                //IMapper iMapper = config.CreateMapper();
                //var source = cd;
                //var destination = iMapper.Map<Bank_CustDedupeAPI, CustomerDuplex> (source);


                var jsonString = JsonConvert.SerializeObject(cd);
                string DedupeRequestBodyEncrypted = EncryptionLibrary.encrypt(jsonString, ckv.Key);

                db.Bank_CustDedupeAPI.Add(cd);
                db.SaveChanges();

                var client = new RestClient(ckv.AxisSetuAPIURL + "/api/gateway/api/v1/fastag/customer-dedupe");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("X-IBM-Client-Id", ckv.XIBMClientId);
                request.AddHeader("X-IBM-Client-Secret", ckv.XIBMClientSecret);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", ckv.JWT);
                ClsEDeduperequest cldr = new ClsEDeduperequest();

                Subheader sd = new Subheader()
                {
                    channelId = "DIGI",
                    requestUUID = vguid,
                    serviceRequestId = "Dedupe",
                    serviceRequestVersion = "1.0"
                };

                cldr.DedupeRequest = new Deduperequest();
                cldr.DedupeRequest.SubHeader = new Subheader();
                cldr.DedupeRequest.SubHeader = sd;
                cldr.DedupeRequest.DedupeRequestBodyEncrypted = DedupeRequestBodyEncrypted;
                var requeststr = JsonConvert.SerializeObject(cldr);
                //Console.WriteLine("Request to API " + requeststr.ToString());
                request.AddParameter("application/json", requeststr.ToString(), ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                //Console.WriteLine("AS Customer Dedupe Encrypted Reponse" + response.Content);

                APIException apierror = JsonConvert.DeserializeObject<APIException>(response.Content);
                clsDeduperesponse json = JsonConvert.DeserializeObject<clsDeduperesponse>(response.Content);

                if (apierror.httpCode != 200 && apierror.httpCode!=0) //503
                {
                    DedupeRequestBodyEncrypted = JsonConvert.SerializeObject(apierror);
                    bcdar.StatusCode = apierror.errorCode;
                    bcdar.StatusMessage = apierror.moreInformation + " | " + apierror.httpMessage;
                }
                else if (response.Content != string.Empty)
                {
                    DedupeRequestBodyEncrypted = EncryptionLibrary.decrypt(json.DedupeResponse.DedupeResponseBodyEncrypted, ckv.Key);
                    bcdar = JsonConvert.DeserializeObject<Bank_CustDedupeAPIResponse>(DedupeRequestBodyEncrypted);
                    db.Bank_CustDedupeAPIResponse.Add(bcdar);
                    db.SaveChanges();
                }
                else
                {//EncryptionLibrary.DecryptText(response.Content, akey);
                    DedupeRequestBodyEncrypted = "Error";
                }

                db.AxisSetuAPILoggers.Add(new AxisSetuAPILogger()
                {
                    RequestGuid= vguid,
                    subheaderMethod=cldr.DedupeRequest.SubHeader.serviceRequestId,
                    
                    Currenttime = DateTime.Now,
                    request = jsonString,
                    requestEncrypted = requeststr,
                    response = DedupeRequestBodyEncrypted,
                    responseEncrypted= response.Content
                });
                db.SaveChanges();
                
            }
            catch (Exception ex)
            {
                bcdar.StatusMessage = ex.Message + " - " + ex.InnerException;
                return bcdar;
                //db.AxisSetuAPILoggers.Where(r => r.RequestGuid == vguid).FirstOrDefault().response = ex.Message;
                //db.SaveChanges();

            }
           
            
            
     
            return bcdar;
        }

    }

    public class Deduperequest
    {
        public Subheader SubHeader { get; set; }
        public string DedupeRequestBodyEncrypted { get; set; }
    }


    public class CustomerDuplex
    {
        public string agentId { get; set; }
        public string mobilenumber { get; set; }
        public string posnumber { get; set; }
        public string sessionId { get; set; }
        public string field1 { get; set; }
        public string vehregnumber { get; set; }
        public string refId { get; set; }
        public string field3 { get; set; }
        public string field2 { get; set; }
        public string custname { get; set; }
        public string custdob { get; set; }
        public string field5 { get; set; }
        public string channelId { get; set; }
        public string field4 { get; set; }
        
    }


}
