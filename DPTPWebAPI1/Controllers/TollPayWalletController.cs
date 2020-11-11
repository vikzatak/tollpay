using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace DPTPWebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TollPayWalletController : ApiController
    {
        private DP_TPEntities db = new DP_TPEntities();
      
        [JwtAuthentication]
        // GET: api/TollPayWallet/5
        [ResponseType(typeof(TollPayWallet))]
        public TollPayWallet Get(int userid)
        {
            //int userid = db.Users.Where(l => l.username == username).Select(u => u.srno).FirstOrDefault();
            //return userid;
            TollPayWallet tollpaywallet = db.TollPayWallets.Where(l => l.UserID == userid).FirstOrDefault();
            return tollpaywallet;
        }
        [JwtAuthentication]
        // POST: api/TollPayWallet
        [ResponseType(typeof(TollPayWallet))]
        public IHttpActionResult Post(string username)
        {
            int userid = db.Users.Where(l => l.username == username).Select(u => u.srno).FirstOrDefault();
            TollPayWallet tollpaywallet = new TollPayWallet();
            tollpaywallet.UserID = Convert.ToInt32(userid);
            tollpaywallet.WalletAmount = Convert.ToDecimal(0);
            db.TollPayWallets.Add(tollpaywallet);
            db.SaveChanges();
            return CreatedAtRoute("DefaultApi", new { id = tollpaywallet.SrNo }, tollpaywallet);
        }

        // [JwtAuthentication]
        //// POST: api/TollPayWallet
        //[ResponseType(typeof(TollPayWallet))]
        //public IHttpActionResult Post(TollPayWallet tollpaywallet)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.TollPayWallets.Add(tollpaywallet);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = tollpaywallet.SrNo }, tollpaywallet);
        //}

        // [JwtAuthentication]
        //// PUT: api/TollPayWallet/5
        //public void Put(TollPayWallet tollpaywallet)
        //{
        //    //TollPayWallet tollpaywalletobj = db.TollPayWallets.Where(l => l.UserID == tollpaywallet.UserID).FirstOrDefault();
        //    //tollpaywalletobj.WalletAmount = tollpaywalletobj.WalletAmount+ tollpaywallet.WalletAmount;
        //    ////db.TollPayWallets.Add(tollpaywalletobj);
        //    //db.SaveChanges();
        //}

        // DELETE: api/TollPayWallet/5
        public void Delete(int id)
        {
        }
    }
}
