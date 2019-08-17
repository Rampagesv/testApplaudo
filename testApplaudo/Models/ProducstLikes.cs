using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace testApplaudo.Models
{
    public class ProductLikes
    {
        [Key]
        public int ProductId { get; set; }
        public int Likes { get; set; }
        // I whanted to add the customer that make the like, but sinse i'm not
        // I'm not using the customer table anymore in favor of the Token. 
        // maybe I can ad it later
        //ICollection<Customer> Customer { get; set; }
    }
}
