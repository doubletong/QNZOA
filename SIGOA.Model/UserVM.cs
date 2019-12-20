using SIGOA.Data.Enums;
using SIGOA.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SIGOA.Model
{

    public class UserVM
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string RealName { get; set; }
        public DateTime? Birthday { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastActivityDate { get; set; }
        public string PhotoUrl { get; set; }
        public string Qq { get; set; }
        public DateTime CreateDate { get; set; }
        public string GenderText
        {
            get
            {
                return Gender.GetDescription();
            }
        }
    }

 

    public class UserPagedVM
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
        public string Keywords { get; set; }
        public IEnumerable<UserVM> ItemList { get; set; }

        public int LastPageIndex
        {
            get
            {
                if (PageSize > 0)
                {
                    var d = RowCount / PageSize;
                    return (int)Math.Floor((double)d);
                }
                return 0;
            }
        }
    }
    public class RegisterIM
    {

        [Required(ErrorMessage = "请输入用户名")]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        //public string DisplayName { get; set; }    
        //[Required]
        //[RegularExpression("^1\\d{10}$", ErrorMessage = "手机格式不正确")]
        //public string Mobile { get; set; }

        [Required(ErrorMessage = "请输入密码")]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [Display(Name = "密码")]
        public string Password { get; set; }
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "新密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }

      
        [EmailAddress(ErrorMessage ="邮箱格式不正确")]
        [Required(ErrorMessage ="请输入邮箱地址")]
        [Display(Name = "邮箱")]
        public string Email { get; set; }
    }
    public class UserIM
    {
       
        public Guid Id { get; set; }
        [Required]
        public string Username { get; set; }
        public string RealName { get; set; }
        public DateTime Birthday { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public bool IsActive { get; set; }
        public string PhotoUrl { get; set; }
        public string Qq { get; set; }
     


    }
    public class SetPasswordIM
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "新密码")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认新密码")]
        [Compare("NewPassword", ErrorMessage = "新密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }
    }

    public class SetRolesIM
    {
        [Required]
        public Guid UserId { get; set; }
        public List<SelectVM> Roles { get; set; }
    }

    public class UserForSelectVM
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string RealName { get; set; }      
        public bool IsActive { get; set; }
        public string DisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(RealName))
                {
                    return IsActive ? Username : $"{Username} [锁定]" ;
                }
                   
                return IsActive ? $"{RealName}[{Username}]": $"{RealName}[{Username}][锁定]";
            }
        }
    }
}
