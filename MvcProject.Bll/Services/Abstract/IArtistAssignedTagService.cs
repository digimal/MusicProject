using System.Collections.Generic;

namespace MvcProject.Bll.Services.Abstract
{
    public interface IArtistAssignedTagService
    {
        IEnumerable<int> GetArtistTagsIds(int artistId);
        void Remove(int artistId, int tagId);
        void Add(int artistId, int tagId);
        void UpdateTagsForArtist(int artistId, IEnumerable<int> tags);
    }
}
