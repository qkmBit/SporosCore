using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SporosCore.Data;
using SporosCore.Models;
using SporosCore.Models.ViewModels;

namespace SporosCore.Controllers
{
    public class HomeController:Controller
    {
        private readonly ApplicationDbContext context;
        public HomeController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            return View("~/Pages/Index.cshtml");
        }
        public IActionResult Store()
        {
            List<Items> items = new List<Items>();
            items = context.Items.ToList();
            List<StoreViewModel> stores = new List<StoreViewModel>();
            foreach (var item in items)
            {
                StoreViewModel vm = new StoreViewModel(context);
                vm.get(item.ItemId);
                vm.CategoryId = "All";
                stores.Add(vm);
            }
            return View("~/Pages/Store/Store.cshtml", stores);
        }
        [HttpPost]
        [Route("Home/ChangeStore")]
        public IActionResult ChangeStore([FromForm]string id)
        {
            List<Items> items = new List<Items>();
            if (id != "All")
            {
                items = context.Items.Where(i => i.CategoryId == int.Parse(id)).ToList();
            }
            else
            {
                items = context.Items.ToList();
            }
            List<StoreViewModel> stores = new List<StoreViewModel>();
            foreach (var item in items)
            {
                StoreViewModel vm = new StoreViewModel(context);
                vm.get(item.ItemId);
                vm.CategoryId = id;
                stores.Add(vm);
            }
            return PartialView("~/Pages/Store/Store.cshtml", stores);
        }
        public ActionResult OpenAuthWindow()
        {
            return PartialView("~/Pages/Account/LoginPartial.cshtml");
        }
        public ActionResult Cart()
        {
            CartViewModel model = new CartViewModel();
            var user = context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var cart = context.Cart.Where(c => c.UserId == user.Id).FirstOrDefault();
            var cartItems = context.CartItems.Where(ci => ci.CartId == cart.CartId).ToList();
            model.CartItems = cartItems;
            foreach (var item in cartItems)
            {
                var cartItem = context.Items.Where(i => i.ItemId == item.ItemId).FirstOrDefault();
                var category = context.Category.Where(c => c.CategoryId == cartItem.CategoryId).FirstOrDefault();
                model.Items.Add(cartItem);
                model.Categories.Add(category);
            }
            return PartialView("~/Pages/CartPartial.cshtml",model);
        }
    }
}
