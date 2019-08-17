using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace testApplaudo.Models
{
    public class PriceLog
    {
        [Key]
        public int PriceLogid { get; set; }
        public int Productid { get; set; }
        public decimal NewPrice { get; set; }
        public string UserId { get; set; }
        public DateTime Pricechangedate { get; set; }
    }
}
