using System;
using System.Collections.Generic;
using System.Linq;
using MvcProject.Domain;
using MvcProject.Bll.Services.Abstract;
using MvcProject.Dal;
using AutoMapper;

namespace MvcProject.Bll.Services.Concrete
{
    public class ArtistAssignedTagService : BaseService<ArtistAssignedTag>, IArtistAssignedTagService
    {
        public ArtistAssignedTagService(MusicContext musicContext, IMapper mapper) : base(musicContext, mapper) { }

        public void Add(int artistId, int tagId)
        {
            if (!dbSet.Any(x => x.ArtistId == artistId && x.ArtistTagId == tagId))
            {
                Create(new ArtistAssignedTag()
                {
                    ArtistId = artistId,
                    ArtistTagId = tagId
                });
            }
        }

        public void Remove(int artistId, int tagId)
        {
            var assignedTag = GetOneIf(x => x.ArtistTagId == tagId && x.ArtistId == artistId);
            if (assignedTag != null)
            {
                Delete(assignedTag);
            }
        }

        public IEnumerable<int> GetArtistTagsIds(int artistId)
        {
            return GetManyIf(x => x.ArtistId == artistId).Select(x => x.ArtistTagId);
        }

        public void UpdateTagsForArtist(int artistId, IEnumerable<int> tags)
        {
            TemplateFunc(Remove, GetArtistTagsIds(artistId), tags, artistId);
            TemplateFunc(Add, tags, GetArtistTagsIds(artistId), artistId);
        }

        private void TemplateFunc(Action<int, int> action, IEnumerable<int> firstSet, IEnumerable<int> secondSet, int artistId)
        {
            var set = firstSet.Except(secondSet).ToArray();
            foreach (var tagId in set)
            {
                action(artistId, tagId);
            }
        }
    }
}
