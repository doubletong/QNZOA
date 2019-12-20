using System;
using System.Collections.Generic;

namespace SIGOA.Data
{
    public partial class MenuCategory
    {
        public MenuCategory()
        {
            Menus = new HashSet<Menu>();
        }

        public int Id { get; set; }
        public bool Active { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Importance { get; set; }
        public bool IsSys { get; set; }
        public string Title { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public ICollection<Menu> Menus { get; set; }
    }
}
