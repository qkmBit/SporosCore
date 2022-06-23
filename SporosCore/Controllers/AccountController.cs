using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using SporosCore.Data;
using SporosCore.Models;
using SporosCore.Models.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Security.Claims;

namespace SporosCore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        ApplicationDbContext context;

        public AccountController(UserManager<Users> userManager, SignInManager<Users> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    var user = _userManager.FindByEmailAsync(model.Email).Result;
                    var cart = context.Cart.Where(o => o.UserId == user.Id);
                    var cartItems = context.CartItems.Where(oi => oi.CartId == cart.FirstOrDefault().CartId);
                    var count = cartItems.Count().ToString();
                    await _userManager.AddClaimAsync(user, new Claim("CartCount", count));
                    var claims = User.Claims.ToList();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            else 
            { 
                foreach(var ms in ModelState.Values)
                {
                    foreach(var error in ms.Errors)
                    {
                        ModelState.AddModelError("",error.ErrorMessage);
                    }
                }
            }
            return PartialView("~/Pages/Account/LoginPartial.cshtml", model);
        }
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View("~/Pages/Account/Register.cshtml");
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Users { UserName = model.Email, Email = model.Email };
                if (model.FirstName != null) user.FirstName = model.FirstName;
                if (model.SecondName != null) user.SecondName = model.SecondName;
                if (model.Patronymic != null) user.Patronymic = model.Patronymic;
                if (model.CompanyName != null) user.CompanyName = model.CompanyName;
                if (model.PhoneNumber != null) user.PhoneNumber = model.PhoneNumber;
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (model.City != null && model.Address != null) user.Address.Add(new Address { City = model.City, Address1 = model.Address, UserId = user.Id });

                    Cart сart = new Cart() { UserId = user.Id };
                    await context.SaveChangesAsync();
                    await _signInManager.SignInAsync(user, false);
                    await _userManager.AddClaimAsync(user, new Claim("CartCount", "0"));

                    // Дополнительные сведения о включении подтверждения учетной записи и сброса пароля см. на странице https://go.microsoft.com/fwlink/?LinkID=320771.
                    // Отправка сообщения электронной почты с этой ссылкой
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Подтверждение учетной записи", "Подтвердите вашу учетную запись, щелкнув <a href=\"" + callbackUrl + "\">здесь</a>");

                    return RedirectToAction("Index", "Home");
                }
                foreach (var ms in ModelState.Values)
                {
                    foreach (var error in ms.Errors)
                    {
                        ModelState.AddModelError("", error.ErrorMessage);
                    }
                }
            }

            // Появление этого сообщения означает наличие ошибки; повторное отображение формы
            return View("~/Pages/Account/Register.cshtml",model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public IActionResult Profile()
        {
            Users user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var orders = context.Orders.Where(d => d.UserId == user.Id).ToList();
            List<OrderItems> orderItems = new List<OrderItems>();
            List<Items> items = new List<Items>();
            List<Category> categories = new List<Category>();
            var addresses = context.Address.Where(d => d.UserId == user.Id);
            foreach(var order in orders)
            {
                var orderItemsTemp = context.OrderItems.Where(i => i.OrderId == order.OrderId).ToList();
                foreach (var item in orderItemsTemp)
                {
                    orderItems.Add(item);
                    var fic = context.Items.Where(it => it.ItemId == item.ItemId).FirstOrDefault();
                    items.Add(fic);
                    categories.Add(context.Category.Where(c => c.CategoryId == fic.CategoryId).FirstOrDefault());
                }
            }
            ProfileViewModel model = new ProfileViewModel { Email = user.Email };
            if (user.FirstName != null) model.FirstName = user.FirstName;
            if (user.SecondName != null) model.SecondName = user.SecondName;
            if (user.Patronymic != null) model.Patronymic = user.Patronymic;
            if (user.PhoneNumber != null) model.PhoneNumber = user.PhoneNumber;
            if (user.CompanyName != null) model.CompanyName = user.CompanyName;
            if (addresses != null) model.Addresses = addresses.ToList();
            if (user.Orders != null) 
            { 
                model.Orders = orders.ToList();
                model.OrderItems = orderItems;
                model.Items = items;
                model.Categories = categories;
            }
            return View("~/Pages/Account/Profile.cshtml", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Account/GetAddress")]
        public string GetAddress([FromForm]string id)
        {
            var address = context.Address.Where(i=>i.AddressId==int.Parse(id)).FirstOrDefault();
            return JsonConvert.SerializeObject(address);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Account/SaveProfile")]
        public  IActionResult SaveProfile(ProfileViewModel model)
        {
            Users user = _userManager.FindByEmailAsync(User.Identity.Name).Result;
            var addrs = context.Address.Where(u => u.UserId == user.Id).ToList();
            user.CompanyName = model.CompanyName;
            user.FirstName = model.FirstName;
            user.Patronymic = model.Patronymic;
            user.PhoneNumber = model.PhoneNumber;
            user.SecondName = model.SecondName;
            user.Email = model.Email;
            user.UserName = model.Email;
            user.Address = (ICollection<Address>)addrs;
            var result = _userManager.UpdateAsync(user).Result;
            if (result.Succeeded)
            {
                _signInManager.RefreshSignInAsync(user);
                if (model.AddressAdd && model.City != null && model.Address != null)
                {
                    Address address = new Address { City = model.City, Address1 = model.Address, UserId = user.Id };
                    var adrressRes = context.Address.AddAsync(address).IsCompletedSuccessfully;
                    if (!adrressRes)
                    {
                        ModelState.AddModelError("", "Адрес не был добавлен");
                        return View("~/Pages/Account/Profile.cshtml", model);
                    }
                }
                else if (model.City != null && model.Address != null)
                {
                    var addr = user.Address.Where(a => a.AddressId == model.AddressId).FirstOrDefault();
                    addr.Address1 = model.Address;
                    addr.City = model.City;
                    context.Address.Update(addr);
                }
            }
            else
            {
                ModelState.AddModelError("", "Не удалось обновить пользователя.");
                return View("~/Pages/Account/Profile.cshtml", model);
            }
            context.SaveChanges();
            return RedirectToAction("Profile", "Account");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Account/DeleteAddress")]
        public IActionResult DeleteAddress([FromForm]int id)
        {
            var address = context.Address.Where(a => a.AddressId == id).FirstOrDefault();
            var result = context.Address.Remove(address);
            context.SaveChanges();
            return RedirectToAction("Profile", "Account");
        }
    }
}
