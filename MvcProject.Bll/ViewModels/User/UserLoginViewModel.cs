using System.ComponentModel.DataAnnotations;

namespace MvcProject.Bll.ViewModels.User
{
    public class UserLoginViewModel
    {
        [Required(ErrorMessage = "Please, enter your e-mail.")]
        [Display(Name = "E-mail")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please, enter your password.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
