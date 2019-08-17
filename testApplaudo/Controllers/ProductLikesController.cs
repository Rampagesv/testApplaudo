using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testApplaudo.Models;

namespace testApplaudo.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductLikesController : ControllerBase
    {
        private readonly testApplaudoContext _context;

        public ProductLikesController(testApplaudoContext context)
        {
            _context = context;
        }

        // GET: api/ProductLikes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductLikes>>> GetProductLikes()
        {
            return await _context.ProductLikes.ToListAsync();
        }

        // GET: api/ProductLikes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductLikes>> GetProductLikes(int id)
        {
            var productLikes = await _context.ProductLikes.FindAsync(id);

            if (productLikes == null)
            {
                return NotFound();
            }

            return productLikes;
        }

        // PUT: api/ProductLikes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductLikes(int id, ProductLikes productLikes)
        {
            if (id != productLikes.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(productLikes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductLikesExists(id))
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

        // POST: api/ProductLikes
        [HttpPost]
        public async Task<ActionResult<ProductLikes>> PostProductLikes(ProductLikes productLikes)
        {
            _context.ProductLikes.Add(productLikes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductLikes", new { id = productLikes.ProductId }, productLikes);
        }

        //// DELETE: api/ProductLikes/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<ProductLikes>> DeleteProductLikes(int id)
        //{
        //    var productLikes = await _context.ProductLikes.FindAsync(id);
        //    if (productLikes == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.ProductLikes.Remove(productLikes);
        //    await _context.SaveChangesAsync();

        //    return productLikes;
        //}

        private bool ProductLikesExists(int id)
        {
            return _context.ProductLikes.Any(e => e.ProductId == id);
        }
    }
}
