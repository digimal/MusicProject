using MvcProject.Bll.ViewModels.Artist;
using MvcProject.Bll.ViewModels.Common;
using System.Collections.Generic;

namespace MvcProject.Bll.Services.Abstract
{
    public interface IArtistLikeService
    {
        LikesViewModel GetLikes(int artistId, int? userId);
        LikesViewModel ChangeState(int artistId, int userId);
        IEnumerable<ArtistBaseViewModel> GetLikedArtists(int userId);
    }
}
