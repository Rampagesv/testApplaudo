using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace testApplaudo.Models
{
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }
        public int CustomerTipe { get; set; }
        public string CustomerLogin { get; set; }
        public string CustomerPass { get; set; }

    }
}
