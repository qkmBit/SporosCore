using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SporosCore.Models
{
    public class AdditionalOption
    {
        public int AdditionalOptionId { get; set; }
        public string Name { get; set; }
        public int Percent { get; set; }
        public virtual ICollection<OrderItems> OrderItems { get; set; }

    }
}
