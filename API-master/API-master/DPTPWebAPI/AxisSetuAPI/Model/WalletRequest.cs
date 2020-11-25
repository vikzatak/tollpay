using DPTPWebAPI;
using DPTPWebAPI.AxisSetuAPI.Model;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplicationTP.DAL;

namespace ConsoleAppAxisToken.model
{

    public class clsWalletresponse
    {
        public Walletresponse WalletResponse { get; set; }
    }

    public class Walletresponse
    {
        public Subheader SubHeader { get; set; }
        public string WalletResponseBodyEncrypted { get; set; }
    }

   



    public class ClsEWalletrequest
    {
        public Walletrequest WalletRequest { get; set; }
    }

    public class Walletrequest
    {
        public string WalletRequestBodyEncrypted { get; set; }
        public Subheader SubHeader { get; set; }

    }








    public class clsWalletrequest
    {
        DP_TPEntities db = new DP_TPEntities();
        clsKeyValue ckv = EncryptionLibrary.GetKeys();
        Notifications ns = new Notifications();
        public Walletrequest walletRequest { get; set; }
        public string WalletRequestBodyEncrypted { get; set; }
        Bank_CreationWalletAPIResponse bcwar = new Bank_CreationWalletAPIResponse();
        public int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }
        public int createTollPayCustomer(User u)
        {
            try
            {
                User ue = db.Users.Where(i => i.username == u.username).FirstOrDefault();
                if (ue == null)
                {
                    db.Users.Add(u);
                    db.SaveChanges();
                }
                else
                {
                    ue.password = u.password;
                    ue.userEmail = u.userEmail;
                    ue.mobno1 = u.mobno1;
                    ue.mobno2 = u.mobno2;
                    ue.userRollstatus = "C";
                    db.SaveChanges();

                }
                string smsmsg;
                smsmsg = "Dear User, Use Web https://Tollpay.in | App http://bit.ly/34kCC5C  with Username=" + u.username + " & Passwrd=" + u.password;
                ns.sendsms("smsfrom", u.mobno1, smsmsg);
                ns.sendemail(u.userEmail, "", "Welcome to TollPay.IN", smsmsg);

                return db.Users.Where(i => i.username == u.username).FirstOrDefault().srno;
            }
            catch (Exception ex)
            {

                return 0;
            }
        }

