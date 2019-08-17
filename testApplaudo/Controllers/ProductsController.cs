using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using testApplaudo.Models;

namespace testApplaudo.Controllers
{
    [SwaggerTag("Requests about values")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly testApplaudoContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private string LogedUser = string.Empty;

        public ProductsController(testApplaudoContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Products>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Products>> GetProducts(int id)
        {
            var products = await _context.Products.FindAsync(id);

            if (products == null)
            {
                return NotFound();
            }

            return products;
        }

        //// PUT: api/Products/5
        //[Authorize]
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutProducts(int id, Products products)
        //{
        //    var identity = HttpContext.User.Identity as ClaimsIdentity;
        //    bool canContinue = await IdentifyAdminLoginAsync(identity);

        //    if (canContinue)
        //    {
        //        if (id != products.ProductId)
        //        {
        //            return BadRequest();
        //        }

        //        _context.Entry(products).State = EntityState.Modified;

        //        try
        //        {
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ProductsExists(id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }

        //        return NoContent();
        //    }

        //    return NotFound();
        //}

        // PUT: api/Products/Price/5
        [Authorize]
        [HttpPut("Price/{id}")]
        public async Task<IActionResult> PutProductsPrice(int id, Products productsPrice)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            bool canContinue = await IdentifyAdminLoginAsync(identity);

            if (canContinue)
            {

                LogedUser = identity.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();

                var products = await _context.Products.FindAsync(id);
                products.ProductPrice = productsPrice.ProductPrice;

                if (id != products.ProductId)
                {
                    return BadRequest();
                }

                _context.Entry(products).State = EntityState.Modified;

                try
                {
                    var PriceLog = new PriceLog
                    {
                        Productid = products.ProductId,
                        NewPrice = products.ProductPrice,
                        Pricechangedate = DateTime.UtcNow,
                        UserId = LogedUser
                    };
                    _context.PriceLog.Add(PriceLog);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(id))
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
            return NotFound();
        }

        // POST: api/Products
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Products>> PostProducts(Products products)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            bool canContinue = await IdentifyAdminLoginAsync(identity);

            if (canContinue)
            {
                _context.Products.Add(products);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetProducts", new { id = products.ProductId }, products);
            }
            return NotFound();
        }

        // DELETE: api/Products/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Products>> DeleteProducts(int id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            bool canContinue = await IdentifyAdminLoginAsync(identity);

            if (canContinue)
            {
                var products = await _context.Products.FindAsync(id);
                if (products == null)
                {
                    return NotFound();
                }

                _context.Products.Remove(products);
                await _context.SaveChangesAsync();
                return products;
            }
            return NotFound();
        }

        private bool ProductsExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
        private async Task<bool> IdentifyAdminLoginAsync(ClaimsIdentity claimsIdentity)
        {
            var user = await _userManager.FindByNameAsync(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value.ToString());
            return await _userManager.IsInRoleAsync(user, "Admin");
        }
    }
}
