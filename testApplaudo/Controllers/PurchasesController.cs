using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class PurchasesController : ControllerBase
    {
        private readonly testApplaudoContext _context;

        public PurchasesController(testApplaudoContext context)
        {
            _context = context;
        }

        // GET: api/Purchases
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Purchase>>> GetPurchase()
        {
            return await _context.Purchase.ToListAsync();
        }

        // GET: api/Purchases/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Purchase>> GetPurchase(int id)
        {
            var purchase = await _context.Purchase.FindAsync(id);

            if (purchase == null)
            {
                return NotFound();
            }

            return purchase;
        }

        // PUT: api/Purchases/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutPurchase(int id, Purchase purchase)
        //{
        //    if (id != purchase.PurchaseId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(purchase).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!PurchaseExists(id))
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

        // POST: api/Purchases
        [HttpPost]
        public async Task<ActionResult<Purchase>> PostPurchase(Purchase purchase)
        {
            // here we recover the user or customer login , without careing if it comes or not in
            // the purchse object and we update the purchase object with the corresoponding user
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var LogedUser = identity.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
            purchase.UserId = LogedUser;

            // we recover the existing product quanty
            var products = await _context.Products.FindAsync(purchase.ProductId);

            if ((products.inStock - purchase.PurchaseTotal) > 0)
            {
                _context.Purchase.Add(purchase);
                await _context.SaveChangesAsync();
                var createResult = CreatedAtAction("GetPurchase", new { id = purchase.PurchaseId }, purchase);

                products.inStock = (products.inStock - purchase.PurchaseTotal);

                if (createResult.StatusCode == 201)
                {
                    var stock = new Stock
                    {
                        PurchaseId = purchase.PurchaseId,
                        ProductID = purchase.ProductId,
                        MovementTypeid = 2,
                        MovementDate = DateTime.UtcNow,
                        MovementQuantity = purchase.PurchaseTotal,
                        ProductQuantity = products.inStock 
                    };
                    _context.Products.Update(products);
                    _context.Stock.Add(stock);
                    await _context.SaveChangesAsync();

                    return createResult;
                }
                else
                {
                    return NoContent();
                }
            }
            return NoContent();
        }

        // DELETE: api/Purchases/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<Purchase>> DeletePurchase(int id)
        //{
        //    var purchase = await _context.Purchase.FindAsync(id);
        //    if (purchase == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Purchase.Remove(purchase);
        //    await _context.SaveChangesAsync();

        //    return purchase;
        //}

        private bool PurchaseExists(int id)
        {
            return _context.Purchase.Any(e => e.PurchaseId == id);
        }
    }
}
