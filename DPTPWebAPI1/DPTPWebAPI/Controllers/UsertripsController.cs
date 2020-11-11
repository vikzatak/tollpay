using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using WebApplicationTP.DAL;

namespace DPTPWebAPI.Controllers
{


    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UsertripsController : ApiController
    {
        private DP_TPEntities db = new DP_TPEntities();
        Notifications ns = new Notifications();
        [JwtAuthentication]
        // GET: api/Usertrips
        public IQueryable<User_Trips> GetUser_Trips()
        {
            return db.User_Trips;
        }
        [JwtAuthentication]
        // GET: api/Usertrips/5
        [ResponseType(typeof(User_Trips))]
        public IQueryable<User_Trips> GetUser_Trips(int id)
        {
            //User_Trips user_Trips = db.User_Trips.Where(l => l.userid == id).FirstOrDefault();//Find(id);
            //if (user_Trips == null)
            //{
            //    return NotFound();
            //}

            return db.User_Trips.Where(l => l.userid == id);
        }
        [JwtAuthentication]
        // PUT: api/Usertrips/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser_Trips(int id, User_Trips user_Trips)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user_Trips.srno)
            {
                return BadRequest();
            }

            db.Entry(user_Trips).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!User_TripsExists(id))
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
        [AllowAnonymous]
        [ResponseType(typeof(User_Trips))]
        public IHttpActionResult PostUser_Trips(User_Trips user_Trips)
        {
            //bool cb = true; //sagar code
            bool cb = false; //sachin code
            if (cb)
            {
                //User_Trips user_Trips = new User_Trips();
                //user_Trips.tripStartLocation = source;
                //user_Trips.tripEndLocation = destination;
                //user_Trips.tripDate =Convert.ToDateTime(startdate);
                bool flag = false;

                IQueryable<User_Trips> result = db.User_Trips.Where(l => l.userid == user_Trips.userid);
                User_Trips[] utrips = result.ToArray();
                foreach (var item in utrips)
                {
                    if (user_Trips.tripStartLocation == item.tripStartLocation && user_Trips.tripEndLocation == item.tripEndLocation)
                    {
                        flag = true;
                        item.tripDate = user_Trips.tripDate;
                        item.tripEndDate = user_Trips.tripEndDate;
                        item.CreatedDate = System.DateTime.Now;
                        db.SaveChanges();
                    }
                }
                if (flag == false)
                {
                    user_Trips.CreatedDate = DateTime.Now;
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    db.User_Trips.Add(user_Trips);
                    db.SaveChanges();
                }
            }
            else
            {
                int? uid = user_Trips.userid;
                string src = user_Trips.tripStartLocation;
                string dest = user_Trips.tripEndLocation;
                decimal? tripamt = user_Trips.totalCost;
                string tt = "single";
                string vehicleno = user_Trips.userVechicleNumber;
                string vehicleType = db.User_Vehicle.Where(p => p.uservehicleRegNo == vehicleno).FirstOrDefault().uservehicleType;
                string pm = "wallet";
                string POSDetails = "wallet";
                DateTime? stdate = user_Trips.tripDate;
                DateTime? enddate = user_Trips.tripEndDate;
                string resulttolllist = db.RouteTollsQueries.Where(r => r.src.Contains(src) && r.dest.Contains(dest)).FirstOrDefault().routetolls;

                // db.CreateTrip(#ut.userid, #ut.tripStartLocation, #ut.tripEndLocation, #resulttolllist, #TripTotalAmount, #pm,
                //#TxtPOSDetails.Text, #TxtPOSDetails.Text, #TxtPOSDetails.Text, ut.userVechicleNumber, vt, tt, ut.tripDate, ut.tripEndDate);

                int rid = db.CreateTrip(uid, src, dest, resulttolllist, tripamt, pm, POSDetails, POSDetails, POSDetails, vehicleno, vehicleType, tt, stdate, enddate, user_Trips.DriverMobileNo, user_Trips.DriverOwnerMobNo);
                string smsmsg;
                smsmsg = "Dear Customer, Rs. " + tripamt + " Paid via " + POSDetails + "for Trip Src-" + src + "Dest- " + dest;
                ns.sendsms("smsfrom", user_Trips.DriverMobileNo, smsmsg);
                string userEmail = db.Users.Where(o => o.srno == uid).FirstOrDefault().userEmail;
                ns.sendemail(userEmail, "", "TollPay.IN Trip Booked", smsmsg);
            }
            return CreatedAtRoute("DefaultApi", new { id = user_Trips.srno }, user_Trips);
        }
        [JwtAuthentication]
        // DELETE: api/Usertrips/5
        [ResponseType(typeof(User_Trips))]
        public IHttpActionResult DeleteUser_Trips(int id)
        {
            User_Trips user_Trips = db.User_Trips.Find(id);
            if (user_Trips == null)
            {
                return NotFound();
            }

            db.User_Trips.Remove(user_Trips);
            db.SaveChanges();

            return Ok(user_Trips);
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
        private bool User_TripsExists(int id)
        {
            return db.User_Trips.Count(e => e.srno == id) > 0;
        }
    }
}