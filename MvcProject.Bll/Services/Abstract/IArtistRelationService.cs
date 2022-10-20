using MvcProject.Bll.ViewModels.Artist;

namespace MvcProject.Bll.Services.Abstract
{
    public interface IArtistRelationService
    {
        void DeleteAllArtistRelations(int artistId);
        MemberViewModel Create(MemberViewModel viewModel, RelationKeyViewModel keyModel);
        void Update(MemberViewModel viewModel, RelationKeyViewModel keyModel);
        void Delete(MemberViewModel viewModel, RelationKeyViewModel keyModel);
    }
}
