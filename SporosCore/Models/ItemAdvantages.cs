using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SporosCore.Models
{
    public partial class ItemAdvantages
    {
        public int ItemId { get; set; }
        public string Advantage { get; set; }

        public virtual Items Items { get; set; }
    }
}
