using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Web.Mvc;
using System.Security;

namespace MyBlog.Models
{
    public class UserModel
    {
       [Required(ErrorMessage="Email is required")]
        [StringLength (150)]
        [DataType(DataType.EmailAddress)]
        [Display(Name="Email Address:")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength=6)]
        [Display(Name = "Password:")]
        public string Password { get; set; }

    }
}