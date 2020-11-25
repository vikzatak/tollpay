using ConsoleAppAxisToken;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace DPTPWebAPI.paymentGateway
{
    public class clsRespEaseBuzz
    {
        public int status { get; set; }
        public string data { get; set; }
    }

    //easebuzz api system starts
    public class Easebuzz
    {
        clsKeyValue ckv = EncryptionLibrary.GetKeys();
        public string easebuzz_action_url = string.Empty;
        public string gen_hash;
        public string txnid = String.Empty;
        public string easebuzz_merchant_key = string.Empty;
        public string salt = "XNP77FNNIY"; // string.Empty;
        public string Key = "EG6N9D4GUU";  //string.Empty;
        public string env = "test";


        public Easebuzz(string SALT, string KEY, string ENV)
        {
            salt = SALT;
            Key = KEY;
            env = ENV;
        }
        // this function is required to initiate payment
        public string initiatePaymentAPI(string aAmount, String Firstname, String Email, String Phone, String Productinfo, String Surl, String Furl, String Txnid)
        {
             env = ckv.easebuzzenv;
             string[] hashVarsSeq;
            string hash_string = string.Empty;
            string saltvalue = ckv.easebuzzsalt;
            string amount = aAmount;
            string firstname = Firstname;
            string email = Email;
            string phone = Phone;
            string productinfo = Productinfo;
            string surl = Surl;
            string furl = Furl;
            string udf1 = "";
            string udf2 = "";
            string udf3 = "";
            string udf4 = "";
            string udf5 = "";

            // Generate transaction ID -> make sure this is unique for all transactions
            Random rnd = new Random();
            string strHash = Easebuzz_Generatehash512(rnd.ToString() + DateTime.Now);
            //txnid = strHash.ToString().Substring(0, 20);
            txnid = Txnid;

            string paymentUrl = getURL();
            // Get configs from web config
            easebuzz_action_url = paymentUrl; // + "/pay/secure";

            // generate hash table
            System.Collections.Hashtable data = new System.Collections.Hashtable(); // adding values in gash table for data post
            data.Add("txnid", txnid);
            data.Add("key", ckv.easebuzzkey);
            string AmountForm = Convert.ToDecimal(amount.Trim()).ToString("g29");// eliminating trailing zeros
            amount = AmountForm;
            data.Add("amount", amount);
            data.Add("firstname", firstname.Trim());
            data.Add("email", email.Trim());
            data.Add("phone", phone.Trim());
            data.Add("productinfo", productinfo.Trim());
            data.Add("surl", surl.Trim());
            data.Add("furl", furl.Trim());
            data.Add("udf1", udf1.Trim());
            data.Add("udf2", udf2.Trim());
            data.Add("udf3", udf3.Trim());
            data.Add("udf4", udf4.Trim());
            data.Add("udf5", udf5.Trim());


            // generate hash
            hashVarsSeq = "key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10".Split('|'); // spliting hash sequence from config
            hash_string = "";
            foreach (string hash_var in hashVarsSeq)
            {
                hash_string = hash_string + (data.ContainsKey(hash_var) ? data[hash_var].ToString() : "");
                hash_string = hash_string + '|';
            }
            hash_string += salt;// appending SALT
            gen_hash = Easebuzz_Generatehash512(hash_string).ToLower();        //generating hash
            data.Add("hash", gen_hash);
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var client = new RestClient("https://pay.easebuzz.in/payment/initiateLink");
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            string postdata = "firstname=" + firstname + "&amount=" + amount + "&email=" + email + "&productinfo=FastagRecharge&phone=" + phone + "&surl=https://payment.tollpay.in/success.aspx&furl=https://payment.tollpay.in/success.aspx&txnid=" + txnid + "&key=" + ckv.easebuzzkey + "&udf1=&udf2=&udf3=&udf4=&udf4=&udf5=&udf6=&udf7=&udf8=&udf9=&udf10=&hash=" + gen_hash;

            request.AddParameter("application/x-www-form-urlencoded", postdata, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            clsRespEaseBuzz obj = JsonConvert.DeserializeObject<clsRespEaseBuzz>(response.Content);
            if (obj.status == 1)
            {
                DP_TPEntities db = new DP_TPEntities();
                PaymentGatewayResponse pgr = new PaymentGatewayResponse();
                pgr.firstname = firstname;
                pgr.email = email;
                pgr.CreatedDate = DateTime.Now;
                pgr.phone = phone;
                pgr.amount = Convert.ToDecimal(amount);
                pgr.txnid = txnid;
                db.PaymentGatewayResponses.Add(pgr);
                db.SaveChanges();
                string strForm = easebuzz_action_url + obj.data;                     //Easebuzz_PreparePOSTForm(easebuzz_action_url, data);
                return strForm;
            }
            else
            {
                DP_TPEntities db = new DP_TPEntities();
                PaymentGatewayResponse pgr = new PaymentGatewayResponse();
                pgr.firstname = firstname;
                pgr.email = email;
                pgr.CreatedDate = DateTime.Now;
                pgr.phone = phone;
                pgr.amount = Convert.ToDecimal(amount);
                pgr.txnid = txnid;
                db.PaymentGatewayResponses.Add(pgr);
                db.SaveChanges();
                return response.Content;
            }

        }

        //prepare a postform for redirection to payment gateway
        public string Easebuzz_PreparePOSTForm(string url, System.Collections.Hashtable data)
        {
            //Set a name for the form
            string formID = "PostForm";
            //Build the form using the specified data to be posted.
            StringBuilder strForm = new StringBuilder();
            strForm.Append("<form id=\"" + formID + "\" name=\"" +
                           formID + "\" action=\"" + url +
                           "\" method=\"POST\">");

            foreach (System.Collections.DictionaryEntry key in data)
            {

                strForm.Append("<input type=\"hidden\" name=\"" + key.Key +
                               "\" value=\"" + key.Value + "\">");
            }
            strForm.Append("</form>");
            //Build the JavaScript which will do the Posting operation.
            StringBuilder strScript = new StringBuilder();
            strScript.Append("<script language='javascript'>");
            strScript.Append("var v" + formID + " = document." +
                             formID + ";");
            strScript.Append("v" + formID + ".submit();");
            strScript.Append("</script>");
            //Return the form and the script concatenated.
            //(The order is important, Form then JavaScript)
            return strForm.ToString() + strScript.ToString();
        }

        // hashcode generation
        public string Easebuzz_Generatehash512(string text)
        {

            byte[] message = Encoding.UTF8.GetBytes(text);

            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            SHA512Managed hashString = new SHA512Managed();
            string hex = "";
            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;

        }



        //get url using env varibale
        public string getURL()
        {
            if (env == "test")
            {
                string paymentUrl = "https://testpay.easebuzz.in/pay/";
                return paymentUrl;
            }
            else
            {
                string paymentUrl = "https://pay.easebuzz.in/pay/";
                return paymentUrl;
            }
        }

        //initiate refund api 
        public string RefundAPI(string txnid, string refund_amount, string phone, string amount, string email)
        {
            System.Collections.Hashtable data = new System.Collections.Hashtable(); // adding values in gash table for data post
            data.Add("txnid", txnid.Trim());
            data.Add("refund_amount", refund_amount.Trim());
            data.Add("key", Key);
            //string AmountForm = Convert.ToDecimal(amount.Trim()).ToString("g29");// eliminating trailing zeros
            //amount = AmountForm;
            data.Add("amount", amount);
            data.Add("email", email.Trim());
            data.Add("phone", phone.Trim());
            // generate hash
            string[] hashVarsSeq = "key|txnid|amount|refund_amount|email|phone".Split('|'); // spliting hash sequence from config
            string hash_string = "";
            foreach (string hash_var in hashVarsSeq)
            {
                hash_string = hash_string + (data.ContainsKey(hash_var) ? data[hash_var].ToString() : "");
                hash_string = hash_string + '|';
            }
            hash_string += salt;// appending SALT
            Console.WriteLine(hash_string);
            gen_hash = Easebuzz_Generatehash512(hash_string).ToLower();        //generating hash
            data.Add("hash", gen_hash);

            var postData = "txnid=" + txnid;
            postData += "&refund_amount=" + refund_amount;
            postData += "&phone=" + phone;
            postData += "&key=" + Key;
            postData += "&amount=" + amount;
            postData += "&email=" + email;
            postData += "&hash=" + gen_hash;

            string url = "https://dashboard.easebuzz.in/transaction/v1/refund";

            var request = (HttpWebRequest)WebRequest.Create(url);

            var Ndata = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = Ndata.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(Ndata, 0, Ndata.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return responseString;
        }

        //initiates transaction api 
        public string transactionAPI(string txnid, string amount, string email, string phone)
        {
            System.Collections.Hashtable data = new System.Collections.Hashtable();
            data.Add("key", Key);
            data.Add("txnid", txnid);
            data.Add("amount", amount);
            data.Add("email", email);
            data.Add("phone", phone);

            // generate hash
            string[] hashVarsSeq = "key|txnid|amount|email|phone".Split('|'); // spliting hash sequence from config
            string hash_string = "";
            foreach (string hash_var in hashVarsSeq)
            {
                hash_string = hash_string + (data.ContainsKey(hash_var) ? data[hash_var].ToString() : "");
                hash_string = hash_string + '|';
            }
            hash_string += salt;// appending SALT
            Console.WriteLine(hash_string);
            gen_hash = Easebuzz_Generatehash512(hash_string).ToLower();        //generating hash
            data.Add("hash", gen_hash);

            string url = "https://dashboard.easebuzz.in/transaction/v1/retrieve";
            var request = (HttpWebRequest)WebRequest.Create(url);

            var postData = "txnid=" + txnid;
            postData += "&amount=" + amount;
            postData += "&email=" + email;
            postData += "&phone=" + phone;
            postData += "&key=" + Key;
            postData += "&hash=" + gen_hash;

            var Ndata = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = Ndata.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(Ndata, 0, Ndata.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            //Response.Write(responseString);
            //string testResponse = "take it or leave it </br>";
            return responseString;
        }

        //iniitiate transactionDateAPI api 
        public string transactionDateAPI(string merchant_email, string transaction_date)
        {
            System.Collections.Hashtable data = new System.Collections.Hashtable();
            data.Add("key", Key);
            data.Add("merchant_email", merchant_email);
            data.Add("transaction_date", transaction_date);
            // generate hash
            string[] hashVarsSeq = "key|merchant_email|transaction_date".Split('|'); // spliting hash sequence from config
            string hash_string = "";
            foreach (string hash_var in hashVarsSeq)
            {
                hash_string = hash_string + (data.ContainsKey(hash_var) ? data[hash_var].ToString() : "");
                hash_string = hash_string + '|';
            }
            hash_string += salt;// appending SALT
            gen_hash = Easebuzz_Generatehash512(hash_string).ToLower();        //generating hash
            data.Add("hash", gen_hash);

            string url = "https://dashboard.easebuzz.in/transaction/v1/retrieve/date";
            var request = (HttpWebRequest)WebRequest.Create(url);

            var postData = "merchant_key=" + Key;
            postData += "&merchant_email=" + merchant_email;
            postData += "&transaction_date=" + transaction_date;
            postData += "&hash=" + gen_hash;

            var Ndata = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = Ndata.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(Ndata, 0, Ndata.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return responseString;
        }

        //initiate payoutAPI api 
        public string payoutAPI(string merchant_email, string payout_date)
        {
            System.Collections.Hashtable data = new System.Collections.Hashtable();
            data.Add("key", Key);
            data.Add("merchant_email", merchant_email);
            data.Add("payout_date", payout_date);
            // generate hash
            string[] hashVarsSeq = "key|merchant_email|payout_date".Split('|'); // spliting hash sequence from config
            string hash_string = "";
            foreach (string hash_var in hashVarsSeq)
            {
                hash_string = hash_string + (data.ContainsKey(hash_var) ? data[hash_var].ToString() : "");
                hash_string = hash_string + '|';
            }
            hash_string += salt;// appending SALT
            gen_hash = Easebuzz_Generatehash512(hash_string).ToLower();        //generating hash
            data.Add("hash", gen_hash);

            string url = "https://dashboard.easebuzz.in/payout/v1/retrieve";
            var request = (HttpWebRequest)WebRequest.Create(url);

            var postData = "merchant_key=" + Key;
            postData += "&merchant_email=" + merchant_email;
            postData += "&payout_date=" + payout_date;
            postData += "&hash=" + gen_hash;
            var Ndata = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = Ndata.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(Ndata, 0, Ndata.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return responseString;
        }


    }


}