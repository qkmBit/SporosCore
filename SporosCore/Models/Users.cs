using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace SporosCore.Models
{
    public partial class Users : IdentityUser
    {
        public Users()
        {
            this.Address = new HashSet<Address>();
            this.Orders = new HashSet<Orders>();
        }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Patronymic { get; set; }
        public string CompanyName { get; set; }

        public virtual ICollection<Address> Address { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }
    }
}
