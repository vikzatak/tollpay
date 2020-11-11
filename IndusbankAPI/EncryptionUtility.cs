using DPTPWebAPI;
using Google.GData.Client;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using HttpUtility = Google.GData.Client.HttpUtility;

namespace IndusAPIMiddleWareAPI.Helperclasses
{

    public class CustomerKYCUpload
    {
        public string Customerid { get; set; }
        public string IDProofType { get; set; }
        public string IDProofNumber { get; set; }
        public string IDProofImage { get; set; }
        public string AddressProofType { get; set; }
        public string AddressProofNumber { get; set; }
        public string AddressproofImage { get; set; }
        public string ProfileImage { get; set; }
    }

    public class TagStatusResponse
    {
        public string StatusCode { get; set; }
        public string Status { get; set; }
        public string TagStatus { get; set; }
        public string VEHICLENUMBER { get; set; }
        public string TID { get; set; }
        public string TagID { get; set; }
        public string VehicleClassID { get; set; }
        public string SerialNo { get; set; }
    }

    public class TagStatus
    {
        public string Type { get; set; }
        public string InputVal { get; set; }
    }

    public class TagReplacement
    {
        public string ReplaceSerialNo { get; set; }
        public string TagAccountNo { get; set; }
        public string Reason { get; set; }
        public string TagCost { get; set; }
        public string TransactionID { get; set; }
    }

    public class CustomerRegistrationNonIndividual
    {
        public string TransactionID { get; set; }
        public string Name { get; set; }
        public string EmailId { get; set; }
        public string ContactNumber { get; set; }
        public string ContactPerson { get; set; }
        public string RegionId { get; set; }
        public string StateId { get; set; }
        public string CityId { get; set; }
        public string Address { get; set; }
        public string PinCode { get; set; }
        public string PAN { get; set; }
        public string Entity { get; set; }
        public string GST { get; set; }
        public string CIN { get; set; }
        public string AddressProofID { get; set; }
        public string AddressProofImage { get; set; }

        public string KycProofID { get; set; }
        public string KycProofImage { get; set; }
    }

    public class DistributorToSalesPersonTags
    {
        public string TagNo { get; set; }
        public string DistributorUserID { get; set; }
        public string SalesUserID { get; set; }
    }
    public class TagStackStatus
    {
        public string TagNo { get; set; }
        public string TagSrNo { get; set; }

        public string salesstatus { get; set; }

        public string CustomerWallet { get; set; }

        public string CustomerVRN { get; set; }

        public string Vehicle_Type { get; set; }

        public DateTime LastUpdated { get; set; }
    }
    public class DistributorSalesPersons
    {
        public string UserID { get; set; }
        public string MobNo { get; set; }
        public string Name { get; set; }

        public string Branch { get; set; }

        public string Role { get; set; }

        public List<TagStackStatus> tagstockstatus { get; set; }


    }


    public class clsRespFindWalletInfo
    {
        public string StatusCode { get; set; }
        public string Status { get; set; }
        public Walletinfodetail[] WalletInfoDetails { get; set; }
        public Taginfo[] TagInfo { get; set; }
    }

    public class Walletinfodetail
    {
        public string CustomerID { get; set; }
        public string Name { get; set; }
        public string WalletBalance { get; set; }
        public string MobileNo { get; set; }
        public string KycStatus { get; set; }
        public string CustomerType { get; set; }
        public string PANNO { get; set; }
        public string AddharNO { get; set; }
        public string CBSAccountNumber { get; set; }
    }

    public class Taginfo
    {
        public string TagAccountNo { get; set; }
        public string VRN { get; set; }
        public string TagSerialNo { get; set; }
        public string Status { get; set; }
        public string MinAmount { get; set; }
        public string TagBalance { get; set; }
        public string RechargeAmount { get; set; }
        public string SerialNo { get; set; }
        public string StatusCode { get; set; }
        public string SecurityDeposit { get; set; }
        public string JoiningFee { get; set; }
        public string ClosingRequestRemark { get; set; }
        public string TransLimit { get; set; }
        public string TransLimitStatus { get; set; }
        public string ConsumedLimit { get; set; }
        public DateTime  IssuanceDate { get; set; }

    }

