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
        public Orders()
        {
            this.OrderItems = new HashSet<OrderItems>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public System.DateTime OrderDate { get; set; }
        public Nullable<bool> Payed { get; set; }


        public virtual ICollection<OrderItems> OrderItems { get; set; }
        public virtual Users User { get; set; }
    }
}
