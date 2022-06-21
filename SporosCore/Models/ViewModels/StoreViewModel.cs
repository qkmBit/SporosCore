using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SporosCore.Data;

namespace SporosCore.Models.ViewModels
{
    public class StoreViewModel
    {
        private readonly ApplicationDbContext context;
        public Items Item { get; set; }
        public List<ItemAdvantages> Advantages { get; set; }
        public MaturationGroup MaturationGroup { get; set; }
        public List<Category> Categories { get; set; }
        public Category Category { get; set; }
        public GrowthType GrowthType { get; set; }
        public StoreViewModel(ApplicationDbContext context)
        {
            this.context = context;
        }
        public void get(int id)
        {
            Item = context.Items.Where(p => p.ItemId == id).FirstOrDefault();
            Category = context.Category.Where(c => c.CategoryId == Item.CategoryId).FirstOrDefault();
            Categories = context.Category.ToList();
        }
    }
}
