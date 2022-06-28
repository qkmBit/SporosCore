using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SporosCore.Models
{
    public partial class Orders
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public Nullable<bool> Payed { get; set; }

        public virtual ICollection<OrderItems> OrderItems { get; set; }
        public virtual Users User { get; set; }
    }
}