    public class ebusbuzzpaymentResponseIndusWalletRecharge
    {
        
        public PaymentGatewayResponse paymentGatewayResponse { get; set; }
        public ClsRechargeWallet objRechargeWallet { get; set; }
    }
    public class clsPayGateWayWalletRecharge
    {
        public PaymentGatewayResponse paymentGatewayResponse { get; set; }
        public ResponseWalletRecharge objRespWalletResponse { get; set; }
    }
    public class TokenResponse
    {
        public string ResponseData { get; set; }
    }
    #region StatusCode
    public class clsStatuscode
    {
        public string StatusCode { get; set; }
        public string Status { get; set; }
        public object[] RegionDetails { get; set; }
    }
    #endregion

    //Customer Registration
    #region GetRegion

    public class clsRegiondetail
    {
        public string StatusCode { get; set; }
        public string Status { get; set; }
        public Regiondetail[] RegionDetails { get; set; }
    }

    public class Regiondetail
    {
        public int RegionID { get; set; }
        public string RegionName { get; set; }
    }
    #endregion

    #region GetState

    public class clsInGetState
    {
        public string RegionID { get; set; }
    }


    public class clsOutGetState
    {
        public string StatusCode { get; set; }
        public string Status { get; set; }
        public Statedetail[] StateDetails { get; set; }
    }

    public class Statedetail
    {
        public string StateID { get; set; }
        public string StateName { get; set; }
    }
    #endregion


    #region forGetCity

    public class clsInGetCity
    {
        public string RegionID { get; set; }
        public string StateID { get; set; }
    }
    //created by SP
    public class ClsCitytOutDetails
    {
        public string StatusCode { get; set; }
        public string Status { get; set; }
        public CityDetails[] CityDetails { get; set; }
    }
    ////created by SP
    public class CityDetails
    {
        public string CityId { get; set; }
        public string CityName { get; set; }
    }
    #endregion


    #region GetIDType

    public class clsGetIDType
    {
        public string StatusCode { get; set; }
        public string Status { get; set; }
        public Idprooftypedetail[] IDProofTypeDetails { get; set; }
    }

    public class Idprooftypedetail
    {
        public int TypeID { get; set; }
        public string TypeValue { get; set; }
    }

    #endregion

    #region WalletCustomerRegistration
    public class clsWalletCustomerRegistration
    {
        public string TransactionID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string EmailID { get; set; }
        public string DOB { get; set; }
        public string MobileNumber { get; set; }
        public string MotherName { get; set; }
        public string Address { get; set; }
        public string RegionID { get; set; }
        public string StateID { get; set; }
        public string CityID { get; set; }
        public string Pincode { get; set; }
        public string GST { get; set; }
        public string Aadhaarno { get; set; }
        public string PANNo { get; set; }


    }


    public class ClsCustRegResponse
    {
        public string StatusCode { get; set; }

        public string Status { get; set; }

    }
    #endregion

    #region WalletCustomerRegistrationResendOTP
    public class clsWalletCustomerRegistrationResendOTP
    {
        public string TransactionID { get; set; }
        public string MobileNumber { get; set; }
    }
    #endregion

    #region WalletCustomerRegistrationOTPValidation

    public class clsWalletCustomerRegistrationOTPValidation
    {
        public string TransactionID { get; set; }
        public string MobileNumber { get; set; }
        public string OTP { get; set; }
    }

    #endregion

    #region TagRegistration




    //created by SP
    public class ClsVehicleClass
    {
        public string StatusCode { get; set; }
        public string Status { get; set; }
        public VehicleFindingdetails[] VehicleClassDetails { get; set; }
    }

    ////created by SP
    public class VehicleFindingdetails
    {
        public string VehicleID { get; set; }
        public string VehicleClassId { get; set; }
        public string VehicleType { get; set; }
        public string Amount { get; set; }
        public string SecurityDeposit { get; set; }
        public string CardCost { get; set; }

    }



