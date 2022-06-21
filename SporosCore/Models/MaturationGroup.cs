using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SporosCore.Models
{
    public partial class MaturationGroup
    {
        public MaturationGroup()
        {
            this.Items = new HashSet<Items>();
        }

        public int MaturationGroupId { get; set; }
        public string MaturationGroupName { get; set; }

        public virtual ICollection<Items> Items { get; set; }
    }
}
