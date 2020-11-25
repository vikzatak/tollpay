using DPTPWebAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;


namespace WebApplicationTP.DAL
{
    public class Notifications
    {

        public bool sendemail(string eto, string ecc, string esubject, string ebody)
        {
            try
            {
                DALSingletonAppSetting d = DALSingletonAppSetting.GetInstance;
                List<AppKey> ak = d.GetAppKey();
                string smtpusername = ak.Where(k => k.vKey == "smtpusername").FirstOrDefault().vvalue;
                string smtpport = ak.Where(k => k.vKey == "smtpport").FirstOrDefault().vvalue;
                string smtphost = ak.Where(k => k.vKey == "smtphost").FirstOrDefault().vvalue;
                string smtpassword = ak.Where(k => k.vKey == "smtppassword").FirstOrDefault().vvalue;


                MailAddress to = new MailAddress(eto);

                MailAddress from = new MailAddress(smtpusername);
                MailMessage mail = new MailMessage(from, to);

                mail.Subject = smtpusername;
                mail.Body = ebody;


                SmtpClient smtp = new SmtpClient();
                smtp.Host = smtphost;
                smtp.Port = Convert.ToInt32(smtpport);


                smtp.Credentials = new NetworkCredential(smtpusername, smtpassword);
                smtp.EnableSsl = true;
                smtp.Send(mail);

                return true;

            }
            catch (Exception)
            {
                return false;

            }

        }

        public bool sendsms(string smsfrom, string smsto, string smsmsg)
        {
            try
            {
                string smsgateway = "http://sms.hspsms.com";

                if (smsgateway == "http://sms.hspsms.com")
                {
                    WebClient wb = new WebClient();
                    string strurl = "http://sms.hspsms.com/sendSMS?username=tollpay&message=" + smsmsg + "&sendername=TolPay&smstype=TRANS&numbers=" + smsto + "&apikey=0f2d332e-2c6d-432d-9cc0-c59ecc830425";
                    string result = wb.DownloadString(strurl);
                }

                if (smsgateway == "TwilioClient")
                {

                }
                return true;
            }
            catch (Exception)
            {

                return false;

            }

        }

    }
}