using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using DPTPWebAPI;

namespace DPTPWebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PaymentGatewayResponsesController : ApiController
    {
        private DP_TPEntities db = new DP_TPEntities();

        //// GET: api/PaymentGatewayResponses
        //public IQueryable<PaymentGatewayResponse> GetPaymentGatewayResponses()
        //{
        //    return db.PaymentGatewayResponses;
        //}

        //// GET: api/PaymentGatewayResponses/5
        //[ResponseType(typeof(PaymentGatewayResponse))]
        //public IHttpActionResult GetPaymentGatewayResponse(int id)
        //{
        //    PaymentGatewayResponse paymentGatewayResponse = db.PaymentGatewayResponses.Find(id);
        //    if (paymentGatewayResponse == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(paymentGatewayResponse);
        //}

        //// PUT: api/PaymentGatewayResponses/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutPaymentGatewayResponse(int id, PaymentGatewayResponse paymentGatewayResponse)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != paymentGatewayResponse.srno)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(paymentGatewayResponse).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!PaymentGatewayResponseExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        // POST: api/PaymentGatewayResponses
        [AllowAnonymous]
        [ResponseType(typeof(PaymentGatewayResponse))]
        [HttpPost]
        public IHttpActionResult PostPaymentGatewayResponse(PaymentGatewayResponse paymentGatewayResponse)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PaymentGatewayResponses.Add(paymentGatewayResponse);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = paymentGatewayResponse.srno }, paymentGatewayResponse);
        }

        //// DELETE: api/PaymentGatewayResponses/5
        //[ResponseType(typeof(PaymentGatewayResponse))]
        //public IHttpActionResult DeletePaymentGatewayResponse(int id)
        //{
        //    PaymentGatewayResponse paymentGatewayResponse = db.PaymentGatewayResponses.Find(id);
        //    if (paymentGatewayResponse == null)
        //    {
        //        return NotFound();
        //    }

        //    db.PaymentGatewayResponses.Remove(paymentGatewayResponse);
        //    db.SaveChanges();

        //    return Ok(paymentGatewayResponse);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PaymentGatewayResponseExists(int id)
        {
            return db.PaymentGatewayResponses.Count(e => e.srno == id) > 0;
        }
    }
}