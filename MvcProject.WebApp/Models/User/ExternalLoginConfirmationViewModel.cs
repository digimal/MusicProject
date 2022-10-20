using System.ComponentModel.DataAnnotations;

namespace MvcProject.WebApp.Models.User
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Требуется ввести фамилию.")]
        public string UserName { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }
}