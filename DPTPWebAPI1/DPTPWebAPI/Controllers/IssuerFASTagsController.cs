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
    public class IssuerFASTagsController : ApiController
    {
        private DP_TPEntities db = new DP_TPEntities();

        // GET: api/IssuerFASTags
        [AllowAnonymous]
        public IQueryable<IssuerFASTag> GetIssuerFASTags()
        {
            return db.IssuerFASTags;
        }

        // GET: api/IssuerFASTags/5
        [ResponseType(typeof(IssuerFASTag))]
        public IHttpActionResult GetIssuerFASTag(int id)
        {
            IssuerFASTag issuerFASTag = db.IssuerFASTags.Find(id);
            if (issuerFASTag == null)
            {
                return NotFound();
            }

            return Ok(issuerFASTag);
        }

        // PUT: api/IssuerFASTags/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutIssuerFASTag(int id, IssuerFASTag issuerFASTag)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != issuerFASTag.srno)
            {
                return BadRequest();
            }

            db.Entry(issuerFASTag).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IssuerFASTagExists(id))
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

        // POST: api/IssuerFASTags
        [ResponseType(typeof(IssuerFASTag))]
        public IHttpActionResult PostIssuerFASTag(IssuerFASTag issuerFASTag)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.IssuerFASTags.Add(issuerFASTag);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = issuerFASTag.srno }, issuerFASTag);
        }

        //// DELETE: api/IssuerFASTags/5
        //[ResponseType(typeof(IssuerFASTag))]
        //public IHttpActionResult DeleteIssuerFASTag(int id)
        //{
        //    IssuerFASTag issuerFASTag = db.IssuerFASTags.Find(id);
        //    if (issuerFASTag == null)
        //    {
        //        return NotFound();
        //    }

        //    db.IssuerFASTags.Remove(issuerFASTag);
        //    db.SaveChanges();

        //    return Ok(issuerFASTag);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool IssuerFASTagExists(int id)
        {
            return db.IssuerFASTags.Count(e => e.srno == id) > 0;
        }
    }
}