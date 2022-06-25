using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SporosCore.Models
{
    public partial class Items
    {
        public Items()
        {
            this.ItemAdvantages = new HashSet<ItemAdvantages>();
            this.OrderItems = new HashSet<OrderItems>();
            this.CartItems = new HashSet<CartItems>();
        }

        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public string PicPath { get; set; }
        public string Originator { get; set; }
        public string ProteinContent { get; set; }
        public string Temperature { get; set; }
        public string OilContent { get; set; }
        public string LysineContent { get; set; }
        public string About { get; set; }
        public Nullable<int> MaturationGroupId { get; set; }
        public string GrowingSeason { get; set; }
        public string SeedingRate { get; set; }
        public string MaxYield { get; set; }
        public string PlantsHeight { get; set; }
        public Nullable<int> GrowthTypeId { get; set; }
        public string Branching { get; set; }
        public string Coloring { get; set; }
        public string Tillering { get; set; }
        public string SpikeletsCount { get; set; }
        public string GrainsCount { get; set; }
        public string Filminess { get; set; }
        public string GrainNature { get; set; }
        public string DryMatter { get; set; }
        public string PotentialProductivity { get; set; }
        public string BgPicPath { get; set; }
        public int Circle1 { get; set; }
        public int Circle2 { get; set; }
        public int Circle3 { get; set; }
        public string InfoPic1 { get; set; }
        public string InfoPic2 { get; set; }
        public string InfoPic3 { get; set; }
        public int Price { get; set; }

        public virtual ICollection<CartItems> CartItems { get; set; }
        public virtual Category Category { get; set; }
        public virtual GrowthType GrowthType { get; set; }
        public virtual ICollection<ItemAdvantages> ItemAdvantages { get; set; }
        public virtual MaturationGroup MaturationGroup { get; set; }
        public virtual ICollection<OrderItems> OrderItems { get; set; }
    }
}
