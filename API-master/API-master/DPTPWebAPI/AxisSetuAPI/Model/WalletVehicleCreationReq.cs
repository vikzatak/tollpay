
using DPTPWebAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppAxisToken.model
{
     
    public class clsWalletrequest
    {
        DP_TPEntities db = new DP_TPEntities();
        public Walletrequest WalletRequest { get; set; }
        public clsWalletrequest()
        {
            string key = "8E41AEF6156CF2221EF6A2EA6950A934";
            string vguid = Guid.NewGuid().ToString();   //DateTime.Now.ToString("yyyyMMddHHmmssfffffff");
            Walletrequest wr = new Walletrequest();
            Subheader sub = new Subheader()
            {
                serviceRequestVersion = "1.0",
                serviceRequestId = "WALLET",
                requestUUID = vguid,
                channelId = "DIGI"
            };
            wr.SubHeader = sub;
            Walletrequestbody wrb = new Walletrequestbody();
            var jsonString = JsonConvert.SerializeObject(wrb);
            Console.WriteLine("Walletrequestbody plan -" + jsonString.ToString());

            wr.WalletRequestBodyEncrypted = EncryptionLibrary.encrypt(jsonString, key);
            this.WalletRequest = wr;
            db.AxisSetuAPILoggers.Add(new AxisSetuAPILogger()
            {
                RequestGuid = vguid
               ,
                request = jsonString
            });
            db.SaveChanges();

        }
    }

    public class Walletrequest
    {
        public string WalletRequestBodyEncrypted { get; set; }
        public Subheader SubHeader { get; set; }

    }



    public class WalletVehicleCreationReq
    {
        public Walletrequest WalletRequest { get; set; }
        public WalletVehicleCreationReq()
        {

        }

    }





    public class Walletrequestbody
    {
        public Walletrequestbody()
        {
            string vguid = DateTime.Now.ToString("yyyyMMddHHmmssfffffff"); 
            custemailId = "digipment@gmail.com";
            vehmobilenumber = string.Empty;
            chasisnumber = "MA1NA2SMXG8E39451";
            issuanceamt = "100";
            custfirstname = "Yuvraj";
            regstate = "maharastra";
            paymentstatus = "1";
            custaddress = "A P Urawade  Tal Mulshi Dist  Pune";
            rccopynumber = "2185787";
            walletId = string.Empty;
            custpincode = "412111";
            productId = "5";
            custcity = "Pune";
            vehdriveremail = "sachinsnimbalkar@gmail.com";
            depositamt = "200";
            custtype = "1";
            custlastname = "Shelar";
            field1 = string.Empty;
            field2 = string.Empty;
            field3 = string.Empty;
            field4 = string.Empty;
            field5 = string.Empty;
            field6 = string.Empty;
            field7 = string.Empty;
            field8 = string.Empty;
            field9 = string.Empty;
            field10 = string.Empty;
            commercialtype = "0";
            custdob = "19/07/1989";
            vehclass = "4";
            agentId = "2";
            posnumber = "65";
            paymentrefnumber = "abcde";
            paymentremarks = string.Empty;
            taxtamt = "10.00";
            custstate = "maharastra";
            vehdrivername = "Sachin Nimbalkar";
            operationId = "1";
            vehregnumber = "MH12NJ1772";
            regexpiry = string.Empty;
            channelId = "6";
            cifId = string.Empty;
            tagdispatch = "1";
            accountnumber = "";
            enginenumber = "SMG8E24883";
            custmobilenumber = "9623121584";
            documenttype = "105";
            vehweight = "2225";
            sessionId = Guid.NewGuid().ToString();// vguid;
            ifsccode = string.Empty;
            cubiccapacity = string.Empty;
            regdate = string.Empty;
            custgender = "male";
            documentId = "A1234";
            bankname = string.Empty;
            tagseq = "10599829";
            paymenttype = "3";
         }

        
            public string custemailId { get; set; }
            public string vehmobilenumber { get; set; }
            public string chasisnumber { get; set; }
            public string issuanceamt { get; set; }
            public string custfirstname { get; set; }
            public string regstate { get; set; }
            public string paymentstatus { get; set; }
            public string custaddress { get; set; }
            public string rccopynumber { get; set; }
            public string walletId { get; set; }
            public string custpincode { get; set; }
            public string productId { get; set; }
            public string custcity { get; set; }
            public string vehdriveremail { get; set; }
            public string depositamt { get; set; }
            public string custtype { get; set; }
            public string custlastname { get; set; }
            public string field10 { get; set; }
            public string commercialtype { get; set; }
            public string custdob { get; set; }
            public string vehclass { get; set; }
            public string agentId { get; set; }
            public string posnumber { get; set; }
            public string paymentrefnumber { get; set; }
            public string paymentremarks { get; set; }
            public string taxtamt { get; set; }
            public string custstate { get; set; }
            public string vehdrivername { get; set; }
            public string operationId { get; set; }
            public string vehregnumber { get; set; }
            public string regexpiry { get; set; }
            public string channelId { get; set; }
            public string cifId { get; set; }
            public string tagdispatch { get; set; }
            public string accountnumber { get; set; }
            public string enginenumber { get; set; }
            public string custmobilenumber { get; set; }
            public string documenttype { get; set; }
            public string vehweight { get; set; }
            public string sessionId { get; set; }
            public string ifsccode { get; set; }
            public string field1 { get; set; }
            public string cubiccapacity { get; set; }
            public string regdate { get; set; }
            public string field7 { get; set; }
            public string custgender { get; set; }
            public string field6 { get; set; }
            public string documentId { get; set; }
            public string field9 { get; set; }
            public string bankname { get; set; }
            public string field8 { get; set; }
            public string tagseq { get; set; }
            public string field3 { get; set; }
            public string field2 { get; set; }
            public string paymenttype { get; set; }
            public string field5 { get; set; }
            public string field4 { get; set; }
        


    }


}
