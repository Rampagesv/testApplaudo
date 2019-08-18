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
using testApplaudo.Others;

namespace testApplaudo.Controllers
{
    [SwaggerTag("Requests about values")]
    [Route("api/[controller]")]
    [ApiController]

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

        // Get: api/Products/Search
        [HttpGet("Search/{ProductName}")]
        public async Task<ActionResult<Products>> GetProducts(string ProductName)
        {

            var ProductList = await _context.Products.ToListAsync();

            ProductsOutputModel SearchbyName = new ProductsOutputModel
            {
                      Items = ProductList.Where(m => m.ProductName.ToUpper().Contains(ProductName.ToUpper())).Select(m => ToProducInfo(m)).ToList() 
            };
            return Ok(SearchbyName);

        }

        // api/GetProducts/
        [HttpGet("Sort")]
        public async Task<IActionResult> GetProductSort(string SortBy)
        {
            // DOTO: find a better sotution for this... need to read more
            // I don't likes this quick soluction
            var ProductList = await _context.Products.ToListAsync();
            switch (SortBy)
            {
                case "ProductSKU":
                    ProductsOutputModel SortProductSKU = new ProductsOutputModel
                    {
                        Items = ProductList.Select(m => ToProducInfo(m)).OrderBy(x => x.ProductSKU).ToList()
                    };
                    return Ok(SortProductSKU);
                case "ProductPrice":
                    ProductsOutputModel SortProductPrice = new ProductsOutputModel
                    {
                        Items = ProductList.Select(m => ToProducInfo(m)).OrderBy(x => x.ProductPrice).ToList()
                    };
                    return Ok(SortProductPrice);
                case "inStock":
                    ProductsOutputModel SortinStock = new ProductsOutputModel
                    {
                        Items = ProductList.Select(m => ToProducInfo(m)).OrderBy(x => x.inStock).ToList()
                    };
                    return Ok(SortinStock);
                case "ProductLikes":
                    ProductsOutputModel SortProductLikes = new ProductsOutputModel
                    {
                        Items = ProductList.Select(m => ToProducInfo(m)).OrderByDescending(x => x.ProductLikes).ToList()
                    };
                    return Ok(SortProductLikes);
                case "ProductName":
                default:
                    ProductsOutputModel SortProductName = new ProductsOutputModel
                    {
                        Items = ProductList.Select(m => ToProducInfo(m)).OrderBy(x => x.ProductName).ToList()
                    };
                    return Ok(SortProductName);
            }

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
                // I was thinking of leaving the Stock Insert in a separeted API Resp
                // but lets say that the bussines logic demands to set an inictial stock
                // wend a product is created, even if is zero :p
                var stock = new Stock
                {
                    PurchaseId = 0,
                    ProductID = products.ProductId,
                    MovementTypeid = 1,
                    MovementDate = DateTime.UtcNow,
                    MovementQuantity = products.inStock,
                    ProductQuantity = products.inStock
                };
                _context.Stock.Add(stock);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetProducts", new { id = products.ProductId }, products);
            }
            return NotFound();
        }

        [Authorize]
        [HttpPut("Price/{id}")]
        public async Task<IActionResult> PutProductsPrice(int id, decimal productsPrice)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            bool canContinue = await IdentifyAdminLoginAsync(identity);

            if (canContinue)
            {

                LogedUser = identity.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();

                var products = await _context.Products.FindAsync(id);
                products.ProductPrice = productsPrice;

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

        [Authorize]
        [HttpPut("QuantityAjust/{id}")]
        public async Task<IActionResult> PutQuantityAjust(int id, int NewQuantity)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            bool canContinue = await IdentifyAdminLoginAsync(identity);

            if (canContinue)
            {
                try
                {
                    LogedUser = identity.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();

                    var products = await _context.Products.FindAsync(id);

                    if (id != products.ProductId)
                    {
                        return BadRequest();
                    }
                    products.inStock = NewQuantity;

                    Stock stock = new Stock
                    {
                        PurchaseId = 0,
                        ProductID = products.ProductId,
                        MovementTypeid = 4,
                        MovementDate = DateTime.UtcNow,
                        MovementQuantity = products.inStock,
                        ProductQuantity = products.inStock
                    };

                    _context.Entry(products).State = EntityState.Modified;
                    _context.Stock.Add(stock);
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


        private bool ProductsExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
        private async Task<bool> IdentifyAdminLoginAsync(ClaimsIdentity claimsIdentity)
        {
            var user = await _userManager.FindByNameAsync(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value.ToString());
            return await _userManager.IsInRoleAsync(user, "Admin");
        }
        private Products ToProducInfo(Products model)
        {
            return new Products
            {
                ProductId = model.ProductId,
                ProductSKU = model.ProductSKU,
                ProductName = model.ProductName,
                ProductPrice = model.ProductPrice,
                inStock = model.inStock,
                ProductLikes = model.ProductLikes
            };
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

    }


}