    public class OutTagRegistration
    {
        public int ResponseCode { get; set; }
        public string PGTrackID { get; set; }
        public string Amount { get; set; }
        public string CustomerID { get; set; }
    }


    public class clsInputRequeryTagReg
    {
        public string PGTrackId { get; set; }

    }

    public class ClsOutTagRequeryTag
    {
        public string ResponseCode { get; set; }

        public TranDetails[] OrderInfo { get; set; }

    }


    public class TranDetails
    {
        public string TransactionId { get; set; }
        public string CustomerId { get; set; }

        public string ServiceTax { get; set; }

        public string ServiceTaxAmount { get; set; }

        public string ShippingTax { get; set; }

        public string ShippingTaxAmount { get; set; }

        public string Amount { get; set; }
        public string TotalAmount { get; set; }

        public string Status { get; set; }


    }
    #endregion

    #region WalletInformation

    public class clsWalletInformation
    {
        //public int ResponseCode { get; set; }
        //public Walletinfo WalletInfo { get; set; }
        //public Taginfo[] TagInfo { get; set; }
        public int STATUSCODE { get; set; }
        public TheWalletinfo WalletInfoDetails { get; set; }
        public TaginfoDetails[] TagInfo { get; set; }
    }

    public class Walletinfo
    {
        public string CustomerID { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string MobileNumber { get; set; }
        public string EmailID { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string RegionID { get; set; }
        public string StateID { get; set; }
        public string CityID { get; set; }
        public string Pincode { get; set; }
        public string AadhaarNo { get; set; }
        public string PANNo { get; set; }
        public string WalletCreatedDate { get; set; }
        public string WalletBalance { get; set; }
        public string CBSAccountNumber { get; set; }
    }

    //public class Taginfo
    //{
    //    public string TagAccountNo { get; set; }
    //    public string VRN { get; set; }
    //    public string TagSerialNo { get; set; }
    //    public int TagStatusCode { get; set; }
    //    public string TagStatus { get; set; }
    //    public string SerialNo { get; set; }
    //    public int MinAmount { get; set; }
    //    public int SecurityDeposit { get; set; }
    //    public int JoiningFee { get; set; }
    //    public int TagBalance { get; set; }
    //    public int RechargeAmount { get; set; }
    //    public object ClosingRequestRemark { get; set; }
    //    public int TransLimit { get; set; }
    //    public int TransLimitStatus { get; set; }
    //    public int ConsumedLimit { get; set; }
    //}
    #endregion

    public class RechargeTagFromWallet
    {
        public string TransactionID { get; set; }
        public string MobileNumber { get; set; }
        public string TagAccountNo { get; set; }
        public string Amount { get; set; }
    }

    #region RechargeTagFromWallet




    #endregion

    #region WalletRecharge

    public class clsWalletRecharge
    {
        public string TransactionID { get; set; }
        public int PaymentType { get; set; }
        public int CustomerID { get; set; }
        public string TagAccountNo { get; set; }
        public string MobileNumber { get; set; }
        public float Amount { get; set; }
        public float RechargePercentage { get; set; }
        public float TotalAmount { get; set; }
    }


    #endregion

    #region MovetoWalletFromTag
    public class clsMovetoWalletFromTag
    {
        public string TransactionID { get; set; }
        public string MobileNumber { get; set; }
        public string TagAccountNo { get; set; }
        public int Amount { get; set; }
    }
    #endregion

    public class clsDistCustomerTagTranHistorySummary
    {
        public decimal TotalAllCustomerCredit { get; set; }
        public decimal TotalAllCustomerDebit { get; set; }
        public List<CustomerVehicle> CustomerVehicleWiseDebit { get; set; }
  

    }
    public class CustomerVehicle
    {
        public decimal TotalAllCustomerCredit { get; set; }
        public decimal TotalAllCustomerDebit { get; set; }
        public string CustomerMobileNo { get; set; }
        public string CustomerVehicleNo { get; set; }

    }

    public class clsDistCustomerTagTranHistory
    {
        public string FromDate { get; set; }
        public int DistributorId { get; set; }
        public string ToDate { get; set; }
    }
        #region TransactionHistory
        public class clsTransactionHistory
    {
        public string FromDate { get; set; }
        public string MobileNumber { get; set; }
        public string ToDate { get; set; }
        public string TagID { get; set; }
    }


    public class clsReportTransactionHistory
    {
        public Class1[] Property1 { get; set; }
    }

    public class Class1
    {
        public string TransactionID { get; set; }
        public int Amount { get; set; }
        public string TransactionDate { get; set; }
        public object Paymentthrough { get; set; }
        public string TransactionType { get; set; }
        public string TransactionStatus { get; set; }
        public string VehicleNo { get; set; }
        public string TagID { get; set; }

    }

    #endregion

    //created by SP
    #region tagreg
    public class ClsInputWalletInfo
    {
        public string MobileNumber { get; set; }
    }

    public class TheWalletinfo
    {
        public string CUSTOMERID { get; set; }
        public string Name { get; set; }
        public string WalletBalance { get; set; }
        public string MobileNo { get; set; }
        public string CBSAccountNumber { get; set; }
    }

    public class TaginfoDetails
    {
        public string TagAccountNo { get; set; }
        public string VRN { get; set; }
        public string TagSerialNo { get; set; }
        public int STATUS { get; set; }
        public string MinAmount { get; set; }
        public string TagBalance { get; set; }
        public int RechargeAmount { get; set; }
        public int SerialNo { get; set; }
        public int StatusCode { get; set; }
        public int SecurityDeposit { get; set; }
        public int JoiningFee { get; set; }
        public object ClosingRequestRemark { get; set; }
        public int TransLimit { get; set; }
        public int TransLimitStatus { get; set; }
        public int ConsumedLimit { get; set; }
    }

    #endregion

    #region walletrecharge
    public class ClsRechargeWallet
    {
        public string TransactionID { get; set; }
        public string PaymentType { get; set; }
        public string CustomerID { get; set; }
        public string TagAccountNo { get; set; }
        public string MobileNumber { get; set; }
        public string Amount { get; set; }
        public string RechargePercentage { get; set; }
        public string TotalAmount { get; set; }



    }



    public class clsResponseTagFromWallet
    {
        public int ResponseCode { get; set; }
        public Responsecontent ResponseMessage { get; set; }
    }
    #endregion

    #region EncryptionUtility

    public class clsTokenResponse
    {
        public int ResponseCode { get; set; }
        public Responsecontent ResponseContent { get; set; }
    }


    public class ClsRequestData
    {
        public string RequestData { get; set; }
    }

    public class Responsecontent
    {
        public string Token { get; set; }
        public string TokenSecret { get; set; }
    }

    public class EncryptionsUtility
    {
        static string Consumerkey = "DigipmentDigital";
        static string ConsumerSecret = "K0RhZGl1c0haa2NJbmpQRENjNXRZb0g1TSs2bEVIL1I1";
        static string AESkey = "8468A97B4373415365537265744B6579746869834973215365637261745B6579";
        //created by SP
        public async Task<string> GetDecToken()
        {

            return await GetTokenFromIndus();

            // return obj.ResponseContent.Token;
        }

        private async Task<string> GetTokenFromIndus()
        {

            DP_TPEntities db = new DP_TPEntities();
            string uri = "https://fastag.gitechnology.in/indusindAPI/api/Authentication/RequestToken";
            var client = new RestClient(uri);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            string nonce = OAuthBase.GenerateNonce();
            string timeStamp = OAuthBase.GenerateTimeStamp();
            string sig = OAuthBase.GenerateSignature(new Uri(uri), Consumerkey, ConsumerSecret, null, null, "GET", timeStamp, nonce, OAuthBase.HMACSHA1SignatureType);
            sig = HttpUtility.UrlEncode(sig);
            string authString = String.Format(@"OAuth oauth_consumer_key=""{0}"",oauth_signature_method=""HMAC-SHA1"",oauth_timestamp=""{1}"",oauth_nonce=""{2}"",oauth_version=""1.0"",oauth_signature=""{3}""""", Consumerkey, timeStamp, nonce, sig);
            request.AddHeader("Authorization", authString);
            request.AddHeader("Connection", "Keep-Alive");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            TokenResponse json = JsonConvert.DeserializeObject<TokenResponse>(response.Content);
            var x = json.ResponseData;// this extracts the encrypted requesttoken;
            string s1 = EncryptionsUtility.AES_DECRYPT(x, AESkey);
            clsTokenResponse obj = JsonConvert.DeserializeObject<clsTokenResponse>(s1);
            Console.WriteLine(obj.ResponseContent.Token);
            db.IndusInd_Token_Secrete.Add(new IndusInd_Token_Secrete() { CreateDate = DateTime.Now, Token = obj.ResponseContent.Token, TokenSecrete = obj.ResponseContent.TokenSecret });
            db.SaveChanges();
            return s1;
        }
        public static string getheader(string strurl, string methodtype)
        {
            string nonce = OAuthBase.GenerateNonce();
            string timeStamp = OAuthBase.GenerateTimeStamp();

            DP_TPEntities db = new DP_TPEntities();
            IndusInd_Token_Secrete lastestts = db.IndusInd_Token_Secrete.ToList().LastOrDefault();

            string sig = OAuthBase.GenerateSignature(new Uri(strurl), Consumerkey, ConsumerSecret, lastestts.Token, lastestts.TokenSecrete, methodtype, timeStamp, nonce, OAuthBase.HMACSHA1SignatureType);
            sig = HttpUtility.UrlEncode(sig);
            string authString2 = String.Format(@"OAuth oauth_consumer_key=""{0}"",oauth_nonce=""{1}"",oauth_signature=""{2}"",oauth_signature_method=""HMAC-SHA1"",oauth_timestamp=""{3}"",oauth_token=""{4}"",oauth_version=""1.0""", Consumerkey, nonce, sig, timeStamp, lastestts.Token);


            return authString2;
        }
        //SN method
        public static string GetToken()
        {

            var client = new RestClient("https://fastag.gitechnology.in/indusindAPI/api/Authentication/RequestToken");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization",
                "OAuth oauth_consumer_key=\"DigipmentDigital\"," +
                "oauth_signature_method=\"HMAC-SHA1\"," +
                "oauth_timestamp=\"1592817270\"," +
                "oauth_nonce=\"S2ufuB6Uh8i\",oauth_version=\"1.0\",oauth_signature=\"rscaZ%2FKArm3IZKHcSft4spwWrGw%3D\"");
            IRestResponse response = client.Execute(request);
            TokenResponse json = JsonConvert.DeserializeObject<TokenResponse>(response.Content);
            Console.WriteLine(json);

            return json.ResponseData;

        }
        public static string AES_ENCRYPT(string text, string hexPassword)
        {
            var result = string.Join("", text.Select(c => ((int)c).ToString("X2")));
            byte[] originalBytes = HexStringToByte(result);
            byte[] passwordBytes = HexStringToByte(hexPassword);
            byte[] encryptedBytes = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.Padding = PaddingMode.Zeros;
                    AES.Key = passwordBytes;
                    AES.Mode = CipherMode.ECB;

                    using (CryptoStream cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(originalBytes, 0, originalBytes.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }

            }
            return BitConverter.ToString(encryptedBytes).Replace("-", "");
        }


        public static string AES_DECRYPT(string text, string hexPassword)
        {
            byte[] originalBytes = HexStringToByte(text);
            byte[] passwordBytes = HexStringToByte(hexPassword);
            byte[] encryptedBytes = null;

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.Padding = PaddingMode.Zeros;
                    AES.Key = passwordBytes;
                    AES.Mode = CipherMode.ECB;

                    using (CryptoStream cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(originalBytes, 0, originalBytes.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }
            string input = ASCIIEncoding.UTF8.GetString(encryptedBytes).Trim();
            int index = input.LastIndexOf("}");
            if (index > 0)
            {
                input = input.Substring(0, index); // or index + 1 to keep slash
                input = input + "}";
            }
            return input;
        }

        private static byte[] HexStringToByte(string hexString)
        {
            try
            {
                int bytesCount = (hexString.Length) / 2;
                byte[] bytes = new byte[bytesCount];
                for (int x = 0; x < bytesCount; ++x)
                {
                    bytes[x] = Convert.ToByte(hexString.Substring(x * 2, 2), 16);
                }
                return bytes;
            }
            catch
            {
                throw;
            }
        }
        private static string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }


    }
    #endregion

