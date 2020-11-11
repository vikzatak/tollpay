using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DPTPWebAPI.Controllers
{
    public class Ima
    {
        public string userid { get; set; }
        public string base64image { get; set; }

        public string Imagetype { get; set; }
    }

    public class ImaRC
    {
        public string vehicleRN { get; set; }
        public string imageside { get; set; }
        public string base64image { get; set; }
    }
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserimageController : ApiController
    {
         DP_TPEntities db = new DP_TPEntities();
        
       // [JwtAuthentication]
         [JwtAuthentication]
        [Route("api/Userimage/PostUserimage")]
        [HttpPost]
        public HttpResponseMessage PostUserimage([FromBody] Ima a)//or (Ima a) ,it works too
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError, "value");
            try
            {
               

              var bytes = Convert.FromBase64String(a.base64image);// a.base64image 
            //or full path to file in temp location
            //var filePath = Path.GetTempFileName();

            // full path to file in current project location
            string filedir = "C:\\inetpub\\Websites\\tollpay.in\\docs\\"; // Path.Combine(Directory.GetCurrentDirectory(), "NewFolder");
            //Console.WriteLine(filedir);
            //Console.WriteLine(Directory.Exists(filedir));
            if (!Directory.Exists(filedir))
            { //check if the folder exists;
                Directory.CreateDirectory(filedir);
            }
                string profilephoto = string.Empty;

                if (a.Imagetype == "Profile")
                {
                    profilephoto = a.userid + "_Pro.jpg";
                    int srno = Convert.ToInt32(a.userid);
                    db.Users.Where(b => b.srno == srno).FirstOrDefault().profilephoto = profilephoto;
                    db.SaveChanges();
                }
                else if (a.Imagetype=="DrivingLicence")
                {
                    profilephoto = a.userid + "_DL.jpg";
                    int srno = Convert.ToInt32(a.userid);
                    string kdus = db.UserBankDetails.Where(b => b.userid == srno).FirstOrDefault().KYC_Documents_URLs;
                    kdus=kdus + "|"+ profilephoto;
                    db.UserBankDetails.Where(b => b.userid == srno).FirstOrDefault().KYC_Documents_URLs = kdus;
                    db.SaveChanges();
                }
                else if (a.Imagetype == "Aadharcard")
                {
                    profilephoto = a.userid + "_AC.jpg";
                    int srno = Convert.ToInt32(a.userid);
                    string kdus = db.UserBankDetails.Where(b => b.userid == srno).FirstOrDefault().KYC_Documents_URLs;
                    kdus = kdus + "|" + profilephoto;
                    db.UserBankDetails.Where(b => b.userid == srno).FirstOrDefault().KYC_Documents_URLs = kdus;
                    db.SaveChanges();
                }

                string file = Path.Combine(filedir, profilephoto);
            //Console.WriteLine(file);
            //Debug.WriteLine(File.Exists(file));


            if (bytes.Length > 0)
            {
                using (var stream = new FileStream(file, FileMode.Create))
                {
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Flush();
                }
            }
         
               
                string imgpath = db.AppKeys.Where(x => x.vKey == "ImagePath").FirstOrDefault().vvalue + profilephoto;
               
                response = Request.CreateResponse(HttpStatusCode.OK, imgpath);
                return response;
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.OK, "Failed to upload"+ex.Message);
                return response;


            }
        }


      //  [JwtAuthentication]
        [Route("api/Userimage/PostRCimage")]
         [JwtAuthentication]
        [HttpPost]
        public HttpResponseMessage PostRCimage([FromBody] ImaRC a)//or (Ima a) ,it works too
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError, "value");
            try
            {

           
            var bytes = Convert.FromBase64String(a.base64image);// a.base64image 
            //or full path to file in temp location
            //var filePath = Path.GetTempFileName();

            // full path to file in current project location
            string filedir = "C:\\inetpub\\Websites\\tollpay.in\\docs\\"; // Path.Combine(Directory.GetCurrentDirectory(), "NewFolder");
            //Console.WriteLine(filedir);
            //Console.WriteLine(Directory.Exists(filedir));
            if (!Directory.Exists(filedir))
            { //check if the folder exists;
                Directory.CreateDirectory(filedir);
            }

            string file = string.Empty;
             string profilephoto = string.Empty;
            //Debug.WriteLine(File.Exists(file));
            if (a.imageside == "F")
            {
                    profilephoto = a.vehicleRN + "_FS.jpg";
                file = Path.Combine(filedir, profilephoto);
                db.User_Vehicle.Where(b => b.uservehicleRegNo == a.vehicleRN).FirstOrDefault().RC_FrontImage = profilephoto;
                db.SaveChanges();
            }
            else if (a.imageside == "B")
                {
                     profilephoto = a.vehicleRN + "_BS.jpg";
                      file = Path.Combine(filedir, profilephoto);
                db.User_Vehicle.Where(b => b.uservehicleRegNo == a.vehicleRN).FirstOrDefault().RC_BackImage = profilephoto;
                db.SaveChanges();
            }
            if (bytes.Length > 0)
            {
                using (var stream = new FileStream(file, FileMode.Create))
                {
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Flush();
                }
            }
                string imgpath = db.AppKeys.Where(x => x.vKey == "ImagePath").FirstOrDefault().vvalue + profilephoto;
                response = Request.CreateResponse(HttpStatusCode.OK, imgpath);
                return response;
                //return Ok();
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.OK, "Failed to upload" + ex.Message);
                return response;
                
            }
        }
    }
}
