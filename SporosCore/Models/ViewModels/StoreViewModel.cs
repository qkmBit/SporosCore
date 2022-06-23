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
        public List<Items> Items { get; set; }
        public List<ItemAdvantages> Advantages { get; set; }
        public MaturationGroup MaturationGroup { get; set; }
        public List<Category> Categories { get; set; }
        public Category Category { get; set; }
        public GrowthType GrowthType { get; set; }
        public StoreViewModel(ApplicationDbContext context)
        {
            this.context = context;
        }
        public string CategoryName(string name)
        {
            switch (name)
            {
                case "Соя": return "сои";
                case "Пшеница": return "пшеницы";
                case "Овёс": return "овса";
                default: return "";
            }
            
        }
        public void getItem(int id)
        {
            Item = context.Items.Where(p => p.ItemId == id).FirstOrDefault();
            Category = context.Category.Where(c => c.CategoryId == Item.CategoryId).FirstOrDefault();
            Items = context.Items.Where(c => c.CategoryId == Category.CategoryId).ToList();
            Categories = context.Category.ToList();
            Advantages = context.ItemAdvantages.Where(i => i.ItemId == Item.ItemId).ToList();
            MaturationGroup = context.MaturationGroup.Where(i => i.MaturationGroupId == Item.MaturationGroupId).FirstOrDefault();
            GrowthType = context.GrowthType.Where(i => i.GrowthTypeId == Item.GrowthTypeId).FirstOrDefault();
        }
        public void get(int id)
        {
            Item = context.Items.Where(p => p.ItemId == id).FirstOrDefault();
            Category = context.Category.Where(c => c.CategoryId == Item.CategoryId).FirstOrDefault();
            Categories = context.Category.ToList();
        }
    }
}
