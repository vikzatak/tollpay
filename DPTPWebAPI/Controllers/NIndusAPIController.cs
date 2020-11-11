using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using IndusAPIMiddleWareAPI.Helperclasses;
using Google.GData.Client;
using DPTPWebAPI.IndusbankAPI;
using AutoMapper;
using System.Web;
using System.Data.Entity.Migrations.Model;
using WebApplicationTP.DAL;
using System.Web.Http.Results;

namespace DPTPWebAPI.Controllers
{
    public class disttransactions
    {
        public int distributorid { get; set; }

        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }

    }
    public class dist
    {
        public static object ownerID { get; internal set; }
        public string distributorid { get; set; }
    }

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class NIndusAPIController : ApiController
    {
        DP_TPEntities db = new DP_TPEntities();
        Notifications ns = new Notifications();
        static string AESkey = "8468A97B4373415365537265744B6579746869834973215365637261745B6579";
        //https://localhost:44391/api/GetRegions

        [JwtAuthentication]
        [Route("api/DistributorAndTeamUnSoldsTags")]
        public HttpResponseMessage DistributorAndTeamUnSoldsTags(HttpRequestMessage request, dist d)
        {
     
            return request.CreateResponse(HttpStatusCode.OK, db.DistributorAndTeamUnSoldsTags(d.distributorid.ToString()));
        }
        
        [JwtAuthentication]
        [Route("api/DistributorAndTeamSoldsTags")]
        public HttpResponseMessage DistributorAndTeamSoldsTags(HttpRequestMessage request, disttransactions d)
        {

           return request.CreateResponse(HttpStatusCode.OK, db.DistributorAndTeamSoldsTags(d.distributorid.ToString(), d.startdate, d.enddate));
        }
       
        [JwtAuthentication]
        [Route("api/DistributorAccPassbookByDistID")]
        public HttpResponseMessage DistributorAccPassbookByDistID(HttpRequestMessage request, disttransactions d)
        {

            return request.CreateResponse(HttpStatusCode.OK, db.DistributorAccPassbook(d.distributorid.ToString(), d.startdate, d.enddate));
        }
        [JwtAuthentication]
        [Route("api/DistributorSalesReport")]
        public HttpResponseMessage DistributorSalesReport(HttpRequestMessage request, dist d)
        {
            if (string.Empty == d.distributorid)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
                 

            return request.CreateResponse(HttpStatusCode.OK, db.salesreportbyDistributorId(d.distributorid));
        }
        [JwtAuthentication]
        [Route("api/GetToken")]
        public IHttpActionResult GetToken()
        {
            //EncryptionsUtility.GetDecToken();
            return Ok("Service is Running- New Token Created");
        }

        [JwtAuthentication]
        [Route("api/GetRegions")]
        public IHttpActionResult GetAllRegions()
        {

            string strulr = "https://fastag.gitechnology.in/indusindAPI/api/BC/LookUp/GetRegion";
            string methodtype = "GET";
            //\"dkErdTV3aVVPVEJnU05JSkdncmFUeEV0QkU3NjAvQVJKejdWMmhIejVwbmYrbWV1Wm80bzl0VStDcDBCaXZKU255ck5IM1VOQXZBPQ2\"
            string thetoken = EncryptionsUtility.getheader(strulr, methodtype);
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var client = new RestClient(strulr);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", thetoken);
            request.AddParameter("text/plain", "", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            if (response.Content == null)
            {
                return BadRequest();
            }
            else
            {
                try
                {
                    TokenResponse json = JsonConvert.DeserializeObject<TokenResponse>(response.Content);
                    var x = json.ResponseData;// this extracts the encrypted requesttoken;


                    string resData = EncryptionsUtility.AES_DECRYPT(x, AESkey);
                    clsRegiondetail objRegions = JsonConvert.DeserializeObject<clsRegiondetail>(resData);
                    return Ok(objRegions);

                }
                catch (Exception ex)
                {
                    //EncryptionsUtility En = new EncryptionsUtility();
                    //await En.GetDecToken();
                    return BadRequest(ex.Message);

                }

            }

        }


        // post method - for GetAllStates()
        //https://localhost:44391/api/ListStates


         [JwtAuthentication]
        [Route("api/ListStates")]
        public IHttpActionResult PostAllStates([FromBody] clsInGetState regionId)
        {
            string strulr = "https://fastag.gitechnology.in/indusindAPI/api/BC/LookUp/GetState";
            string methodtype = "POST";
            string thetoken = EncryptionsUtility.getheader(strulr, methodtype);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                clsInGetState stateObj = new clsInGetState();
                stateObj.RegionID = regionId.RegionID;//"2";
                string regionInput = JsonConvert.SerializeObject(stateObj);
                string encryptedRegion = EncryptionsUtility.AES_ENCRYPT(regionInput, AESkey);
                ClsRequestData objRequestData = new ClsRequestData() { RequestData = encryptedRegion };
                regionInput = JsonConvert.SerializeObject(objRequestData);
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var client = new RestClient(strulr);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", thetoken);
                //request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", regionInput, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (response == null)
                {
                    return NotFound();
                }
                else
                {
                    try
                    {
                        TokenResponse json = JsonConvert.DeserializeObject<TokenResponse>(response.Content);

                        var resData = json.ResponseData;
                        // "B214B32154A9472936EAA4493D3D2A4A9EA09A173CABDE28FDA034A47DE360D7D4327870412BB2D4C1C78CAB42F9783014AD760984C2A8E172D30E09337B425BAA4BD608311340DCC728026C2473789D"; // // this extracts the encrypted requesttoken;
                        //Console.WriteLine("encrypted value " + x);
                        string decResult = EncryptionsUtility.AES_DECRYPT(resData, AESkey);
                        clsOutGetState objStateslist = JsonConvert.DeserializeObject<clsOutGetState>(decResult);
                        return Ok(objStateslist);
                    }
                    catch (Exception ex)
                    {

                        return BadRequest(ex.Message);
                    }
                }

            }

        }



         [JwtAuthentication]
        [Route("api/GetAllCities")]
        public IHttpActionResult PostAllCities([FromBody] clsInGetCity data) //string RegionID, string StateID
        {
            string strulr = "https://fastag.gitechnology.in/indusindAPI/api/BC/LookUp/GetCity";
            string methodtype = "POST";
            string thetoken = EncryptionsUtility.getheader(strulr, methodtype);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {

                clsInGetCity cityInObj = new clsInGetCity() { RegionID = data.RegionID, StateID = data.StateID };
                string serialInputCity = JsonConvert.SerializeObject(cityInObj);
                string encryptedCityInput = EncryptionsUtility.AES_ENCRYPT(serialInputCity, AESkey);
                ClsRequestData objRequestData = new ClsRequestData() { RequestData = encryptedCityInput };
                serialInputCity = JsonConvert.SerializeObject(objRequestData);
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var client = new RestClient(strulr);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", thetoken);
                //request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", serialInputCity, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
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
                        ClsCitytOutDetails objCities = JsonConvert.DeserializeObject<ClsCitytOutDetails>(decResult);
                        return Ok(objCities);
                    }
                    catch (Exception ex)
                    {

                        return BadRequest(ex.Message);
                    }

                }


            }

        }


        //https://localhost:44391/api/GetIdType
         [JwtAuthentication]
        [Route("api/GetIdType")]
        public IHttpActionResult GetAllIdType()
        {
            string strulr = "https://fastag.gitechnology.in/indusindAPI/api/BC/LookUp/GetProofType";
            string methodtype = "GET";
            string thetoken = EncryptionsUtility.getheader(strulr, methodtype);
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var client = new RestClient(strulr);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", thetoken);
            IRestResponse response = client.Execute(request);
            TokenResponse json = JsonConvert.DeserializeObject<TokenResponse>(response.Content);
            var x = json.ResponseData;// this extracts the encrypted restoken;
            string s1 = EncryptionsUtility.AES_DECRYPT(x, AESkey);
            clsGetIDType objIdType = JsonConvert.DeserializeObject<clsGetIDType>(s1);

            if (response == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    TokenResponse resjson = JsonConvert.DeserializeObject<TokenResponse>(response.Content);
                    var encryptRes = resjson.ResponseData;// this extracts the encrypted resttoken;


                    string resData = EncryptionsUtility.AES_DECRYPT(encryptRes, AESkey);
                    clsGetIDType objIdTypes2 = JsonConvert.DeserializeObject<clsGetIDType>(resData);
                    return Ok(objIdTypes2);
                }
                catch (Exception ex)
                {

                    return BadRequest(ex.Message);
                }
            }

        }


        //https://localhost:44391/api/WalletCustRegistration
         [JwtAuthentication]
        [Route("api/WalletCustRegistration")]
        public IHttpActionResult PostWalletCustRegistration([FromBody] IndusInd_CustomerRegistration cust)
        {
            string strulr = "https://fastag.gitechnology.in/indusindAPI/api/BC/WalletIndividual/CustomerRegistration";
            string methodtype = "POST";
            string thetoken = EncryptionsUtility.getheader(strulr, methodtype);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {

                clsWalletCustomerRegistration objCustReg = new clsWalletCustomerRegistration()
                {
                    TransactionID = cust.TransactionID,
                    FirstName = cust.FirstName,
                    LastName = cust.LastName,
                    Gender = Convert.ToString(cust.Gender),
                    EmailID = cust.EmailID,
                    DOB = cust.DOB,
                    MobileNumber = cust.MobileNumber,
                    MotherName = cust.MotherName,
                    Address = cust.Address,
                    RegionID = Convert.ToString(cust.RegionID),
                    StateID = Convert.ToString(cust.StateID),
                    CityID = Convert.ToString(cust.CityID),
                    Pincode = Convert.ToString(cust.Pincode),
                    GST = cust.GST,
                    Aadhaarno = cust.Aadhaarno,
                    PANNo = cust.PANNo

                };
                string serialobjCustReg = JsonConvert.SerializeObject(objCustReg);
                string encryptedserialobjCustReg = EncryptionsUtility.AES_ENCRYPT(serialobjCustReg, AESkey);
                ClsRequestData objRequestData = new ClsRequestData() { RequestData = encryptedserialobjCustReg };
                encryptedserialobjCustReg = JsonConvert.SerializeObject(objRequestData);

                //Invoke Rest Service
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var client = new RestClient(strulr);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", thetoken);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", encryptedserialobjCustReg, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response == null)
                {
                    return NotFound();
                }
                else
                {
                    try
                    {

                        TokenResponse json = JsonConvert.DeserializeObject<TokenResponse>(response.Content);
                        var x = json.ResponseData;// this extracts the encrypted requesttoken;
                        string s1 = EncryptionsUtility.AES_DECRYPT(x, AESkey);
                        ClsCustRegResponse objCustRegResponse = JsonConvert.DeserializeObject<ClsCustRegResponse>(s1);
                        Distributor_UsersApp dua = new Distributor_UsersApp();




                        int tollpaycustid = dua.createTollPayCustomer(cust);
                        cust.TransactionFromIPAddress = HttpContext.Current != null ? HttpContext.Current.Request.UserHostAddress : "";
                        cust.TollPayCustId = tollpaycustid;

                        objCustRegResponse.Status += " {'TollPayCustId' :" + tollpaycustid + "'IndusTranId:'" + cust.TransactionID;
                        IndusInd_CustomerRegistration icr = db.IndusInd_CustomerRegistration.Where(i => i.MobileNumber == cust.MobileNumber).FirstOrDefault();
                        if (icr != null) //if already exist update this details.
                        {

                            icr.TransactionID = cust.TransactionID;
                            icr.TransactionDate = DateTime.Now;
                            icr.TransactionFromIPAddress = cust.TransactionFromIPAddress;
                            icr.Aadhaarno = cust.Aadhaarno;
                            icr.Address = cust.Address;
                            icr.CityID = cust.CityID;
                            icr.DistributorID = cust.DistributorID;
                            icr.DOB = cust.DOB;
                            icr.EmailID = cust.EmailID;
                            icr.FirstName = cust.FirstName;
                            icr.Gender = cust.Gender;
                            icr.GST = cust.GST;
                            icr.LastName = cust.LastName;
                            icr.MotherName = cust.MotherName;
                            icr.PANNo = cust.PANNo;
                            icr.Pincode = cust.Pincode;
                            icr.RegionID = cust.RegionID;
                            icr.StateID = cust.StateID;
                            icr.TollPayCustId = cust.TollPayCustId;
                            db.SaveChanges();
                        }
                        else //if not exist add customer
                        {

                            db.IndusInd_CustomerRegistration.Add(cust);
                            db.SaveChanges();
                        }
                        return Ok(objCustRegResponse);
                    }
                    catch (Exception ex)
                    {

                        return BadRequest(ex.Message);
                    }
                }
            }


        }

        //https://localhost:44391/api/WalletCustRegistration
         [JwtAuthentication]
        [Route("api/CustomerRegistrationNonIndividual")]
        public IHttpActionResult CustomerRegistrationNonIndividual([FromBody] IndusInd_CustomerRegistration cust)
        {
            string strulr = "https://fastag.gitechnology.in/indusindAPI/api/BC/WalletIndividual/CustomerRegistrationNonIndividual";
            string methodtype = "POST";
            string thetoken = EncryptionsUtility.getheader(strulr, methodtype);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {

                CustomerRegistrationNonIndividual objCustReg = new CustomerRegistrationNonIndividual()
                {
                    TransactionID = cust.TransactionID,
                    Name = cust.FirstName   , // + " " + cust.LastName,
                    ContactPerson = cust.FirstName + " " + cust.LastName,
                    ContactNumber = cust.MobileNumber,
                    AddressProofID = cust.AddressProofID,
                    AddressProofImage = cust.AddressProofImage,
                    KycProofID = cust.KycProofID,
                    KycProofImage = cust.KycProofImage,
                    Address = cust.Address,
                    RegionId = Convert.ToString(cust.RegionID),
                    StateId = Convert.ToString(cust.StateID),
                    CityId = Convert.ToString(cust.CityID),
                    PinCode = Convert.ToString(cust.Pincode),
                    EmailId = cust.EmailID,
                    GST = cust.GST,
                    CIN=cust.CIN,
                    Entity = cust.Entity,
                    PAN = cust.PANNo

                };
                string serialobjCustReg = JsonConvert.SerializeObject(objCustReg);
                string encryptedserialobjCustReg = EncryptionsUtility.AES_ENCRYPT(serialobjCustReg, AESkey);
                ClsRequestData objRequestData = new ClsRequestData() { RequestData = encryptedserialobjCustReg };
                encryptedserialobjCustReg = JsonConvert.SerializeObject(objRequestData);

                //Invoke Rest Service
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var client = new RestClient(strulr);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", thetoken);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", encryptedserialobjCustReg, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response == null)
                {
                    return NotFound();
                }
                else
                {
                    try
                    {

                        TokenResponse json = JsonConvert.DeserializeObject<TokenResponse>(response.Content);
                        var x = json.ResponseData;// this extracts the encrypted requesttoken;
                        string s1 = EncryptionsUtility.AES_DECRYPT(x, AESkey);
                        ClsCustRegResponse objCustRegResponse = JsonConvert.DeserializeObject<ClsCustRegResponse>(s1);
                        Distributor_UsersApp dua = new Distributor_UsersApp();




                        int tollpaycustid = dua.createTollPayCustomer(cust);
                        cust.TransactionFromIPAddress = HttpContext.Current != null ? HttpContext.Current.Request.UserHostAddress : "";
                        cust.TollPayCustId = tollpaycustid;

                        objCustRegResponse.Status += " {'TollPayCustId' :" + tollpaycustid + "'IndusTranId:'" + cust.TransactionID;
                        IndusInd_CustomerRegistration icr = db.IndusInd_CustomerRegistration.Where(i => i.MobileNumber == cust.MobileNumber).FirstOrDefault();
                        if (icr != null) //if already exist update this details.
                        {

                            icr.TransactionID = cust.TransactionID;
                            icr.TransactionDate = DateTime.Now;
                            icr.TransactionFromIPAddress = cust.TransactionFromIPAddress;
                            icr.Aadhaarno = cust.Aadhaarno;
                            icr.Address = cust.Address;
                            icr.CityID = cust.CityID;
                            icr.DistributorID = cust.DistributorID;
                            icr.DOB = cust.DOB;
                            icr.CIN = cust.CIN;
                            icr.EmailID = cust.EmailID;
                            icr.FirstName = cust.FirstName;
                            icr.Gender = cust.Gender;
                            icr.GST = cust.GST;
                            icr.LastName = cust.LastName;
                            icr.MotherName = cust.MotherName;
                            icr.PANNo = cust.PANNo;
                            icr.Pincode = cust.Pincode;
                            icr.RegionID = cust.RegionID;
                            icr.StateID = cust.StateID;
                            icr.TollPayCustId = cust.TollPayCustId;
                            db.SaveChanges();
                        }
                        else //if not exist add customer
                        {

                            db.IndusInd_CustomerRegistration.Add(cust);
                            db.SaveChanges();
                        }
                        return Ok(objCustRegResponse);
                    }
                    catch (Exception ex)
                    {

                        return BadRequest(ex.Message);
                    }
                }
            }


        }

        //https://localhost:44391/api/WalletCustRegistrationResendOTP
         [JwtAuthentication]
        [Route("api/WalletCustRegistrationResendOTP")]
        public IHttpActionResult PostCustomerRegResendOTP([FromBody] clsWalletCustomerRegistrationResendOTP custOTP)
        {
            string strulr = "https://fastag.gitechnology.in/indusindAPI/api/BC/WalletIndividual/CustomerRegistration/OTPResend";
            string methodtype = "POST";
            string thetoken = EncryptionsUtility.getheader(strulr, methodtype);
            ClsCustRegResponse obj = null;
            clsWalletCustomerRegistrationResendOTP custOTPResendObj = new clsWalletCustomerRegistrationResendOTP()
            {
                TransactionID = custOTP.TransactionID,
                MobileNumber = custOTP.MobileNumber
            };
            string serialobjCust = JsonConvert.SerializeObject(custOTPResendObj);
            // Console.WriteLine(serialobjCustReg);

            string encryptcustOTPResendObj = EncryptionsUtility.AES_ENCRYPT(serialobjCust, AESkey);
            Console.WriteLine(encryptcustOTPResendObj);

            ClsRequestData objRequestData = new ClsRequestData() { RequestData = encryptcustOTPResendObj };
            encryptcustOTPResendObj = JsonConvert.SerializeObject(objRequestData);

            //invoke service
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var client = new RestClient(strulr);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", thetoken);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", encryptcustOTPResendObj, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            if (response == null)
            {
                BadRequest();
            }
            else
            {
                try
                {
                    TokenResponse json = JsonConvert.DeserializeObject<TokenResponse>(response.Content);
                    var x = json.ResponseData;// this extracts the encrypted requesttoken;

                    string s1 = EncryptionsUtility.AES_DECRYPT(x, AESkey);
                    Console.WriteLine(s1);
                    obj = JsonConvert.DeserializeObject<ClsCustRegResponse>(s1);

                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return Ok(obj);
        }

        //https://localhost:44391/api/WalletCustRegistrationResendOTP
         [JwtAuthentication]
        [Route("api/WalletCustValidatedOTP")]
        public IHttpActionResult PostCustomerValidatedOTP([FromBody] clsWalletCustomerRegistrationOTPValidation custValidOTP)
        {
            string strulr = "https://fastag.gitechnology.in/indusindAPI/api/BC/WalletIndividual/CustomerRegistration/OTPValidation";
            string methodtype = "POST";

                string thetoken = EncryptionsUtility.getheader(strulr, methodtype);
            clsWalletCustomerRegistrationOTPValidation objOTPValid = new clsWalletCustomerRegistrationOTPValidation()
            {
                TransactionID = custValidOTP.TransactionID,
                MobileNumber = custValidOTP.MobileNumber,
                OTP = custValidOTP.OTP
            };
            string serialobjCust = JsonConvert.SerializeObject(objOTPValid);
            string encryptobjOTPValid = EncryptionsUtility.AES_ENCRYPT(serialobjCust, AESkey);
            ClsRequestData objRequestData = new ClsRequestData() { RequestData = encryptobjOTPValid };
            encryptobjOTPValid = JsonConvert.SerializeObject(objRequestData);
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var client = new RestClient(strulr);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", thetoken);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", encryptobjOTPValid, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            if (response == null)
            {
                return BadRequest();
            }
            else
            {
                try
                {
                    TokenResponse json = JsonConvert.DeserializeObject<TokenResponse>(response.Content);
                    var x = json.ResponseData;// this extracts the encrypted requesttoken;
                    string s1 = EncryptionsUtility.AES_DECRYPT(x, AESkey);

                    ClsCustRegResponse objResponse = JsonConvert.DeserializeObject<ClsCustRegResponse>(s1);
                    //if objResponse
                    if (objResponse.StatusCode == "000")
                    {
                        IndusInd_CustomerRegistration icr = db.IndusInd_CustomerRegistration.Where(c => c.MobileNumber == custValidOTP.MobileNumber).FirstOrDefault();
                        if (icr.isOTPVerified != "Yes")
                        {
                            string password = db.Users.Where(u => u.mobno1 == icr.MobileNumber).FirstOrDefault().password;
                            string smsmsg = "Dear Customer, Use Web https://Tollpay.in with Username=" + icr.MobileNumber + ", Password=" + password;
                            ns.sendsms("smsfrom", icr.MobileNumber, smsmsg);
                            ns.sendemail(icr.EmailID, "", "Welcome to icr.MobileNumber", smsmsg);
                            icr.TransactionDate = DateTime.Now;
                            icr.isOTPVerified = "Yes";
                            db.SaveChanges();
                           
                        }
                    }
                    return Ok(objResponse);
                }
                catch (Exception ex)
                {

                    return BadRequest(ex.Message);
                }

            }
        }


        //https://localhost:44391/api/GetVehicelClass
         [JwtAuthentication]
        [Route("api/GetVehicelClass")]
        public IHttpActionResult GetVehicelClass()
        {
            string strulr = "https://fastag.gitechnology.in/indusindAPI/api/BC/Tag/get_vehicleclass";
            string methodtype = "GET";
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string thetoken = EncryptionsUtility.getheader(strulr, methodtype);
            ClsVehicleClass obj = null;
            var client = new RestClient(strulr);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);

            request.AddHeader("Authorization", thetoken);
            IRestResponse response = client.Execute(request);
            if (response == null)
            {
                BadRequest();
            }
            else
            {
                try
                {
                    TokenResponse json = JsonConvert.DeserializeObject<TokenResponse>(response.Content);
                    var x = json.ResponseData;// this extracts the encrypted requesttoken;
                                              //  Console.WriteLine("encryoted value is here");
                                              // Console.WriteLine(x);

                    string s1 = EncryptionsUtility.AES_DECRYPT(x, AESkey);
                    obj = JsonConvert.DeserializeObject<ClsVehicleClass>(s1);
                }
                catch (Exception ex)
                {

                    return BadRequest(ex.Message);
                }

            }
            return Ok(obj);
        }

         [JwtAuthentication]
        [Route("api/FindWalletInfo")]
        public IHttpActionResult Postgetwalletinfo([FromBody] ClsInputWalletInfo objinputwallett)
        {


            ClsInputWalletInfo walletInputObj = new ClsInputWalletInfo() { MobileNumber = objinputwallett.MobileNumber }; //objinputwallett.MobileNumber

            clsRespFindWalletInfo obj = null;

            string strulr = "https://fastag.gitechnology.in/indusindAPI/api/BC/Tag/getwalletinfo";
            string methodtype = "POST";
            string thetoken = EncryptionsUtility.getheader(strulr, methodtype);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                string walletInput = JsonConvert.SerializeObject(walletInputObj);
                //  Console.WriteLine(walletInput);
                string encryptedwalletInput = EncryptionsUtility.AES_ENCRYPT(walletInput, AESkey);

                ClsRequestData objRequestData = new ClsRequestData() { RequestData = encryptedwalletInput };
                string walletInputserial = JsonConvert.SerializeObject(objRequestData);
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var client = new RestClient(strulr);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", thetoken);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", walletInputserial, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response == null)
                {
                    BadRequest();
                }
                else
                {
                    try
                    {
                        TokenResponse json = JsonConvert.DeserializeObject<TokenResponse>(response.Content);
                        var x = json.ResponseData;// this extracts the encrypted requesttoken;

                        string s1 = EncryptionsUtility.AES_DECRYPT(x, AESkey);
                        Console.WriteLine(s1);
                        obj = JsonConvert.DeserializeObject<clsRespFindWalletInfo>(s1);
                        if (obj.StatusCode == "000")
                        {
                            string mobileno = obj.WalletInfoDetails[0].MobileNo;
                            IndusInd_CustomerRegistration cust = db.IndusInd_CustomerRegistration.Where(o => o.MobileNumber == mobileno).FirstOrDefault();

                            obj.WalletInfoDetails[0].PANNO = cust.PANNo;
                            obj.WalletInfoDetails[0].AddharNO = cust.Aadhaarno;
                        }
                            return Ok(obj);

                    }
                    catch (Exception ex)
                    {

                        return BadRequest(ex.Message);
                    }

                }

            }
            return Ok(obj);

        }

         [JwtAuthentication]
        [Route("api/WalletRecharge")]
        public IHttpActionResult PostWalletRecharge([FromBody] ClsRechargeWallet objRechargeWallet)
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

                //test data from API doc
                //ClsRechargeWallet objWallet = new ClsRechargeWallet()
                //{
                //    TransactionID = "GI1234",
                //    PaymentType = 1,
                //    CustomerID = 162253,
                //    TagAccountNo = "",
                //    MobileNumber = "9789300546",
                //    Amount = 100.0f,
                //    RechargePercentage = 1.5,
                //    TotalAmount = 101.5f
                //};

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
                        ResponseWalletRecharge objCities = JsonConvert.DeserializeObject<ResponseWalletRecharge>(decResult);
                        return Ok(objCities);
                    }
                    catch (Exception ex)
                    {

                        return BadRequest(ex.Message);
                    }

                }


            }

        }


        //RechargeTagFromWallet()
         [JwtAuthentication]
        [Route("api/recharge_tag_from_wallet")]
        public IHttpActionResult PostRechargeTagFromWallet([FromBody] RechargeTagFromWallet ObjFromWallet)
        {

            //test datagiven in api doc
            //RechargeTagFromWallet obj = new RechargeTagFromWallet()
            //{
            //    TransactionID = "GI1234",
            //    CustomerID = "100793",
            //    TagAccountNo = "1007930001",
            //    Amount = 300
            //};
            // { "TransactionID":"GI1234","MobileNumber":"9789300546","TagAccountNo":"1007930001","Amount":300}

            string strulr = "https://fastag.gitechnology.in/indusindAPI/api/BC/Tag/recharge_tag_from_wallet";
            string methodtype = "POST";
            string thetoken = EncryptionsUtility.getheader(strulr, methodtype);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                RechargeTagFromWallet obj = new RechargeTagFromWallet()
                {
                    TransactionID = ObjFromWallet.TransactionID,
                    //CustomerID = ObjFromWallet.CustomerID,
                    TagAccountNo = ObjFromWallet.TagAccountNo,
                    Amount = ObjFromWallet.Amount,
                    MobileNumber = ObjFromWallet.MobileNumber
                };

                string serialWallet = JsonConvert.SerializeObject(obj);
                string encryptedWallet = EncryptionsUtility.AES_ENCRYPT(serialWallet, AESkey);
                ClsRequestData objRequestData = new ClsRequestData() { RequestData = encryptedWallet };
                serialWallet = JsonConvert.SerializeObject(objRequestData);
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var client = new RestClient(strulr);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", thetoken);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", serialWallet, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response == null)
                {
                    return NotFound();
                }
                else
                {

                    try
                    {
                        TokenResponse json = JsonConvert.DeserializeObject<TokenResponse>(response.Content);
                        var x = json.ResponseData;// this extracts the encrypted requesttoken;
                        string s1 = EncryptionsUtility.AES_DECRYPT(x, AESkey);
                        ClsCustRegResponse objresponse = JsonConvert.DeserializeObject<ClsCustRegResponse>(s1);
                        return Ok(objresponse);
                    }
                    catch (Exception ex)
                    {

                        return BadRequest(ex.Message);
                    }
                }


            }
        }

        //MoveTagFromWallet
         [JwtAuthentication]
        [Route("api/move_to_wallet_from_tag")]

        public IHttpActionResult PostMoveTagFromWallet([FromBody] RechargeTagFromWallet ObjFromWallet)
        {

            //test datagiven in api doc
            //RechargeTagFromWallet obj = new RechargeTagFromWallet()
            //{
            //    TransactionID = "GI1234",
            //    CustomerID = "100793",
            //    TagAccountNo = "1007930001",
            //    Amount = 300
            //};
            //{"TransactionID":"GI1234","MobileNumber":"9789300546","TagAccountNo":"1007930001","Amount":300}
            string strulr = "https://fastag.gitechnology.in/indusindAPI/api/BC/Tag/move_to_wallet_from_tag";
            string methodtype = "POST";
            string thetoken = EncryptionsUtility.getheader(strulr, methodtype);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                RechargeTagFromWallet obj = new RechargeTagFromWallet()
                {
                    TransactionID = ObjFromWallet.TransactionID,
                    MobileNumber = ObjFromWallet.MobileNumber,
                    TagAccountNo = ObjFromWallet.TagAccountNo,
                    Amount = ObjFromWallet.Amount
                };

                string serialWallet = JsonConvert.SerializeObject(obj);
                string encryptedWallet = EncryptionsUtility.AES_ENCRYPT(serialWallet, AESkey);
                ClsRequestData objRequestData = new ClsRequestData() { RequestData = encryptedWallet };
                serialWallet = JsonConvert.SerializeObject(objRequestData);
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var client = new RestClient(strulr);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", thetoken);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", serialWallet, ParameterType.RequestBody);
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
                        var x = json.ResponseData;// this extracts the encrypted requesttoken;
                        string s1 = EncryptionsUtility.AES_DECRYPT(x, AESkey);
                        ClsCustRegResponse objresponse = JsonConvert.DeserializeObject<ClsCustRegResponse>(s1);
                        return Ok(objresponse);
                    }
                    catch (Exception ex)
                    {

                        return BadRequest(ex.Message);
                    }
                }


            }
        }






        //https://localhost:44391/api/TagRegistration
         [JwtAuthentication]
        [Route("api/TagRegistration")]
        public IHttpActionResult PostTagRegistration([FromBody] ClsTollPayTagRegistraion ObjTPTagReg)
        {


            string strulr = "https://fastag.gitechnology.in/indusindAPI/api/BC/Tag/tag_registration";
            string methodtype = "POST";
            string thetoken = EncryptionsUtility.getheader(strulr, methodtype);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                ClsInputTagRegistraion ObjTagReg = new ClsInputTagRegistraion();
                ObjTagReg.VehicleInfo = ObjTPTagReg.VehicleInfo;
                ObjTagReg.MobileNo = ObjTPTagReg.MobileNo;
                ObjTagReg.OrderNo = ObjTPTagReg.OrderNo;
                ObjTagReg.PaymentType = ObjTPTagReg.PaymentType;
                ObjTagReg.ShippingAddress = ObjTPTagReg.ShippingAddress;
                ObjTagReg.TagTotalAmount = ObjTPTagReg.TagTotalAmount;
                ObjTagReg.AccountType = ObjTPTagReg.AccountType;

                string clsTagInput = JsonConvert.SerializeObject(ObjTagReg);

                string encryptedClsTagInput = EncryptionsUtility.AES_ENCRYPT(clsTagInput, AESkey);

                ClsRequestData objRequestData = new ClsRequestData() { RequestData = encryptedClsTagInput };
                string clsTagInputserial = JsonConvert.SerializeObject(objRequestData);

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var client = new RestClient(strulr);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", thetoken);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", clsTagInputserial, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (response == null)
                {
                    return NotFound();
                }
                else
                {
                    try
                    {
                        //ResposeTagRootobject
                        TokenResponse json = JsonConvert.DeserializeObject<TokenResponse>(response.Content);
                        var x = json.ResponseData;// this extracts the encrypted requesttoken;

                        string s1 = EncryptionsUtility.AES_DECRYPT(x, AESkey);
                        Console.WriteLine(s1);
                        ResposeTagRootobject obj = JsonConvert.DeserializeObject<ResposeTagRootobject>(s1);
                        Distributor_UsersApp dua = new Distributor_UsersApp();


                        if (obj.StatusCode == "000")
                        {
                            foreach (var vehicle in obj.VehicleInfo)
                            {
                                if (vehicle.StatusCode == "000")
                                {
                                    dua.distributorFasttag(obj, ObjTPTagReg);
                                }
                                else
                                {
                                    return Ok(obj);

                                }
                            }

                        }
                        return Ok(obj);
                    }

                    catch (Exception ex)
                    {

                        return BadRequest(ex.Message);
                    }
                }

            }


        }


        //RequeryTagregistration()//https://localhost:44391/api/RequeryTagregistration
         [JwtAuthentication]
        [Route("api/RequeryTagregistration")]
        public IHttpActionResult PostRequeryTagregistration([FromBody] ClsInputRequerytagreg objReqTag)
        {
            string strulr = "https://fastag.gitechnology.in/indusindAPI/api/BC/Tag/requery_tag_registration";
            string methodtype = "POST";
            string thetoken = EncryptionsUtility.getheader(strulr, methodtype);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else

            {
                ClsInputRequerytagreg obj = new ClsInputRequerytagreg()
                { OrderNo = objReqTag.OrderNo, SerialNo = objReqTag.SerialNo };

                string clsTagInput = JsonConvert.SerializeObject(obj);

                string encryptedClsTagInput = EncryptionsUtility.AES_ENCRYPT(clsTagInput, AESkey);

                ClsRequestData objRequestData = new ClsRequestData() { RequestData = encryptedClsTagInput };
                string clsTagInputserial = JsonConvert.SerializeObject(objRequestData);

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var client = new RestClient(strulr);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", thetoken);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", clsTagInputserial, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (response == null)
                {
                    return NotFound();
                }
                else
                {
                    try
                    {

                        TokenResponse json = JsonConvert.DeserializeObject<TokenResponse>(response.Content);
                        var x = json.ResponseData;// this extracts the encrypted requesttoken;
                                                  //response decryption

                        string s1 = EncryptionsUtility.AES_DECRYPT(x, AESkey);
                        ResponseReqTagReg objrespone = JsonConvert.DeserializeObject<ResponseReqTagReg>(s1);
                        return Ok(objrespone);

                    }
                    catch (Exception ex)
                    {

                        return BadRequest(ex.Message);
                    }

                }


            }

        } //method ends 



         [JwtAuthentication]
        [Route("api/TagReplacement")]
        public IHttpActionResult TagReplacement([FromBody] TagReplacement objRepTag)
        {
            string strulr = "https://fastag.gitechnology.in/indusindAPI/api/BC/Tag/TagReplacement";
            string methodtype = "POST";
            string thetoken = EncryptionsUtility.getheader(strulr, methodtype);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else

            {
                
                string clsTagInput = JsonConvert.SerializeObject(objRepTag);

                string encryptedClsTagInput = EncryptionsUtility.AES_ENCRYPT(clsTagInput, AESkey);

                ClsRequestData objRequestData = new ClsRequestData() { RequestData = encryptedClsTagInput };
                string clsTagInputserial = JsonConvert.SerializeObject(objRequestData);

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var client = new RestClient(strulr);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", thetoken);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", clsTagInputserial, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (response == null)
                {
                    return NotFound();
                }
                else
                {
                    try
                    {

                        TokenResponse json = JsonConvert.DeserializeObject<TokenResponse>(response.Content);
                        var x = json.ResponseData;// this extracts the encrypted requesttoken;
                                                  //response decryption

                        string s1 = EncryptionsUtility.AES_DECRYPT(x, AESkey);
                        ClsCustRegResponse objrespone = JsonConvert.DeserializeObject<ClsCustRegResponse>(s1);
                        return Ok(objrespone);

                    }
                    catch (Exception ex)
                    {

                        return BadRequest(ex.Message);
                    }

                }


            }

        } //method ends 


         [JwtAuthentication]
        [Route("api/TagStatus")]
        public IHttpActionResult TagStatus([FromBody] TagStatus ts)
        {
            string strulr = "https://fastag.gitechnology.in/indusindAPI/api/BC/Tag/TagStatus";
            string methodtype = "POST";
            string thetoken = EncryptionsUtility.getheader(strulr, methodtype);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else

            {

                string clsTagInput = JsonConvert.SerializeObject(ts);

                string encryptedClsTagInput = EncryptionsUtility.AES_ENCRYPT(clsTagInput, AESkey);

                ClsRequestData objRequestData = new ClsRequestData() { RequestData = encryptedClsTagInput };
                string clsTagInputserial = JsonConvert.SerializeObject(objRequestData);

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var client = new RestClient(strulr);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", thetoken);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", clsTagInputserial, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (response == null)
                {
                    return NotFound();
                }
                else
                {
                    try
                    {

                        TokenResponse json = JsonConvert.DeserializeObject<TokenResponse>(response.Content);
                        var x = json.ResponseData;// this extracts the encrypted requesttoken;
                                                  //response decryption

                        string s1 = EncryptionsUtility.AES_DECRYPT(x, AESkey);
                        TagStatusResponse objrespone = JsonConvert.DeserializeObject<TagStatusResponse>(s1);
                        return Ok(objrespone);

                    }
                    catch (Exception ex)
                    {

                        return BadRequest(ex.Message);
                    }

                }


            }

        } //method ends 
        [JwtAuthentication]
        [Route("api/TransactionHistoryByDistributor")]
        public IHttpActionResult TransactionHistoryByDistributor([FromBody] clsDistCustomerTagTranHistory cdctth)
        {
            //get all customer mobile nos
           // List<int> distteam = db.Users.Where(u => u.srno == cdctth.DistributorId || u.DistributorId == cdctth.DistributorId).Select(s => s.srno).ToList();
         //    db.ecom_RFID.Where(a => distteam.Contains(Convert.ToInt32(a.ecom_DistributionID))).ToList();
            List<ClsInputTransactionHistory> didcustomertags = new List<ClsInputTransactionHistory>();
         
            didcustomertags = db.DistributorAndTeamCustomerList(cdctth.DistributorId.ToString(), Convert.ToDateTime( cdctth.FromDate), Convert.ToDateTime(cdctth.ToDate))
                             .Select(s=> new ClsInputTransactionHistory() { FromDate= cdctth.FromDate,ToDate= cdctth.ToDate,MobileNumber=s.ecom_CustomerMobNo, TagID=s.ecom_RFIDTagSrNo }).ToList();
            decimal TotalAllCustomerCredit=0;
            decimal TotalAllCustomerDebit=0;
            List<CustomerVehicle> CustomerVehicleList = new List<CustomerVehicle>();
            foreach (var item in didcustomertags)
            {
                 CustomerVehicle cv = new CustomerVehicle();
                 cv.CustomerMobileNo = item.MobileNumber;
                 IHttpActionResult res = PostTransactionHistory(item);
                 dynamic aResponseTranHistory = res as OkNegotiatedContentResult<ResponseTranHistory>;
            
                 if(aResponseTranHistory.Content.StatusCode=="000")
                {
                    Transactiondetail[] tdl =  aResponseTranHistory.Content.TransactionDetails;
                    if (tdl.Where(s => s.TransactionType == "Debit").Count() > 0)
                    {
                        cv.CustomerVehicleNo = tdl.Where(s => s.TransactionType == "Debit").FirstOrDefault().VehicleNo;
                        cv.TotalAllCustomerDebit = tdl.Where(s => s.TransactionType == "Debit").Sum(s => Convert.ToDecimal(s.Amount));
                    }
                    cv.TotalAllCustomerCredit= tdl.Where(s => s.TransactionType == "Credit").Sum(s => Convert.ToDecimal(s.Amount));
               
                    TotalAllCustomerCredit += cv.TotalAllCustomerCredit;
                    TotalAllCustomerDebit += cv.TotalAllCustomerDebit;
                }
                CustomerVehicleList.Add(cv);
            }
            clsDistCustomerTagTranHistorySummary summary = new clsDistCustomerTagTranHistorySummary();
            summary.TotalAllCustomerCredit = TotalAllCustomerCredit;
            summary.TotalAllCustomerDebit = TotalAllCustomerDebit;
            summary.CustomerVehicleWiseDebit = CustomerVehicleList;
                return Ok(summary);
        }

            [JwtAuthentication]
        [Route("api/TransactionHistory")]
        public IHttpActionResult PostTransactionHistory([FromBody] ClsInputTransactionHistory objHistory)
        {
            ResponseTranHistory objresponse = null;
            string strulr = "https://fastag.gitechnology.in/indusindAPI/api/BC/Tag/GetTransactionHistory";
            string methodtype = "POST";

            string thetoken = EncryptionsUtility.getheader(strulr, methodtype);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {

                ClsInputTransactionHistory objHistoryTran = new ClsInputTransactionHistory()
                {
                    FromDate = objHistory.FromDate,
                    MobileNumber = objHistory.MobileNumber,
                    ToDate = objHistory.ToDate,
                    TagID = objHistory.TagID
                };

                string serialHistoryTran = JsonConvert.SerializeObject(objHistoryTran);
                string encryptedHistoryTran = EncryptionsUtility.AES_ENCRYPT(serialHistoryTran, AESkey);
                ClsRequestData objRequestData = new ClsRequestData() { RequestData = encryptedHistoryTran };
                serialHistoryTran = JsonConvert.SerializeObject(objRequestData);

                var client = new RestClient(strulr);
                client.Timeout = -1;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", thetoken);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", serialHistoryTran, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response == null)
                {
                    BadRequest();
                }
                else
                {

                    try
                    {
                        TokenResponse json = JsonConvert.DeserializeObject<TokenResponse>(response.Content);
                        var x = json.ResponseData;// this extracts the encrypted requesttoken;

                        string s1 = EncryptionsUtility.AES_DECRYPT(x, AESkey);
                        Console.WriteLine(s1);

                        objresponse = JsonConvert.DeserializeObject<ResponseTranHistory>(s1);
                        return Ok(objresponse);
                    }
                    catch (Exception ex)
                    {

                        return BadRequest(ex.Message);
                    }

                }
                return Ok(objresponse);
            }
            //return null;
        }//method ends



         [JwtAuthentication]
        [Route("api/CustomerKYCUpload")]
        public IHttpActionResult CustomerKYCUpload([FromBody] CustomerKYCUpload ckycu)
        {
            string strulr = "https://fastag.gitechnology.in/indusindAPI/api/BC/WalletIndividual/CustomerKYCUpload";
            string methodtype = "POST";
            string thetoken = EncryptionsUtility.getheader(strulr, methodtype);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else

            {

                string clsTagInput = JsonConvert.SerializeObject(ckycu);

                string encryptedClsTagInput = EncryptionsUtility.AES_ENCRYPT(clsTagInput, AESkey);

                ClsRequestData objRequestData = new ClsRequestData() { RequestData = encryptedClsTagInput };
                string clsTagInputserial = JsonConvert.SerializeObject(objRequestData);

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var client = new RestClient(strulr);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", thetoken);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", clsTagInputserial, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (response == null)
                {
                    return NotFound();
                }
                else
                {
                    try
                    {

                        TokenResponse json = JsonConvert.DeserializeObject<TokenResponse>(response.Content);
                        var x = json.ResponseData;// this extracts the encrypted requesttoken;
                                                  //response decryption

                        string s1 = EncryptionsUtility.AES_DECRYPT(x, AESkey);
                        ClsCustRegResponse objrespone = JsonConvert.DeserializeObject<ClsCustRegResponse>(s1);
                        return Ok(objrespone);

                    }
                    catch (Exception ex)
                    {

                        return BadRequest(ex.Message);
                    }

                }


            }

        } //method ends 

    }
}


