using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace testApplaudo.Models
{
    public class Purchase
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PurchaseId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public int PurchaseTotal { get; set; }
        // daniel brito: I was thingking to add a multiple product shoping card, 
        // but is there is time I will Implement is, for now it will be commented.
        // to make a single producto purchase.
        // public ICollection<PurchaseDetails> PurchaseDetails { get; set; }
    }
}
