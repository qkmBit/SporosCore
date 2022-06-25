using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SporosCore.Models;
using SporosCore.Data;

namespace SporosCore.Models.ViewModels
{
    public class CartViewModel
    {
        public List<Items> Items = new List<Items>();
        public List<CartItems> CartItems { get; set; }
        public List<Category> Categories = new List<Category>();
    }
}
