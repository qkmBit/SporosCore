using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SporosCore.Models
{
    public partial class GrowthType
    {
        public GrowthType()
        {
            this.Items = new HashSet<Items>();
        }

        public int GrowthTypeId { get; set; }
        public string GrowthTypeName { get; set; }

        public virtual ICollection<Items> Items { get; set; }
    }
}
