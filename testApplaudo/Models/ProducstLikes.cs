using System;
using System.ComponentModel.DataAnnotations;

namespace testApplaudo.Models
{
    public class ProductLikes
    {
        [Key]
        public long LikeId { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }
        // I wanted to add the customer that make the like, but sinse i'm not
        // I'm not using the customer table anymore in favor of the Token. 
        // maybe I can ad it later
        public DateTime DateLiked { get; set; }

    }
}
