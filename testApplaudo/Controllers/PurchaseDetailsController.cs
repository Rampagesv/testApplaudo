using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testApplaudo.Models;

namespace testApplaudo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseDetailsController : ControllerBase
    {
        private readonly testApplaudoContext _context;

        public PurchaseDetailsController(testApplaudoContext context)
        {
            _context = context;
        }

        // GET: api/PurchaseDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseDetails>>> GetPurchaseDetails()
        {
            return await _context.PurchaseDetails.ToListAsync();
        }

        // GET: api/PurchaseDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PurchaseDetails>> GetPurchaseDetails(int id)
        {
            var purchaseDetails = await _context.PurchaseDetails.FindAsync(id);

            if (purchaseDetails == null)
            {
                return NotFound();
            }

            return purchaseDetails;
        }

        // PUT: api/PurchaseDetails/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPurchaseDetails(int id, PurchaseDetails purchaseDetails)
        {
            if (id != purchaseDetails.PurchaseDetailsId)
            {
                return BadRequest();
            }

            _context.Entry(purchaseDetails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseDetailsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PurchaseDetails
        [HttpPost]
        public async Task<ActionResult<PurchaseDetails>> PostPurchaseDetails(PurchaseDetails purchaseDetails)
        {
            _context.PurchaseDetails.Add(purchaseDetails);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPurchaseDetails", new { id = purchaseDetails.PurchaseDetailsId }, purchaseDetails);
        }

        // DELETE: api/PurchaseDetails/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PurchaseDetails>> DeletePurchaseDetails(int id)
        {
            var purchaseDetails = await _context.PurchaseDetails.FindAsync(id);
            if (purchaseDetails == null)
            {
                return NotFound();
            }

            _context.PurchaseDetails.Remove(purchaseDetails);
            await _context.SaveChangesAsync();

            return purchaseDetails;
        }

        private bool PurchaseDetailsExists(int id)
        {
            return _context.PurchaseDetails.Any(e => e.PurchaseDetailsId == id);
        }
    }
}
