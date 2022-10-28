using Microsoft.AspNetCore.Mvc;
using MvcProject.Bll.ViewModels.Common;

namespace MvcProject.WebNewApp.ViewComponents
{
    public class PictureReadyViewComponent : ViewComponent
    {
        private const string DefaultPicturePath = "/Pictures/Default/default-1.jpg";

        public IViewComponentResult Invoke(string picturePath)
        {
            return View("Picture", new PictureViewModel { Path = string.IsNullOrEmpty(picturePath) ? DefaultPicturePath : picturePath });
        }
    }
}
