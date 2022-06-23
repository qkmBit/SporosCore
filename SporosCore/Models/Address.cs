using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SporosCore.Models
{
    public partial class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AddressId { get; set; }
        public string UserId { get; set; }
        public string City { get; set; }
        public string Address1 { get; set; }

        public virtual ICollection<Orders> Orders { get; set; }
        public virtual Users Users { get; set; }
    }
}
