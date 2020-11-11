using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using WebApplicationTP.DAL;

namespace DPTPWebAPI.Controllers
{
    public class UserTollTrackersController : ApiController
    {
        private DP_TPEntities db = new DP_TPEntities();
        Notifications ns = new Notifications();
        [JwtAuthentication]
        // GET: api/UserTollTrackers/5
        [ResponseType(typeof(GetTollPlazzDetailsByTripId_Result))]

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetUserTollTracker(int id)
        {

            List<GetTollPlazzDetailsByTripId_Result> res = db.GetTollPlazzDetailsByTripId(id).ToList();

            if (res == null)
            {
                return NotFound();
            }

            return Ok(res);
        }



        [JwtAuthentication]
        // GET: api/UserTollTrackers/5

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult VerifyFastTagBooking(string RFID, string TollPlazaId)
        {
            User_Vehicle vh = db.User_Vehicle.Where(v => v.RFIDNumber == RFID).FirstOrDefault();
            User vu = db.Users.Where(u => u.srno == vh.userid).FirstOrDefault();
            TollPayWallet tpw = db.TollPayWallets.Where(w => w.UserID == vh.userid).FirstOrDefault();
            DateTime d = DateTime.Now;
            UserTollTracker utt = db.UserTollTrackers.Where(ut => ut.TollPlazzaNo == Convert.ToInt32(TollPlazaId) && ut.VehicleNumber == vh.uservehicleRegNo && ut.tripDate <= d && ut.tripEndDate <= d).FirstOrDefault();
            utt.TollRemark = "Pass_" + d.ToString();
            db.SaveChanges();
            string smsmsg;
            smsmsg = "Dear " + vu.userEmail + " , TollPay.in U Just Passed. -" + TollPlazaId + ", TollAmt : " + utt.TollCost + "We be deducted";
            ns.sendsms("smsfrom", vu.mobno1, smsmsg);
            ns.sendemail(vu.userEmail, "", "TollPay.IN Notifications", smsmsg);
            bool res = true;
            return Ok(res);
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
        private bool UserTollTrackerExists(int id)
        {
            return db.UserTollTrackers.Count(e => e.srno == id) > 0;
        }
    }
}