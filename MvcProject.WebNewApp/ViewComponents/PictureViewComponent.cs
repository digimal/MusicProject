using Microsoft.AspNetCore.Mvc;
using MvcProject.Bll.Services.Abstract;
using MvcProject.Bll.ViewModels.Common;

namespace MvcProject.WebNewApp.ViewComponents
{
    public class PictureViewComponent : ViewComponent
    {
        private const int DefaultPictureId = 1;

        private readonly IPictureService pictureService;

        public PictureViewComponent(IPictureService pictureService)
        {
            this.pictureService = pictureService;
        }

        public IViewComponentResult Invoke(int? pictureId)
        {
            return View("Picture", pictureService.Get(pictureId ?? DefaultPictureId));
        }
    }
}
