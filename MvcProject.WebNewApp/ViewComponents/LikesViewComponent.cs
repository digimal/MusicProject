using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MvcProject.Bll.Services.Abstract;
using MvcProject.Domain;
using MvcProject.WebNewApp.Helpers;

namespace MvcProject.WebNewApp.ViewComponents
{
    public class LikesViewComponent : ViewComponent
    {
        private readonly IArtistLikeService likeService;
        private readonly UserManager<User> userManager;

        public LikesViewComponent(IArtistLikeService likeService, UserManager<User> userManager)
        {
            this.likeService = likeService;
            this.userManager = userManager;
        }

        public IViewComponentResult Invoke(int artistId)
        {
            ViewBag.ArtistId = artistId;
            return View("Likes", likeService.GetLikes(artistId, userManager.GetCurrentUserId(HttpContext)));
        }
    }
}
