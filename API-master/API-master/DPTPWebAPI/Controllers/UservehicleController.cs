using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace DPTPWebAPI.Controllers
{
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
        [AllowAnonymous]
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


        [AllowAnonymous]
        // POST: api/Uservehicle
        [ResponseType(typeof(User_Vehicle))]
        public IHttpActionResult PostUser_Vehicle(User_Vehicle user_Vehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.User_Vehicle.Add(user_Vehicle);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = user_Vehicle.srno }, user_Vehicle);
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
    }
}