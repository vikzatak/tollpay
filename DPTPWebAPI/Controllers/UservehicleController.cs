using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace DPTPWebAPI.Controllers
{
    public class clsCustomerDetail
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UservehicleController : ApiController
    {
        private DP_TPEntities db = new DP_TPEntities();
        [JwtAuthentication]
        // GET: api/Uservehicle
        public IQueryable<User_Vehicle> GetUser_Vehicle()
        {
            return db.User_Vehicle;
        }

        //// GET: api/Uservehicle/5
        //[ResponseType(typeof(User_Vehicle))]
        //public IHttpActionResult GetUser_Vehicle(long id)
        //{
        //    User_Vehicle user_Vehicle = db.User_Vehicle.Find(id);
        //    if (user_Vehicle == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(user_Vehicle);
        //}
         [JwtAuthentication]
        // GET: api/Uservehicle/5
        [ResponseType(typeof(User_Vehicle))]
        public IQueryable<User_Vehicle> GetUser_VehiclebyUserId(string username)
        {
            int userid = db.Users.Where(l => l.username == username).Select(u => u.srno).FirstOrDefault();
            return db.User_Vehicle.Where(l => l.userid == userid);
        }
        [JwtAuthentication]
        // PUT: api/Uservehicle/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser_Vehicle(long id, User_Vehicle user_Vehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user_Vehicle.srno)
            {
                return BadRequest();
            }

            db.Entry(user_Vehicle).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!User_VehicleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


         [JwtAuthentication]
        // POST: api/Uservehicle
        [ResponseType(typeof(User_Vehicle))]
        public IHttpActionResult PostUser_Vehicle(User_Vehicle user_V)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                User_Vehicle UV = db.User_Vehicle.Where(u => u.OwnerName == user_V.OwnerName &&
                u.uservehicleRegNo == user_V.uservehicleRegNo).FirstOrDefault();
                if (UV != null)
                {
                    UV.userid = user_V.userid;
                    UV.CreatedDate = DateTime.Now;
                    UV.Latitude = user_V.Latitude;
                    UV.Longitude = user_V.Longitude;

                    db.SaveChanges();
                }
                else
                {
                    db.User_Vehicle.Add(user_V);
                    db.SaveChanges();
                }
            }
            catch(Exception ex )
            {
                return CreatedAtRoute("DefaultApi", new { uservehicleStatus = ex.Message}, user_V);

            }

            return CreatedAtRoute("DefaultApi", new { id = user_V.srno }, user_V);
        }


       


        [JwtAuthentication]
        // DELETE: api/Uservehicle/5
        [ResponseType(typeof(User_Vehicle))]
        public IHttpActionResult DeleteUser_Vehicle(long id)
        {
            User_Vehicle user_Vehicle = db.User_Vehicle.Find(id);
            if (user_Vehicle == null)
            {
                return NotFound();
            }

            db.User_Vehicle.Remove(user_Vehicle);
            db.SaveChanges();

            return Ok(user_Vehicle);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [JwtAuthentication]
        private bool User_VehicleExists(long id)
        {
            return db.User_Vehicle.Count(e => e.srno == id) > 0;
        }

        [JwtAuthentication]
        // GET: api/Uservehicle/5
        [Route("api/GetRidersByOwnersId")]
        [HttpPost]
        public HttpResponseMessage GetUsersByOwnersId(HttpRequestMessage request, dist ownerID)
        {
            int VownerId=Convert.ToInt32(ownerID.distributorid);
            return request.CreateResponse(HttpStatusCode.OK, db.VTP_UsersBy_OwnerID(VownerId));
        }//method end

        [JwtAuthentication]
        // GET: api/Uservehicle/5
        [Route("TPapi/GetNearestVehicles")]
        [HttpPost]
        public HttpResponseMessage GetNearestVehicles(HttpRequestMessage request, clsCustomerDetail ccd)
        {
            
            return request.CreateResponse(HttpStatusCode.OK, db.VTP_GetNearestVehicles(ccd.latitude, ccd.longitude));
        }//method end

        [JwtAuthentication]
        // GET: api/Uservehicle/5
        [Route("api/GetVehicleNoByRiderId")]
        [HttpPost]
        public HttpResponseMessage GetVehicleNoByRiderId(HttpRequestMessage request, dist ownerID)
        {
            if (ownerID != null)
            {
                int VownerId = Convert.ToInt32(ownerID.distributorid);
                
                return request.CreateResponse(HttpStatusCode.OK, db.User_Vehicle.Where(x=>x.userid==VownerId).ToList());
            }
            else
            {
                return request.CreateResponse(HttpStatusCode.BadRequest);
            } 

            
        }//method end
    }
}