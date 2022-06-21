using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SporosCore.Models
{
    public partial class News
    {
        public int NewsId { get; set; }
        public Nullable<System.DateTime> NewsDate { get; set; }
        public string PagePath { get; set; }
    }
}
