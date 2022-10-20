using System.Collections.Generic;
using System.Linq;
using MvcProject.Domain;
using MvcProject.Bll.ViewModels.Authorship;
using MvcProject.Bll.ViewModels.Artist;
using AutoMapper;
using MvcProject.Dal;

namespace MvcProject.Bll.Services.Concrete
{
    public class AuthorArtistService : BaseService<AuthorArtist>
    {
        public AuthorArtistService(MusicContext context, IMapper mapper) : base (context, mapper) { }

        public void Create(AuthorArtistDisplayViewModel model)
        {
            Create(mapper.Map<AuthorArtistDisplayViewModel, AuthorArtist>(model));
        }

        public void Update(AuthorArtistEditViewModel model)
        {
            Update(mapper.Map<AuthorArtistEditViewModel, AuthorArtist>(model));
        }

        public void Delete(AuthorArtistEditViewModel model)
        {
            Delete(mapper.Map<AuthorArtistEditViewModel, AuthorArtist>(model));
        }

        public AuthorshipViewModel GetRecordingAuthorship(Recording recording)
        {
            return new AuthorshipViewModel()
            {
                AuthorId = recording.AuthorId,
                Authors = mapper.Map<ICollection<AuthorArtist>, IEnumerable<AuthorArtistDisplayViewModel>>(recording.Author.Members)
            };
        }

        public IEnumerable<ArtistAuthorDisplayViewModel> GetArtistAuthorships(Artist artist)
        {
            return artist.Authorships.Select(x => new ArtistAuthorDisplayViewModel()
            {
                Artist = mapper.Map<Artist, ArtistBaseViewModel>(x.Artist),
                DisplayName = x.Name,
                JoinPhrase = x.JoinPhrase,
                Position = x.Position
            });
        }

        private AuthorshipViewModel GetAuthorship(Author author)
        {
            return new AuthorshipViewModel()
            {
                AuthorId = author.Id,
                Authors = mapper.Map<ICollection<AuthorArtist>, IEnumerable<AuthorArtistDisplayViewModel>>(author.Members)
            };

        }

        public IEnumerable<AuthorshipViewModel> GetArtistsCommonAuthorships(IEnumerable<Artist> artists)
        {
            return artists
                .Select(artist => artist.Authorships.Select(a => a.Author))
                .Aggregate((xs, ys) => xs.Intersect(ys))
                .Select(author => GetAuthorship(author))
                .OrderBy(vm => vm.Authors.Count());
        }
    }
}
