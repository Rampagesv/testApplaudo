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
        public async Task<IActionResult> GetPriceLog(int Productid)
        {
            // var priceLog = await _context.PriceLog.FindAsync(Productid);
            var priceLogList = await _context.PriceLog.ToListAsync();


            PriceLogOutputModel priceLog = new PriceLogOutputModel
            {
                Items = priceLogList.Where(m => m.Productid == Productid).Select(m => ToPricelogInfo(m)).ToList()
            };
            return Ok(priceLog);
        }
        private PriceLog ToPricelogInfo(PriceLog model)
        {
            return new PriceLog
            {
                PriceLogid = model.PriceLogid,
                Productid = model.Productid,
                NewPrice = model.NewPrice,
                UserId = model.UserId,
                Pricechangedate = model.Pricechangedate
            };
        }
        private bool PriceLogExists(int id)
        {
            return _context.PriceLog.Any(e => e.PriceLogid == id);
        }
    }
}
