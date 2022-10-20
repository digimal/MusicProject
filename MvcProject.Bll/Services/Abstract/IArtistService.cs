using System.Collections.Generic;
using MvcProject.Bll.ViewModels.Common;
using MvcProject.Bll.ViewModels.Artist;

namespace MvcProject.Bll.Services.Abstract
{
    public interface IArtistService
    {
        ArtistViewModel Create(ArtistViewModel artistViewModel);
        void Update(ArtistViewModel artistViewModel);
        void Delete(int artistId);
        IEnumerable<ArtistBaseViewModel> GetArtists(int skip, int take);
        IEnumerable<ArtistBaseViewModel> GetArtists();
        ArtistShowViewModel GetArtist(int artistId);
        IEnumerable<TagViewModel> GetTags(int artistId);
        RelationChoiceViewModel GetRelationChoiceViewModel(int artistId);
        IEnumerable<MemberViewModel> GetRelationMembers(int artistId, int relationId, RelationKeyViewModel model);
        IEnumerable<ArtistBaseViewModel> GetRelationCandidates(int artistId, RelationKeyViewModel model, string query);
    }
}
