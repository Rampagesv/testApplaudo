using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testApplaudo.Models;

namespace testApplaudo.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PriceLogsController : ControllerBase
    {
        private readonly testApplaudoContext _context;

        public PriceLogsController(testApplaudoContext context)
        {
            _context = context;
        }

        // GET: api/PriceLogs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PriceLog>>> GetPriceLog()
        {
            return await _context.PriceLog.ToListAsync();
        }

        // GET: api/PriceLogs/5
        [HttpGet("{Productid}")]
        public async Task<ActionResult<PriceLog>> GetPriceLog(int Productid)
        {
            var priceLog = await _context.PriceLog.FindAsync(Productid);

            if (priceLog == null)
            {
                return NotFound();
            }

            return priceLog;
        }

        //// PUT: api/PriceLogs/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutPriceLog(int id, PriceLog priceLog)
        //{
        //    if (id != priceLog.PriceLogid)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(priceLog).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!PriceLogExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/PriceLogs
        //[HttpPost]
        //public async Task<ActionResult<PriceLog>> PostPriceLog(PriceLog priceLog)
        //{
        //    _context.PriceLog.Add(priceLog);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetPriceLog", new { id = priceLog.PriceLogid }, priceLog);
        //}

        //// DELETE: api/PriceLogs/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<PriceLog>> DeletePriceLog(int id)
        //{
        //    var priceLog = await _context.PriceLog.FindAsync(id);
        //    if (priceLog == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.PriceLog.Remove(priceLog);
        //    await _context.SaveChangesAsync();

        //    return priceLog;
        //}

        private bool PriceLogExists(int id)
        {
            return _context.PriceLog.Any(e => e.PriceLogid == id);
        }
    }
}
