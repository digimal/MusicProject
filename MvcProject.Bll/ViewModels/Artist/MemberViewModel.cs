using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Bll.ViewModels.Artist
{
    public class MemberViewModel : ArtistBaseViewModel
    {
        public int ArtistId { get; set; }

        public int RelationId { get; set; }

        public Common.TimeIntervalViewModel Interval { get; set; }
    }
}
