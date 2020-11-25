using DPTPWebAPI;
using DPTPWebAPI.AxisSetuAPI.Model;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppAxisToken
{
   
     public class clsKeyValue
    {
        public string  AxisSetuAPIURL { get; set; }
        public string Key { get; set; }
        public string XIBMClientId { get; set; }
        public string XIBMClientSecret { get; set; }
        public string JWT { get; set; }
        public string SessionID { get; set; }
        public string easebuzzsalt { get; set; }
        public string easebuzzkey { get; set; }
        public string easebuzzenv { get; set; }
    }
    public class clsdecryptedResult
    {
        public string decryptedResult { get; set; }
    }

    public class clsencryptedResult
    {
        public string encryptedResult { get; set; }
    }

    public class RequestDecryptObj
    {
        public string stringToDecrypt { get; set; }
        public string encryptionKey { get; set; }
    }
    public class RequestEncryptObj
    {
        public string stringToEncrypt { get; set; }
        public string encryptionKey { get; set; }
    }

    public class EncryptionLibrary
    {
      
        public static clsKeyValue GetKeys()
        {
            DP_TPEntities db = new DP_TPEntities();
            JwtAuthManager jwt = new JwtAuthManager();
            string Key = db.AppKeys.Where(x => x.vKey == "key").FirstOrDefault().vvalue;
            string XIBMClientId = db.AppKeys.Where(x => x.vKey == "XIBMClientId").FirstOrDefault().vvalue;
            string easebuzzsalt = db.AppKeys.Where(x => x.vKey == "easebuzzsalt").FirstOrDefault().vvalue;
            string easebuzzkey = db.AppKeys.Where(x => x.vKey == "easebuzzkey").FirstOrDefault().vvalue;
            string easebuzzenv = db.AppKeys.Where(x => x.vKey == "easebuzzenv").FirstOrDefault().vvalue;
            string XIBMClientSecret = db.AppKeys.Where(x => x.vKey == "XIBMClientSecret").FirstOrDefault().vvalue;
            string SessionKey= DateTime.Now.ToString("yyyyMMddHHmmssfffffff");
            string AxisSetuAPIURL= db.AppKeys.Where(x => x.vKey == "AxisSetuAPIURL").FirstOrDefault().vvalue;
            string JWT ="Bearer " + jwt.GenerateJWTToken();
            
            return new clsKeyValue() { Key = Key, XIBMClientId = XIBMClientId, 
                XIBMClientSecret = XIBMClientSecret, SessionID =SessionKey, JWT= JWT,
                easebuzzenv=easebuzzenv,
                easebuzzkey=easebuzzkey,
                easebuzzsalt=easebuzzsalt,
                AxisSetuAPIURL= AxisSetuAPIURL
            };

        }
        public static string encrypt(string input, string key)
        {
            clsencryptedResult json = null;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var clientcr = new RestClient("https://axis-encryption.setu.co/encrypt");
                clientcr.Timeout = -1;
                var requestcr = new RestRequest(Method.POST);
                requestcr.AddHeader("Content-Type", "application/json");
                RequestEncryptObj rco = new RequestEncryptObj();
                rco.encryptionKey = key;
                rco.stringToEncrypt = input;
                var jsonString = JsonConvert.SerializeObject(rco);
                requestcr.AddParameter("application/json", jsonString, ParameterType.RequestBody);
                IRestResponse response = clientcr.Execute(requestcr);
               
               json = JsonConvert.DeserializeObject<clsencryptedResult>(response.Content);
            }
            catch (Exception ex)
            {

                EncryptionLibrary.encrypt(input, key);
            }
            
            return json.encryptedResult;
        }
        public static string decrypt(string input, string key)
        {
            clsdecryptedResult json = null; 
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var clientdcr = new RestClient("https://axis-encryption.setu.co/decrypt");
                clientdcr.Timeout = -1;
                var requestdcr = new RestRequest(Method.POST);
                RequestDecryptObj rdo = new RequestDecryptObj();
                rdo.encryptionKey = key;
                rdo.stringToDecrypt = input;
                var jsonString = JsonConvert.SerializeObject(rdo);
                requestdcr.AddHeader("Content-Type", "application/json");
                requestdcr.AddParameter("application/json", jsonString, ParameterType.RequestBody);
                IRestResponse response = clientdcr.Execute(requestdcr);
               
                 json = JsonConvert.DeserializeObject<clsdecryptedResult>(response.Content);
            }
            catch (Exception ex)
            {
                EncryptionLibrary.decrypt(input, key);
                
            }
            return json.decryptedResult;
        }
    }
}

