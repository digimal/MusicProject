using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Bll.ViewModels.Artist
{
    public class GroupViewModel : ArtistViewModel
    {
        public IEnumerable<MemberViewModel> Members { get; set; }
    }
}
