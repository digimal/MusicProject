using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Bll.ViewModels.Artist
{
    public class ArtistBaseViewModel : Common.BaseViewModel
    {
        public int? PictureId { get; set; }
    }
}
