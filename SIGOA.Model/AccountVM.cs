using SIGOA.Infrastructure.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SIGOA.Model
{
    public class LoginIM
    {
        [Required]
        public string Username { get; set; }
        [Required]
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

}
