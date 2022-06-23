using Microsoft.AspNetCore.Mvc;
using SporosCore.Data;
using SporosCore.Models.ViewModels;
using SporosCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SporosCore.Controllers
{
    public class StoreController : Controller
    {
        private readonly ApplicationDbContext context;
        public StoreController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Item(int id)
        {
            StoreViewModel model = new StoreViewModel(context);
            model.getItem(id);
            return View("~/Pages/Store/Item.cshtml",model);
        }
    }
}
