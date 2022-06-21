using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SporosCore.Models
{
    public partial class Address
    {
        public int AddressId { get; set; }
        public string UserId { get; set; }
        public string City { get; set; }
        public string Address1 { get; set; }

        public virtual Users Users { get; set; }
    }
}
