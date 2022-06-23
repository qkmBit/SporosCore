using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SporosCore.Models
{
    public class Cart
    {
        public Cart()
        {
            this.CartItems = new HashSet<CartItems>();
        }
        public int CartId { get; set; }
        public string UserId { get; set; }
        public virtual Users Users { get; set; }
        public virtual ICollection<CartItems> CartItems { get; set; }
    }
}
