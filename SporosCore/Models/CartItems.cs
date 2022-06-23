using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SporosCore.Models
{
    public class CartItems
    {
        public CartItems()
        {
        }
        public int CartId { get; set; }
        public int ItemId { get; set; }
        public int Count { get; set; }
        public virtual Items Items { get; set; }
        public virtual Cart Cart { get; set; }
    }
}
