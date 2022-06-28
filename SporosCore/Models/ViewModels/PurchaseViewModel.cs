using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SporosCore.Models.ViewModels
{
    public class PurchaseViewModel
    {
        public Users User { get; set; }
        public List<Address> Addresses { get; set; }
        public Cart Cart { get; set; }
        public List<Category> Categories { get; set; }
        public List<ItemInPurchase> CartItems { get; set; }
        public List<AdditionalOption> AdditionalOptions { get; set; }
        public List<Items> Items { get; set; }
        public int Price { get; set; }
    }
    public class ItemInPurchase
    {
        public CartItems CartItem { get; set; }
        public int Price { get; set; }
    }
    public class OrderNowViewModel
    {
        public int ItemId { get; set; }
        public int Count { get; set; }
        public int OptionId { get; set; }
    }
    public class ThanksViewModel
    {
        public int OrderId { get; set; }
    }
}
