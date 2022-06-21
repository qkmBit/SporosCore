using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SporosCore.Models
{
    public partial class Category
    {
        public Category()
        {
            this.Items = new HashSet<Items>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public virtual ICollection<Items> Items { get; set; }
    }
}
