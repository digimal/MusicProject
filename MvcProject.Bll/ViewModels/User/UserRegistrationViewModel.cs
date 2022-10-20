using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace MvcProject.Bll.ViewModels.User
{
    public class UserRegistrationViewModel
    {
        [Required(ErrorMessage = "Please, enter your e-mail.")]
        [Display(Name = "E-mail")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please, enter your login.")]
        [Display(Name = "Login")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please, enter your password.")]
        [DataType(DataType.Password)]
        //[MinLength(10)] [MaxLength(100)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Passwords mismatch!")]
        public string ConfirmPassword { get; set; }
    }
}
