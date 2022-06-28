using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SporosCore.Models
{
    public partial class OrderItems
    {
        public int ItemId { get; set; }
        public int OrderId { get; set; }
        public int Count { get; set; }
        public int AdditionalOptionId
        {
            get; set;
        }
        public virtual AdditionalOption AdditionalOption { get; set; }
        public virtual Items Items { get; set; }
        public virtual Orders Orders { get; set; }
    }
}
