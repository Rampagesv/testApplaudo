using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testApplaudo.Models
{
    public class PurchaseDetails
    {
        public int PurchaseDetailsId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
   
    }
}
