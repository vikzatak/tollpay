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
        [Route("api/ForgotPassword")]
        [HttpGet]
         public clsstatuscode ForgotPassword(string username)
        {
            
            User u = db.Users.Where(l => l.username == username).FirstOrDefault();
            if (u != null)
            {
                int OTP= RandomPassword();
                db.Users.Where(l => l.username == username).FirstOrDefault().TempOTP = OTP;
                db.SaveChanges();
                string smsmsg;
                smsmsg = "Dear " + u.userEmail + " , Your OTP to change Password for https://TollPay.IN is -. -" + OTP;
                ns.sendsms("smsfrom", u.username, smsmsg);
                // ns.sendemail(u.userEmail, "", "TollPay.IN OTP To Reset Password", smsmsg);
                clsstatuscode cs = new clsstatuscode();
                cs.statuscode = 000;
                cs.statusdesc= "OTP Send to Registered Mobile No - XXXXX"+   u.username.Substring(u.username.Length-4);
                return cs;
            }
            else 
            {
                clslststatuscode css = new clslststatuscode();
                clsstatuscode scd = css.getstatuscodes().Where(s => s.statuscode == 020).FirstOrDefault();

                return scd;


            }
         


            
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
                u.password = passreset.Password;
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
