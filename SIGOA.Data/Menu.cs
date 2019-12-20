using SIGOA.Data.Enums;
using System;
using System.Collections.Generic;

namespace SIGOA.Data
{
    public partial class Menu
    {
        public Menu()
        {
            InverseParent = new HashSet<Menu>();
            RoleMenus = new HashSet<RoleMenu>();
        }

        public int Id { get; set; }
        public string Action { get; set; }
        public bool Active { get; set; }
        public string Area { get; set; }
        public int CategoryId { get; set; }
        public string Controller { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Iconfont { get; set; }
        public int Importance { get; set; }
        public bool IsExpand { get; set; }
        public int? LayoutLevel { get; set; }
        public MenuType MenuType { get; set; }
        public int? ParentId { get; set; }
        public string Title { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Url { get; set; }

        public MenuCategory Category { get; set; }
        public Menu Parent { get; set; }
        public ICollection<Menu> InverseParent { get; set; }
        public ICollection<RoleMenu> RoleMenus { get; set; }
    }
}
