using System.Collections.Generic;
using System.Linq;

namespace MvcProject.Bll.ViewModels.Authorship
{
    public class AuthorshipViewModel
    {
        public int AuthorId { get; set; }
        public IEnumerable<AuthorArtistDisplayViewModel> Authors { get; set; }
        public override string ToString()
        {
            return Authors
                .OrderBy(x => x.Position)
                .Select(x => ' ' + x.JoinPhrase + ' ' + x.DisplayName)
                .Aggregate((y, z) => y + z)
                .TrimStart(' ');
        }
    }

}