    public class IndusEncryptionUtility
    {
        public string text { get; set; }
        public string hexPassword { get; set; }

    }

    #region APICatlog
    public class IndusBankBCAPI
    {
        public string Name { get; set; }
        public string APIRoute { get; set; }

    }


    public class APIList
    {

        public List<IndusBankBCAPI> APINameList { get; set; }

        public APIList()
        {
            APINameList.Add(new IndusBankBCAPI() { Name = "GetRegion", APIRoute = "api/BC/LookUp/GetRegion" });
            APINameList.Add(new IndusBankBCAPI() { Name = "GetState", APIRoute = "api/BC/LookUp/GetState" });
            APINameList.Add(new IndusBankBCAPI() { Name = "GetIDType", APIRoute = "api/BC/LookUp/GetProofType" });
            APINameList.Add(new IndusBankBCAPI() { Name = "WalletCustomerRegistration", APIRoute = "/api/BC/WalletIndividual/CustomerRegistration" });
            APINameList.Add(new IndusBankBCAPI() { Name = "WalletCustomerRegistrationResendOTP", APIRoute = "/api/BC/WalletIndividual/CustomerRegistration/OTPResend" });
            APINameList.Add(new IndusBankBCAPI() { Name = "WalletCustomerRegistrationOTPValidation", APIRoute = "api/BC/WalletIndividual/CustomerRegistration/OTPValidation" });
            APINameList.Add(new IndusBankBCAPI() { Name = "Vehicleclass", APIRoute = "api/BC/Tag/get_vehicleclass" });
            APINameList.Add(new IndusBankBCAPI() { Name = "TagRegistration", APIRoute = "api/BC/Tag/tag_registration" });
            APINameList.Add(new IndusBankBCAPI() { Name = "RequeryTagRegistration", APIRoute = "api/BC/Tag/requery_tag_registration" });
            APINameList.Add(new IndusBankBCAPI() { Name = "GetWalletInformation", APIRoute = "api/BC/Tag/getwalletinfo" });
            APINameList.Add(new IndusBankBCAPI() { Name = "WalletRecharge", APIRoute = "api/BC/Tag/recharge_wallet" });
            APINameList.Add(new IndusBankBCAPI() { Name = "RechargeTagFromWallet", APIRoute = "api/BC/Tag/recharge_tag_from_wallet" });
            APINameList.Add(new IndusBankBCAPI() { Name = "MovetoWalletFromTag", APIRoute = "api/BC/Tag/move_to_wallet_from_tag" });
            APINameList.Add(new IndusBankBCAPI() { Name = "TransactionHistory", APIRoute = "api/BC/Tag/GetTransactionHistory" });
            APINameList.Add(new IndusBankBCAPI() { Name = "MovetoWalletFromTag", APIRoute = "api/BC/Tag/move_to_wallet_from_tag" });
            APINameList.Add(new IndusBankBCAPI() { Name = "MovetoWalletFromTag", APIRoute = "api/BC/Tag/move_to_wallet_from_tag" });
            APINameList.Add(new IndusBankBCAPI() { Name = "MovetoWalletFromTag", APIRoute = "api/BC/Tag/move_to_wallet_from_tag" });
            APINameList.Add(new IndusBankBCAPI() { Name = "MovetoWalletFromTag", APIRoute = "api/BC/Tag/move_to_wallet_from_tag" });

        }
    }


    #endregion






}