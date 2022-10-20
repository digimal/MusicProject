using MvcProject.Bll.ViewModels.Artist;
using System.Collections.Generic;

namespace MvcProject.Bll.Services.Abstract
{
    public interface IArtistTagService
    {
        IEnumerable<TagViewModel> GetMatchesContains(string match);
        IEnumerable<TagViewModel> GetMatchesStartsWith(string match);
    }
}
