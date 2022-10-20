using System.Threading.Tasks;
using MvcProject.Bll.ViewModels.Common;

namespace MvcProject.Bll.Services.Abstract
{
    public interface IPictureService
    {
        PictureViewModel Create(PictureViewModel model);
        void Delete(int id);
        PictureViewModel Get(int id);
        Task<PictureViewModel> GetAsync(int id);
        Task DeleteAsync(int id);
    }
}
