using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using WebApplicationTP.DAL;

namespace DPTPWebAPI.Controllers
{
    public class PasswordRest
    {

        public string UserName { get; set; }
        public string  OTP { get; set; }

        public string Password { get; set; }
    }
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class OTPController : ApiController
    {
        private DP_TPEntities db = new DP_TPEntities();
        Notifications ns = new Notifications();
        [AllowAnonymous]
        // GET: api/TollPayWallet/5
        [ResponseType(typeof(string))]
        [Route("api/OTP")]
        [HttpGet]
         public string GetOTP(string username)
        {
            string OTPStatus = " No Such User Exist";
            User u = db.Users.Where(l => l.username == username).FirstOrDefault();
            if (u != null)
            {
                int OTP= RandomPassword();
                db.Users.Where(l => l.username == username).FirstOrDefault().TempOTP = OTP;
                db.SaveChanges();
                string smsmsg;
                smsmsg = "Dear " + u.userEmail + " , Your OTP to change Password for https://TollPay.IN or App is -. -" + OTP;
                ns.sendsms("smsfrom", u.username, smsmsg);
                ns.sendemail(u.userEmail, "", "TollPay.IN OTP To Reset Password", smsmsg);
                OTPStatus = "OTP Send On Registerd -" + u.userEmail  +" & Mobile No - XXXXX"+   u.username.Substring(u.username.Length-4);
            }
            else 
            {
                OTPStatus = " No Such User Exist";

            }
            return OTPStatus;


            
        }
        [AllowAnonymous]
        // GET: api/TollPayWallet/5
        [ResponseType(typeof(string))]
        [Route("api/SetPassword")]
        [HttpPost]
        public string SetPassword(PasswordRest passreset)
        {
            string OTPStatus = " No Such User Exist Password Can't be changed";
            int otp = Convert.ToInt32(passreset.OTP);
            User u = db.Users.Where(l => l.username == passreset.UserName && l.TempOTP== otp).FirstOrDefault();
            if (u != null)
            {
                db.Users.Where(l => l.username == passreset.UserName).FirstOrDefault().password = passreset.Password;
                db.SaveChanges();
                string smsmsg;
                smsmsg = "Dear " + u.userEmail + " , Your Password for https://TollPay.IN or App is sucessfully changed";
                ns.sendsms("smsfrom", u.username, smsmsg);
                ns.sendemail(u.userEmail, "", "TollPay.IN Password Changed...", smsmsg);
                OTPStatus = "TollPay.IN Password changed Sucessfully";
            }
            else
            {
                OTPStatus = " No Such User Exist Password Can't be changed";
            }
            return OTPStatus;

        }
        public int RandomPassword()
        {
            Random random = new Random();
            return random.Next(1000, 9999);
        }
       
    }
}