        public void allocateFastTag(Bank_CreateWalletAPI cw)
        {
            try
            {
                //Create User for TollPay 
                //Update FastTage Vehicle Mapping
                //Update Distribi
                string cfasttag = cw.tagseq;

                ecom_RFID er = db.ecom_RFID.Where(p => p.ecom_RFIDTagSrNo == cfasttag).FirstOrDefault();
                User u = new User();
                u.mobno1 = cw.custmobilenumber;
                u.userEmail = cw.custemailId;
                u.address = "FN=" + cw.custfirstname + "|LN=" + cw.custlastname + "|DoB=" + cw.custdob + "|PINCode=" + cw.custpincode;
                u.password = Convert.ToString(GenerateRandomNo());
                u.status = "a";
                u.username = cw.custmobilenumber.Trim();
                er.ecom_CustomerVehicleNo = cw.vehregnumber;
                er.ecom_CustomerFName = cw.custfirstname;
                er.ecom_CustomerLName = cw.custlastname;
                er.ecom_CustomerMobNo = cw.custmobilenumber.Trim();
                er.ecom_OTP = u.password;
                er.ecom_isAllocated = true;
                er.ecom_CustomerVehicleType = cw.vehclass;
                er.ecom_CustomerIdentity = "DoB=" + cw.custdob + "|PINCode=" + cw.custpincode;
                er.ecom_RFIDTagSrNo = cw.tagseq;
                er.ecom_RFIDStatus = "a";
                er.ecom_CustomerId = createTollPayCustomer(u);
                db.SaveChanges();

                User_Vehicle vu = new User_Vehicle();
                vu.userid = er.ecom_CustomerId;
                vu.RFIDNumber = er.ecom_RFIDTagSrNo;
                vu.uservehicleRegNo = er.ecom_CustomerVehicleNo;
                vu.uservehicleType = er.ecom_CustomerVehicleType;
                User_Vehicle uve = db.User_Vehicle.Where(i => i.uservehicleRegNo == vu.uservehicleRegNo).FirstOrDefault();
                if (uve == null)
                {
                    db.User_Vehicle.Add(vu);
                    db.SaveChanges();
                }
                else
                {
                    uve.userid = er.ecom_CustomerId;
                    uve.RFIDNumber = er.ecom_RFIDTagSrNo;
                    uve.uservehicleRegNo = er.ecom_CustomerVehicleNo;
                    uve.uservehicleType = er.ecom_CustomerVehicleType;
                    db.SaveChanges();

                }




                TollPayWallet tpw = new TollPayWallet();
                tpw.UserID = Convert.ToInt32(vu.userid);
                tpw.WalletAmount = 0;
                tpw.Status = "a";
                tpw.ModifiedDate = DateTime.Now;
                TollPayWallet tpwe = db.TollPayWallets.Where(i => i.UserID == tpw.UserID).FirstOrDefault();
                if (tpwe == null)
                {
                    db.TollPayWallets.Add(tpw);
                    db.SaveChanges();
                }
                else
                {
                    tpwe.UserID = Convert.ToInt32(vu.userid);
                    tpwe.WalletAmount = Convert.ToDecimal(cw.depositamt);
                    tpwe.Status = "a";
                    tpwe.ModifiedDate = DateTime.Now;
                    db.SaveChanges();

                }
            }
            catch (Exception ex)
            {

               
            }
      
           
        }
        public Bank_CreationWalletAPIResponse  MethodclsWalletrequest(Bank_CreateWalletAPI cw)
        {
            string vguid = ckv.SessionID;
            //Same few fix values 
            cw.custpincode = "411042";
            cw.productId = "5";
           // cw.sessionId = ckv.SessionID;
            

            try
            {
                var client = new RestClient(ckv.AxisSetuAPIURL + "/api/gateway/api/v1/fastag/wallet-vehicle-creation");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("X-IBM-Client-Id", ckv.XIBMClientId);
                request.AddHeader("X-IBM-Client-Secret", ckv.XIBMClientSecret);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", ckv.JWT);

                db.Bank_CreateWalletAPI.Add(cw);
                db.SaveChanges();

                Subheader sub = new Subheader()
                {
                    channelId = "DIGI",
                    requestUUID = vguid,
                    serviceRequestVersion = "1.0",
                    serviceRequestId = "WALLET"
                };
                var jsonString = JsonConvert.SerializeObject(cw);

                string WalletRequestBodyEncrypted = EncryptionLibrary.encrypt(jsonString, ckv.Key);

                ClsEWalletrequest cewr = new ClsEWalletrequest();
                cewr.WalletRequest = new Walletrequest();
                cewr.WalletRequest.SubHeader = new Subheader();
                cewr.WalletRequest.SubHeader = sub;
                cewr.WalletRequest.WalletRequestBodyEncrypted = WalletRequestBodyEncrypted;
                var ejsonString = JsonConvert.SerializeObject(cewr);
                request.AddParameter("application/json", ejsonString.ToString(), ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                APIException apierror = JsonConvert.DeserializeObject<APIException>(response.Content);
                clsWalletresponse json = JsonConvert.DeserializeObject<clsWalletresponse>(response.Content);
                if (apierror.httpCode != 200 && apierror.httpCode != 0) //503
                {
                    WalletRequestBodyEncrypted = JsonConvert.SerializeObject(apierror);
                    bcwar.statuscode = apierror.errorCode;
                    bcwar.statusmessage = apierror.moreInformation + " | " + apierror.httpMessage;
                }
                else if (response.Content != string.Empty)
                {
                    WalletRequestBodyEncrypted = EncryptionLibrary.decrypt(json.WalletResponse.WalletResponseBodyEncrypted, ckv.Key);
                    bcwar = JsonConvert.DeserializeObject<Bank_CreationWalletAPIResponse>(WalletRequestBodyEncrypted);
                   if(cw.tagdispatch=="1")
                    {
                        allocateFastTag(cw);                      //You need to set tagdispatch to "1" and set tagseq to the tag sequence number. To test this, you need to get pre provisioned tag numbers from Axis bank.
                    }
                  
                    db.Bank_CreationWalletAPIResponse.Add(bcwar);
                    db.SaveChanges();
                }
                else
                {//EncryptionLibrary.DecryptText(response.Content, akey);
                    WalletRequestBodyEncrypted = "Error";
                }

                db.AxisSetuAPILoggers.Add(new AxisSetuAPILogger()
                {
                    RequestGuid = ckv.SessionID,
                    Currenttime=DateTime.Now,
                    subheaderMethod=json.WalletResponse.SubHeader.serviceRequestId,
                    request = jsonString.ToString(),
                    requestEncrypted = ejsonString.ToString(),
                    responseEncrypted = response.Content,
                    response = WalletRequestBodyEncrypted
                });
                db.SaveChanges();

                return bcwar;
            }
            catch (Exception ex)
            {

                bcwar.statusmessage = ex.Message;
                return bcwar;
            }

           
        }
    }







}
