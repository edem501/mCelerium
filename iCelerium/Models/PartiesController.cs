using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace iCelerium.Models
{
    public class PartiesController : ApiController
    {
        private SMSServersEntities db = new SMSServersEntities();

        // GET: api/Parties
        public IQueryable<Party> GetParties()
        {
            return db.Parties;
        }

        //// GET: api/Parties/5
        //[ResponseType(typeof(Party))]
        //public async Task<IHttpActionResult> GetParty(int id)
        //{
        //    Party party = await db.Parties.FindAsync(id);
        //    if (party == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(party);
        //}

        //// PUT: api/Parties/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutParty(int id, Party party)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != party.ID)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(party).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!PartyExists(id))
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

        //// POST: api/Parties
        //[ResponseType(typeof(Party))]
        //public async Task<IHttpActionResult> PostParty(Party party)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Parties.Add(party);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = party.ID }, party);
        //}

        //// DELETE: api/Parties/5
        //[ResponseType(typeof(Party))]
        //public async Task<IHttpActionResult> DeleteParty(int id)
        //{
        //    Party party = await db.Parties.FindAsync(id);
        //    if (party == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Parties.Remove(party);
        //    await db.SaveChangesAsync();

        //    return Ok(party);
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        //private bool PartyExists(int id)
        //{
        //    return db.Parties.Count(e => e.ID == id) > 0;
        //}
    }
}