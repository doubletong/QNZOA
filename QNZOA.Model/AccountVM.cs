using QNZOA.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace QNZOA.Model
{
    public class LoginIM
    {
        [Display(ResourceType = typeof(Labels), Name = "Username")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string Username { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "Password")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Validations))]
        public string Password { get; set; }

        public bool IsValidate { get; set; }

    }


    public class UserLoginVM
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string RealName { get; set; }
        public string Email { get; set; }
        public string PhotoUrl { get; set; }
        public string Token { get; set; }
    }

    public class UserForSelectVM
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string RealName { get; set; }
        public string DisplayName
        {
            get
            {
                if (!string.IsNullOrEmpty(RealName))
                    return $"{RealName} 【{Username}】";
                return Username;
            }
        }
    }
}
