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
    }
}
