using Microsoft.AspNetCore.Mvc;
using SporosCore.Data;
using SporosCore.Models.ViewModels;
using SporosCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace SporosCore.Controllers
{
    public class StoreController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;

        public StoreController(UserManager<Users> userManager, SignInManager<Users> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.context = context;
        }
        public IActionResult Item(int id)
        {
            StoreViewModel model = new StoreViewModel(context);
            model.getItem(id);
            return View("~/Pages/Store/Item.cshtml",model);
        }
        public async Task<IActionResult> AddToCart(int id)
        {
            var item = context.Items.Where(i => i.ItemId == id).FirstOrDefault();
            var user = context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var cart = context.Cart.Where(c => c.UserId == user.Id).FirstOrDefault();
            var cartItems = context.CartItems.Where(ci => ci.CartId == cart.CartId).ToList();
            foreach(var ci in cartItems)
            {
                if (ci.ItemId == item.ItemId)
                {
                    ci.Count++;
                    context.Update(ci);
                    await context.SaveChangesAsync();
                    return Content((cartItems.Count).ToString());
                }
            }
            CartItems items = new CartItems();
            items.CartId = cart.CartId;
            items.ItemId = item.ItemId;
            items.Count = 1;
            await context.CartItems.AddAsync(items);
            await context.SaveChangesAsync();
            var claim = User.Claims.Where(c => c.Type == "CartCount").FirstOrDefault();
            await _userManager.ReplaceClaimAsync(user, claim, new Claim("CartCount", (cartItems.Count + 1).ToString()));
            await _signInManager.RefreshSignInAsync(user);
            return Content((cartItems.Count + 1).ToString());
        }

        public async Task<ActionResult> DeleteFromCart(int itemId, int cartId)
        {
            var item = context.CartItems.Where(i => i.ItemId == itemId && i.CartId == cartId).FirstOrDefault();
            var cartItems = context.CartItems.Where(i => i.CartId == cartId).ToList();
            var user = _userManager.FindByEmailAsync(User.Identity.Name).Result;
            context.CartItems.Remove(item);
            await context.SaveChangesAsync();
            var claim = User.Claims.Where(c => c.Type == "CartCount").FirstOrDefault();
            await _userManager.ReplaceClaimAsync(user, claim, new Claim("CartCount", (cartItems.Count - 1).ToString()));
            await _signInManager.RefreshSignInAsync(user);
            return RedirectToAction("Cart", "Home");
        }
        public async Task<ActionResult> DeleteAllFromCart(int cartId)
        {
            var cartItems = context.CartItems.Where(ci => ci.CartId == cartId);
            context.CartItems.RemoveRange(cartItems);
            var user = _userManager.FindByEmailAsync(User.Identity.Name).Result;
            await context.SaveChangesAsync();
            var claim = User.Claims.Where(c => c.Type == "CartCount").FirstOrDefault();
            await _userManager.ReplaceClaimAsync(user, claim, new Claim("CartCount", "0"));
            await _signInManager.RefreshSignInAsync(user);
            return Content("Ok");
        }
        public async Task<IActionResult> CartChangeCount([FromForm]List<CartItems> cartItems)
        {
            foreach(var cartItem in cartItems)
            {
                context.CartItems.Update(cartItem);
            }
            await context.SaveChangesAsync();
            return Content("Ok");
        }
        public IActionResult OpenPurchase()
        {
            PurchaseViewModel model = new PurchaseViewModel();
            var user = _userManager.FindByEmailAsync(User.Identity.Name).Result;
            model.User = user;
            var cart = context.Cart.Where(c => c.UserId == user.Id).FirstOrDefault();
            model.Cart = cart;
            var addresses = context.Address.Where(a => a.UserId == user.Id).ToList();
            model.Addresses = addresses;
            var cartItems = context.CartItems.Where(ci => ci.CartId == cart.CartId).ToList();
            List<ItemInPurchase> itemInPurchase = new List<ItemInPurchase>();
            var categories = context.Category.ToList();
            model.Categories = categories;
            var additionalOptions = context.AdditionalOption.ToList();
            model.AdditionalOptions = additionalOptions;
            List<Items> items = new List<Items>();
            int price = 0;
            foreach(var cartItem in cartItems)
            {
                ItemInPurchase inPurchase = new ItemInPurchase();
                var item = context.Items.Where(i => i.ItemId == cartItem.ItemId).FirstOrDefault();
                price += item.Price * cartItem.Count;
                int itemPrice = item.Price * cartItem.Count;
                items.Add(item);
                inPurchase.CartItem = cartItem;
                inPurchase.Price = itemPrice;
                itemInPurchase.Add(inPurchase);
            }
            model.CartItems = itemInPurchase;
            model.Price = price;
            model.Items = items;
            return View("~/Pages/Store/Purchase.cshtml", model);
        }
        public async Task<IActionResult> OrderNow([FromForm]List<OrderNowViewModel> model, string address, string city)
        {
            var user = _userManager.FindByEmailAsync(User.Identity.Name).Result;
            Orders Order = new Orders() { Address = address, City = city, OrderDate = DateTime.Now, UserId = user.Id };
            await context.Orders.AddAsync(Order);
            await context.SaveChangesAsync();
            var orderInDb = context.Orders.Where(o => o.Address == address && o.City == city && o.OrderDate == DateTime.Now && o.UserId==user.Id).FirstOrDefault();
            var userMessage = $"Здравствуйте, {user.Email}. Спасибо за оформление заказа в СПОРОС. <br>В вашем заказе: <br><ol>";
            foreach (var item in model)
            {
                OrderItems orderItem = new OrderItems();
                orderItem.ItemId = item.ItemId;
                orderItem.OrderId = Order.OrderId;
                orderItem.Count = item.Count;
                orderItem.AdditionalOptionId = item.OptionId;
                var itemName = context.Items.Where(i => i.ItemId == orderItem.ItemId).FirstOrDefault();
                var category = context.Category.Where(c => c.CategoryId == itemName.CategoryId).FirstOrDefault();
                userMessage += $"<li>{category.CategoryName} \"{itemName.ItemName}\" {orderItem.Count} кг.</li>";
                await context.OrderItems.AddAsync(orderItem);
            }
            userMessage += $"</ol>";
            var cart = context.Cart.Where(u => u.UserId == user.Id).FirstOrDefault();
            var cartItems = context.CartItems.Where(c => c.CartId == cart.CartId);
            var claim = User.Claims.Where(c => c.Type == "CartCount").FirstOrDefault();
            await _userManager.ReplaceClaimAsync(user, claim, new Claim("CartCount", "0"));
            await _signInManager.RefreshSignInAsync(user);
            context.CartItems.RemoveRange(cartItems);
            await context.SaveChangesAsync();
            ThanksViewModel viewModel = new ThanksViewModel();
            viewModel.OrderId = Order.OrderId;
            EmailService emailService = new EmailService();
            await emailService.SendEmailAsync(user.Email, "Спасибо за оформление заказа в СПОРОС!",userMessage);
            return View("~/Pages/Store/Ordered.cshtml", viewModel);
        }
    }
}
