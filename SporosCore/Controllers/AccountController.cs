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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
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

                    await context.SaveChangesAsync();
                    await _signInManager.SignInAsync(user, false);

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
            var orders = context.Orders.Where(d => d.UserId == user.Id);
            List<OrderItems> orderItems = new List<OrderItems>();
            List<Items> items = new List<Items>();
            var addresses = context.Address.Where(d => d.UserId == user.Id);
            foreach(var order in orders)
            {
                foreach (var item in context.OrderItems.Where(i => i.OrderId == order.OrderId))
                {
                    orderItems.Add(item);
                    items.Add(context.Items.Where(it => it.ItemId == item.ItemId).FirstOrDefault());
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
    }
}
