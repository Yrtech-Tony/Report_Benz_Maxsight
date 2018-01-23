using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Report4Car.Models
{
    public class LoginModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} 不能为空")]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} 不能为空")]
        [Display(Name = "密    码")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}