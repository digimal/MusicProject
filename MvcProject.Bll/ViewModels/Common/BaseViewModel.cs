using System.ComponentModel.DataAnnotations;

namespace MvcProject.Bll.ViewModels.Common
{
    public abstract class BaseViewModel
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Name field is obligatory.")]
        public string Name { get; set; }
    }
}
