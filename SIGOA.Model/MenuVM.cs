using SIGOA.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SIGOA.Model
{
    public class MenuCategoryVM
    {
       
        public int Id { get; set; }
        public bool Active { get; set; }
        public int Importance { get; set; }
        public bool IsSys { get; set; }
        public string Title { get; set; }
 
        public ICollection<MenuVM> Menus { get; set; }
    }

    public class MenuVM
    {

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
        public MenuCategoryVM Category { get; set; }
        public MenuVM Parent { get; set; }
        public ICollection<MenuVM> InverseParent { get; set; }
  
    }

    public class MenuIM
    {
  

        public int Id { get; set; }
        [Display(Name = "菜单名称", Prompt = "菜单名称")]
        [Required(ErrorMessage = "请输入菜单名称")]
        public string Title { get; set; }
        [Display(Name = "链接地址", Prompt = "链接地址")]
        public string Url { get; set; }

        [Display(Name = "控制器")]
        public string Controller { get; set; }
        [Display(Name = "操作")]
        public string Action { get; set; }

        [Display(Name = "类型", Prompt = "类型")]
        public MenuType MenuType { get; set; }

        [Display(Name = "图标")]
        public string Iconfont { get; set; }

        [Display(Name = "排序", Prompt = "排序")]
        [Required(ErrorMessage = "请输入排序")]

        public int Importance { get; set; }

        [Display(Name = "激活", Prompt = "激活")]
        public bool Active { get; set; }

        [Display(Name = "父级菜单")]
        public int? ParentId { get; set; }
        public int CategoryId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class UpDownMoveIM
    {
        public int Id { get; set; }
        public bool IsUp { get; set; }
        
    }

    public class MoveMenuIM
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
    }


    public class RoleMenusIM
    {

        public int Id { get; set; }       
        public string Title { get; set; }
        public int Importance { get; set; }
        public int? ParentId { get; set; }
        public int CategoryId { get; set; }
        public RoleMenusIM Parent { get; set; }
        public ICollection<RoleMenusIM> InverseParent { get; set; }

        public bool IsSelected { get; set; }
    }

}
