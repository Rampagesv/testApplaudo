using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        //PUT: api/Purchases/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPurchase(int id, Purchase purchase)
        {

            if (id != purchase.PurchaseId)
            {
                return BadRequest();
            }

            _context.Entry(purchase).State = EntityState.Modified;

            try
            {
                // I recover the existing product quanty
                var products = await _context.Products.FindAsync(purchase.PurchaseId);

                if ((products.inStock - purchase.PurchaseTotal) < 0)
                {
                    var stock = new Stock
                    {
                        PurchaseId = purchase.PurchaseId,
                        ProductID = purchase.ProductId,
                        MovementTypeid = 2,
                        MovementDate = DateTime.UtcNow,
                        MovementQuantity = purchase.PurchaseTotal,
                        ProductQuantity = products.inStock - purchase.PurchaseTotal
                    };

                    products.inStock = (products.inStock - purchase.PurchaseTotal);

                    _context.Products.Update(products);
                    _context.Stock.Add(stock);

                    await _context.SaveChangesAsync();
                }
                return NotFound();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseExists(id))
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

        // POST: api/Purchases
        //[HttpPost]
        //public async Task<ActionResult<Purchase>> PostPurchase(Purchase purchase)
        //{
        //    // I recover the existing product quanty
        //    var products = await _context.Products.FindAsync(purchase.PurchaseId);

        //    _context.Purchase.Add(purchase);
        //    await _context.SaveChangesAsync();
        //    var stock = new Stock
        //    {
        //        PurchaseId = purchase.PurchaseId,
        //        ProductID = purchase.ProductId,
        //        MovementTypeid = 2,
        //        MovementDate = DateTime.UtcNow,
        //        MovementQuantity = purchase.PurchaseTotal,
        //        ProductQuantity = products.inStock - purchase.PurchaseTotal
        //    };
        //    _context.Products.Add(products);
        //    return CreatedAtAction("GetPurchase", new { id = purchase.PurchaseId }, purchase);


        //}

        // DELETE: api/Purchases/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Purchase>> DeletePurchase(int id)
        {
            var purchase = await _context.Purchase.FindAsync(id);
            if (purchase == null)
            {
                return NotFound();
            }

            _context.Purchase.Remove(purchase);
            await _context.SaveChangesAsync();

            return purchase;
        }

        private bool PurchaseExists(int id)
        {
            return _context.Purchase.Any(e => e.PurchaseId == id);
        }
    }
}
