using System.Collections.Generic;
using System.Linq;
using MvcProject.Domain;
using MvcProject.Bll.Services.Abstract;
using MvcProject.Bll.ViewModels.Artist;
using AutoMapper;
using MvcProject.Dal;

namespace MvcProject.Bll.Services.Concrete
{
    public class ArtistTagService : BaseService<ArtistTag>, IArtistTagService
    {
        public ArtistTagService(MusicContext context, IMapper mapper) : base(context, mapper) { }

        public TagViewModel Get(string name)
        {
            return mapper.Map<ArtistTag, TagViewModel>(
                GetOneIf(x => x.Name == name));
        }

        public void Add(TagViewModel model)
        {
            Create(mapper.Map<TagViewModel, ArtistTag>(model));
        }

        public void Update(TagViewModel model)
        {
            Update(mapper.Map<TagViewModel, ArtistTag>(model));
        }

        public void Delete(int id)
        {
            DeleteOneIf(x => x.Id == id);
        }

        public IEnumerable<TagViewModel> GetMatchesContains(string match)
        {
            return mapper.Map<IQueryable<ArtistTag>, IEnumerable<TagViewModel>>(
                dbSet.Where(x => x.Name.Contains(match)));
        }

        public IEnumerable<TagViewModel> GetMatchesStartsWith(string match)
        {
            return mapper.Map<IQueryable<ArtistTag>, IEnumerable<TagViewModel>>(
                dbSet.Where(x => x.Name.StartsWith(match)));
        }
    }
}
