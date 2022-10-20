using System.Threading.Tasks;
using MvcProject.Domain;
using MvcProject.Bll.Services.Abstract;
using MvcProject.Bll.ViewModels.Common;
using AutoMapper;
using MvcProject.Dal;

namespace MvcProject.Bll.Services.Concrete
{
    public class PictureService : BaseService<Picture>, IPictureService
    {
        public PictureService(MusicContext context, IMapper mapper) : base(context, mapper) { }

        public PictureViewModel Create(PictureViewModel model)
        {
            return mapper.Map<Picture, PictureViewModel>(Create(mapper.Map<PictureViewModel, Picture>(model)));
        }

        public void Delete(int id)
        {
            DeleteOneIf(x => x.Id == id);
        }

        public async Task DeleteAsync(int id)
        {
            await DeleteAsyncOneIf(x => x.Id == id);
        }

        public PictureViewModel Get(int id)
        {
            return mapper.Map<Picture, PictureViewModel>(GetOneIf(x => x.Id == id));
        }

        public async Task<PictureViewModel> GetAsync(int id)
        {
            return mapper.Map<Picture, PictureViewModel>(await GetOneIfAsync(x => x.Id == id));
        }
    }
}
