using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace testApplaudo.Models
{
    public class Stock
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MovementId { get; set; }
        public int ProductID { get; set; }
     
        public int PurchaseId { get; set; }
        public int MovementTypeid { get; set; }
        public DateTime MovementDate { get; set; }
        public int MovementQuantity { get; set; }
        public int ProductQuantity { get; set; }

    }
}
