using MvcProject.Bll.Services.Abstract;
using MvcProject.Domain;
using System.Collections.Generic;
using System.Linq;
using MvcProject.Bll.ViewModels.Common;
using MvcProject.Bll.ViewModels.Artist;
using AutoMapper;
using MvcProject.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MvcProject.Bll.Services.Concrete
{
    public class ArtistLikeService : BaseService<FavoriteArtist>, IArtistLikeService
    {
        public ArtistLikeService(MusicContext context, IMapper mapper) : base(context, mapper)
        {

        }

        private LikesViewModel SetLike(int artistId, int userId)
        {
            Create(new FavoriteArtist() { ArtistId = artistId, UserId = userId });
            return GetLikes(artistId, true);
        }

        private LikesViewModel RemoveLike(FavoriteArtist favArtist)
        {
            Delete(favArtist);
            return GetLikes(favArtist.ArtistId, false);
        }

        public LikesViewModel GetLikes(int artistId, int? userId)
        {
            var likes = GetManyIf(x => x.ArtistId == artistId);

            return new LikesViewModel()
            {
                LikesCount = likes.Count(),
                IsLiked = userId.HasValue ? likes.Any(x => x.UserId == userId) : false
            };
        }

        public LikesViewModel ChangeState(int artistId, int userId)
        {
            var favArtist = GetOneIf(x => x.ArtistId == artistId && x.UserId == userId);
            return favArtist == null 
                ? SetLike(artistId, userId) 
                : RemoveLike(favArtist);
        }

        private LikesViewModel GetLikes(int artistId, bool isLiked)
        {
            return new LikesViewModel()
            {
                LikesCount = GetManyIf(x => x.ArtistId == artistId).Count(),
                IsLiked = isLiked
            };
        }

        public IEnumerable<ArtistBaseViewModel> GetLikedArtists(int userId)
        {
            return GetManyIf(x => x.UserId == userId)
                .ToArray()
                .Select(x => mapper.Map<Artist, ArtistBaseViewModel>(x.Artist));
        }
    }
}
