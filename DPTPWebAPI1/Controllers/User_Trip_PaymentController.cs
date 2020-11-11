using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using WebApplicationTP.DAL;

namespace DPTPWebAPI.Controllers
{
    public class User_Trip_PaymentController : ApiController
    {
        private DP_TPEntities db = new DP_TPEntities();
        Notifications ns = new Notifications();
        [JwtAuthentication]
        // GET: api/User_Trip_Payment
        public IQueryable<User_Trip_Payment> GetUser_Trip_Payment()
        {
            return db.User_Trip_Payment;
        }
        [JwtAuthentication]
        // GET: api/User_Trip_Payment/5
        [ResponseType(typeof(User_Trip_Payment))]
        public IQueryable<User_Trip_Payment> GetUser_Trip_Payment(int id)
        {
            //User_Trip_Payment user_Trip_Payment = db.User_Trip_Payment.Find(id);
            //if (user_Trip_Payment == null)
            //{
            //    return NotFound();
            //}

            return db.User_Trip_Payment.Where(l => l.userno == id);
        }
        [JwtAuthentication]
        // PUT: api/User_Trip_Payment/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser_Trip_Payment(int id, User_Trip_Payment user_Trip_Payment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user_Trip_Payment.srno)
            {
                return BadRequest();
            }

            db.Entry(user_Trip_Payment).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!User_Trip_PaymentExists(id))
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
        // POST: api/User_Trip_Payment
        [ResponseType(typeof(User_Trip_Payment))]
        public IHttpActionResult PostUser_Trip_Payment(User_Trip_Payment user_Trip_Payment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.User_Trip_Payment.Add(user_Trip_Payment);

            try
            {
                db.SaveChanges();
                User_Trips ut = db.User_Trips.Where(t => t.userid == user_Trip_Payment.tripId).FirstOrDefault();
                string smsto = ut.DriverMobileNo + "," + ut.DriverOwnerMobNo;
                User us = db.Users.Where(u => u.srno == Convert.ToInt32(user_Trip_Payment.userno)).FirstOrDefault();
                string smsmsg;
                smsmsg = "Dear Customer, Rs. " + user_Trip_Payment.TripPaymentAmount + " Paid via " + user_Trip_Payment.TripPaymentMode + "for Trip Id -" + user_Trip_Payment.tripId;
                ns.sendsms("smsfrom", smsto, smsmsg);
                ns.sendemail(us.userEmail, "", "TollPay.IN Trip Booked", smsmsg);
            }
            catch (DbUpdateException)
            {
                if (User_Trip_PaymentExists(user_Trip_Payment.srno))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = user_Trip_Payment.srno }, user_Trip_Payment);
        }
        [JwtAuthentication]
        // DELETE: api/User_Trip_Payment/5
        [ResponseType(typeof(User_Trip_Payment))]
        public IHttpActionResult DeleteUser_Trip_Payment(int id)
        {
            User_Trip_Payment user_Trip_Payment = db.User_Trip_Payment.Find(id);
            if (user_Trip_Payment == null)
            {
                return NotFound();
            }

            db.User_Trip_Payment.Remove(user_Trip_Payment);
            db.SaveChanges();

            return Ok(user_Trip_Payment);
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
        private bool User_Trip_PaymentExists(int id)
        {
            return db.User_Trip_Payment.Count(e => e.srno == id) > 0;
        }
    }
}