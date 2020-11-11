using DPTPWebAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using WebApplicationTP.DAL;

namespace DPTPWebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MapperVehicleClassesController : ApiController
    {
        private DP_TPEntities db = new DP_TPEntities();
        Notifications ns = new Notifications();
        // GET: api/MapperVehicleClasses
         [JwtAuthentication]
        public IQueryable<MapperVehicleClass> GetMapperVehicleClasses()
        {
            return db.MapperVehicleClasses;
        }
         [JwtAuthentication]
        [Route("api/TranP_TruckTypes")]
        public List<TranP_TruckTypes> GetTranP_TruckTypes()
        {
            return db.TranP_TruckTypes.ToList();
        }
      
         [JwtAuthentication]
        [Route("api/TranP_MaterialTypes")]
        public List<TranP_MaterialTypes> GetTranP_MaterialTypes()
        {
            return db.TranP_MaterialTypes.ToList();
        }

        // GET: api/MapperVehicleClasses/5
        [ResponseType(typeof(MapperVehicleClass))]
        public IHttpActionResult GetMapperVehicleClass(int id)
        {
            MapperVehicleClass mapperVehicleClass = db.MapperVehicleClasses.Find(id);
            if (mapperVehicleClass == null)
            {
                return NotFound();
            }

            return Ok(mapperVehicleClass);
        }

         [JwtAuthentication]
        [ResponseType(typeof(User_Trips))]
        [Route("api/TranP_BookTruckTrip")]
        public IHttpActionResult PostTranP_Truck_Trips(TranP_BookTrip tt)
        {
           
            
                db.TranP_TruckTrips.Add(tt.tranP_trucktrip);
                db.SaveChanges();
                db.TranP_TruckTrip_Payment.Add(tt.tranP_truckTrip_payment);
                db.SaveChanges();
                string smsmsg;
                smsmsg = "Dear Customer, Rs. " + tt.tranP_trucktrip.FareCost + " Paid via " + tt.tranP_trucktrip.tripDate + "for Truck Trip Src-" + tt.tranP_trucktrip.tripStartLocation  + "Dest- " + tt.tranP_trucktrip.tripEndLocation;
                ns.sendsms("smsfrom", tt.tranP_trucktrip.DriverMobileNo, smsmsg);
                string userEmail = db.Users.Where(o => o.srno == tt.tranP_trucktrip.userid).FirstOrDefault().userEmail;
                ns.sendemail(userEmail, "", "TollPay.IN Truck Trip Booked", smsmsg);

            var a = new { TruckTripId = tt.tranP_trucktrip.srno };
            return Ok(a);
        }

         [JwtAuthentication]
        [ResponseType(typeof(User_Trips))]
        [HttpGet]
        [Route("api/TranP_BookTruckTripByUserId")]
        public IHttpActionResult PostTranP_Truck_Trips(int userid)
        {


            var a=db.TranP_TruckTrips.Where(u => u.userid == userid).ToList();

                     

          
            return Ok(a);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MapperVehicleClassExists(int id)
        {
            return db.MapperVehicleClasses.Count(e => e.SrNo == id) > 0;
        }
    }
}