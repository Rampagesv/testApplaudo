
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace testApplaudo.Models
{
    public class ProductsOutputModel
    {
        public List<Products> Items { get; set; }
    }   
    public class Products
    {
        [Key]
        public int ProductId { get; set; }
        public string ProductSKU { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int inStock { get; set; }
        public int ProductLikes { get; set; }
        //public ICollection<Stock> Stock { get; set; }

    }
}
