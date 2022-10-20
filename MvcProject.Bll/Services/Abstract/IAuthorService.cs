using MvcProject.Bll.ViewModels.Artist;

namespace MvcProject.Bll.Services.Abstract
{
    public interface IAuthorService
    {
        int Obtain(params ArtistBaseViewModel[] artists);
        int[] GetArtistCollabsIds(int artistId);
        void AddArtist(ArtistBaseViewModel artist);
        void RemoveArtist(int artistId);
    }
}
