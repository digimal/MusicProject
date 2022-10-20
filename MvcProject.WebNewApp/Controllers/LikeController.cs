using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MvcProject.Bll.Services.Abstract;
using MvcProject.Domain;

namespace MvcProject.WebNewApp.Controllers
{
    public class LikeController : BaseController
    {
        private readonly IArtistLikeService likeService;

        public LikeController(IArtistLikeService likeService, UserManager<User> userManager) : base(userManager)
        {
            this.likeService = likeService;
        }

        [HttpPost]
        [Authorize]
        public IActionResult ChangeLikeState([FromRoute]int id)
        {
            likeService.ChangeState(id, GetUserId());
            return ViewComponent("Likes", id);
        }
    }
}
