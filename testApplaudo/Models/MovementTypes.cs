using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace testApplaudo.Models
{
    public class MovementTypes
    {
        [Key]
        public int MovementTypeId { get; set; }
        public string MovementName { get; set; }
        public ICollection<Stock> Stock { get; set; }
    }
}
