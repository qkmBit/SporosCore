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
                stores.Add(vm);
            }
            return View("~/Pages/Store/Store.cshtml", stores);
        }
        public ActionResult OpenAuthWindow()
        {
            return PartialView("~/Pages/Account/LoginPartial.cshtml");
        }
    }
}
