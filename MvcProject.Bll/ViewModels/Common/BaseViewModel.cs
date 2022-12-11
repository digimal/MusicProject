using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MvcProject.Bll.ViewModels.Common
{
    public abstract class BaseViewModel
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Name field is obligatory.")]
        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}
