using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SporosCore.Data;
using SporosCore.Models;
using SporosCore.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace SporosCore.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        private readonly ApplicationDbContext context;

        public AdminController(UserManager<Users> userManager, SignInManager<Users> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.context = context;
        }
        public IActionResult Employees()
        {
            List<EmployeesViewModel> model = new List<EmployeesViewModel>();
            var users = context.Users.ToList();
            foreach(var u in users)
            {
                EmployeesViewModel model1 = new EmployeesViewModel();
                model1.User = u;
                if(_userManager.IsInRoleAsync(u, "admin").Result)
                {
                    model1.Role = "admin";
                }
                else if (_userManager.IsInRoleAsync(u, "employee").Result)
                {
                    model1.Role = "employee";
                }
                else
                {
                    model1.Role = "user";
                }
                model.Add(model1);
            }
            return View("~/Pages/Admin/Employees.cshtml",model);
        }
        public IActionResult EmployeesInRole(bool admin, bool employee, bool user)
        {
            List<EmployeesViewModel> model = new List<EmployeesViewModel>();
            var users = context.Users.ToList();
            foreach (var u in users)
            {
                EmployeesViewModel model1 = new EmployeesViewModel();
                model1.User = u;
                if (_userManager.IsInRoleAsync(u, "admin").Result)
                {
                    if (admin)
                    {
                        model1.Role = "admin";
                        model.Add(model1);
                    }
                }
                else if (_userManager.IsInRoleAsync(u, "employee").Result)
                {
                    if (employee)
                    {
                        model1.Role = "employee";
                        model.Add(model1);
                    }
                }
                else if(user)
                {
                    model1.Role = "user";
                    model.Add(model1);
                }
            }
            return PartialView("~/Pages/Admin/EmployeesInRolePartial.cshtml", model);
        }
        public async Task<IActionResult> ChangeRole(string userId, string role)
        {
            var user = _userManager.FindByIdAsync(userId).Result;
            switch (role)
            {
                case "user":
                    var roles = _userManager.GetRolesAsync(user).Result.ToList();
                    await _userManager.RemoveFromRolesAsync(user, roles);
                    break;
                case "employee":
                    if (_userManager.IsInRoleAsync(user, "admin").Result) await _userManager.RemoveFromRoleAsync(user, "admin");
                    else await _userManager.AddToRoleAsync(user, "employee");
                    break;
                case "admin":
                    await _userManager.AddToRoleAsync(user, "admin");
                    await _userManager.AddToRoleAsync(user, "employee");
                    break;
                default: return Content("Error");
            }
            return Content("Ok");
        }
        public IActionResult Spravochnik()
        {
            return View();
        }
    }
}
