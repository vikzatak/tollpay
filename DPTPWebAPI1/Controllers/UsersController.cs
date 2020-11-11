using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using WebApplicationTP.DAL;

namespace DPTPWebAPI.Controllers
{
    public class clsstatuscode
    {
        public int statuscode { get; set; }
        public string statusdesc { get; set; }

       

    }

    public class clslststatuscode
    {
      

        public List<clsstatuscode>  getstatuscodes()
        {
             List<clsstatuscode> lststatuscode = new List<clsstatuscode>();
            lststatuscode.Add(new clsstatuscode() { statuscode= 000,statusdesc= "Success" });
            lststatuscode.Add(new clsstatuscode() { statuscode = 001, statusdesc = "Failure" });
            lststatuscode.Add(new clsstatuscode() { statuscode = 003, statusdesc = "ServerDown" });
            lststatuscode.Add(new clsstatuscode() { statuscode = 004, statusdesc = "RequiredField" });
            lststatuscode.Add(new clsstatuscode() { statuscode = 013, statusdesc = "Token Missing" });
            lststatuscode.Add(new clsstatuscode() { statuscode = 014, statusdesc = "Invalid Token" });
            lststatuscode.Add(new clsstatuscode() { statuscode = 017, statusdesc = "TokenGenerateProblem" });
            lststatuscode.Add(new clsstatuscode() { statuscode = 019, statusdesc = "Already Exists" });
            lststatuscode.Add(new clsstatuscode() { statuscode = 020, statusdesc = "Invalid" });
    
            return lststatuscode;
        }

        
    }
    public class loginUser
    {
        public string username { get; set; }
        public string password { get; set; }
    }
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UsersController : ApiController
    {
        Notifications ns = new Notifications();
        private DP_TPEntities db = new DP_TPEntities();

        public IQueryable<User> GetUsers()
        {
            return db.Users;
        }
        [JwtAuthentication]
        [ResponseType(typeof(User))]
        public int GetUser(string username)
        {
            int userid = db.Users.Where(l => l.username == username).Select(u => u.srno).FirstOrDefault();
            return userid;
        }

        [AllowAnonymous]
        [ResponseType(typeof(User))]
        [Route("api/RefreshToken")]
        [HttpPost]
        public HttpResponseMessage RefreshToken(HttpRequestMessage request, loginUser lu)
        {
            var jsonObject = new JObject();
            jsonObject.Add("token", GenerateToken(lu.username));
            return request.CreateResponse(HttpStatusCode.OK, jsonObject);

        }
            // GET: api/Users/5
        [AllowAnonymous]
        [ResponseType(typeof(User))]
        public HttpResponseMessage GetUser(HttpRequestMessage request, string username, string password)
        {
            User user = CheckUser(username, password);
            if (user != null)
            {
                // strong typed instance 
                var jsonObject = new JObject();
                jsonObject.Add("userid", user.srno);
                jsonObject.Add("username", user.username);
                string imgpath = db.AppKeys.Where(x => x.vKey == "ImagePath").FirstOrDefault().vvalue + user.profilephoto;
                jsonObject.Add("profileimg", imgpath);
                jsonObject.Add("firstname", user.FirstName);
                jsonObject.Add("lastname", user.LastName);
                jsonObject.Add("userRoll", user.userRollstatus);
                jsonObject.Add("businessname", user.BusinessName);
                jsonObject.Add("useremail", user.userEmail);
                jsonObject.Add("useraddress", user.address);
                jsonObject.Add("usermob1", user.mobno1);
                jsonObject.Add("usermob2", user.mobno2);
                jsonObject.Add("distributorid", user.DistributorId);
                jsonObject.Add("token", GenerateToken(username));
                return request.CreateResponse(HttpStatusCode.OK, jsonObject);
            }
            return new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }

        [AllowAnonymous]
        [ResponseType(typeof(User))]
        [Route("api/Login")]
        [HttpPost]
        public HttpResponseMessage Login(HttpRequestMessage request, loginUser lu)

        {
            User user = CheckUser(lu.username, lu.password);
            if (user != null)

            {
                // strong typed instance 
                var jsonObject = new JObject();
                jsonObject.Add("userid", user.srno);
                jsonObject.Add("username", user.username);
                string imgpath = db.AppKeys.Where(x => x.vKey == "ImagePath").FirstOrDefault().vvalue + user.profilephoto;
                jsonObject.Add("profileimg", imgpath);
                jsonObject.Add("firstname", user.FirstName);
                jsonObject.Add("lastname", user.LastName);
                jsonObject.Add("userRoll", user.userRollstatus);
                jsonObject.Add("businessname", user.BusinessName);
                jsonObject.Add("useremail", user.userEmail);
                jsonObject.Add("useraddress", user.address);
                jsonObject.Add("usermob1", user.mobno1);
                jsonObject.Add("usermob2", user.mobno2);

                if (user.userRollstatus == "S")
                {
                    User u = db.Users.Where(d => d.srno == user.DistributorId).FirstOrDefault();
                    string dn = u.FirstName + " " + u.LastName;
                    jsonObject.Add("distributorid", user.DistributorId);
                    jsonObject.Add("distributorName", dn);
                }
                /*
                if (user.userRollstatus == "D")
                {
                    var ts = db.TagStackByDistributor(user.srno.ToString());
                    jsonObject.Add("TagStock", JsonConvert.SerializeObject(ts));
                    var st = db.GetSalesTeambyDistributorId(user.srno.ToString());
                    jsonObject.Add("SalesTeam", JsonConvert.SerializeObject(st));
                    var sr = db.salesreportbyDistributorId(user.srno.ToString());
                    jsonObject.Add("SalesReport", JsonConvert.SerializeObject(sr));

                }*/
                jsonObject.Add("token", GenerateToken(lu.username));
                return request.CreateResponse(HttpStatusCode.OK, jsonObject);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
            
        }
        public User CheckUser(string username, string password)
        {
            // should check in the database
            try
            {
                User user = db.Users.Where(l => l.username == username && l.password == password).FirstOrDefault();
                if (user == null)
                {
                    return null;
                }
                else
                {
                    user.password = "xxxxxxx";
                    return user;
                }
            }
            catch (Exception e)
            {

                return null;
            }
        }

        [JwtAuthentication]
        [Route("api/GetSalesTeambyDistributorId")]
        [HttpPost]
        public HttpResponseMessage GetSalesTeambyDistributorId(HttpRequestMessage request, dist d)
        {

            if (string.Empty == d.distributorid)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }




            return request.CreateResponse(HttpStatusCode.OK, db.GetSalesTeambyDistributorId(d.distributorid));
        }
        // PUT: api/Users/5
        [JwtAuthentication]
        [ResponseType(typeof(void))]
        [Route("api/UpdateUser")]
        [HttpPost]
        public IHttpActionResult UpdateUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (user == null)
            {
                return BadRequest();
            }
            else
            {
                try
                {
                    User utoupdate = db.Users.Where(o => o.srno == user.srno).FirstOrDefault();

                    utoupdate.FirstName = user.FirstName;
                    utoupdate.LastName = user.LastName;
                    utoupdate.BusinessName = user.BusinessName;
                    // db.Users.Find(id).username = user.username;
                    utoupdate.address = user.address;
                    utoupdate.userEmail = user.userEmail;
                    utoupdate.mobno1 = user.mobno1;
                    utoupdate.mobno2 = user.mobno2;
                    utoupdate.status = user.status;


                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {

                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [AllowAnonymous]
        // POST: api/Users
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            User vuser = db.Users.Where(u => u.username == user.username).FirstOrDefault();
            if (vuser == null)
            {
                db.Users.Add(user);
                db.SaveChanges();
                string smsmsg;
                smsmsg = "Dear Customer, Login to Web 'https://TollPay.IN' with UserName=" + user.mobno1 + ", Password=" + user.password;
                ns.sendsms("smsfrom", user.mobno1, smsmsg);
                ns.sendemail(user.userEmail, "", "Welcome to TollPay.IN", smsmsg);
                return CreatedAtRoute("DefaultApi", new { id = user.srno }, user);
            }else
            {
                clslststatuscode css = new clslststatuscode();
                clsstatuscode scd= css.getstatuscodes().Where(s => s.statuscode == 019).FirstOrDefault();
                return CreatedAtRoute("DefaultApi", new { id = user.srno }, scd);
            }
        }

        //// DELETE: api/Users/5
        //[ResponseType(typeof(User))]
        //public IHttpActionResult DeleteUser(int id)
        //{
        //    User user = db.Users.Find(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Users.Remove(user);
        //    db.SaveChanges();

        //    return Ok(user);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.srno == id) > 0;
        }

        /// <summary>
        /// below code to generate symmetric Secret Key
        public static System.Security.Cryptography.HMAC hmac = new HMACSHA256();
        private static string secret = Convert.ToBase64String(hmac.Key);
        /// </summary>
        //Method to Generate token
        public static string GenerateToken(string username, int expireMinutes = 60)
        {
            var symmetricKey = Convert.FromBase64String(secret);
            var tokenHandler = new JwtSecurityTokenHandler();

            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, username)
        }),

                Expires = now.AddMinutes(Convert.ToInt32(expireMinutes)),

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(symmetricKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            return token;
        }
        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                    return null;

                var symmetricKey = Convert.FromBase64String(secret);

                var validationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
                };

                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                return principal;
            }

            catch (Exception)
            {
                return null;
            }
        }

    }
}