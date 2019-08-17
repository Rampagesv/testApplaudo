using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace testApplaudo.Models
{
    public class Products
    {
        [Key]
        public int ProductId { get; set; }
        public string ProductSKU { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int inStock { get; set; }
        public ICollection<Stock> Stock { get; set; }

    }
}
