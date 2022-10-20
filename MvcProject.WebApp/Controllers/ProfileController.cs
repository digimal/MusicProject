using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcProject.Bll.Services.Abstract;
using MvcProject.Bll.ViewModels.User;
using MvcProject.WebApp.Models.User;

namespace MvcProject.WebApp.Controllers
{
    [Authorize]
    public class ProfileController : BaseUserManagementController
    {
        private readonly IArtistLikeService _artistLikeService;

        public ProfileController (IArtistLikeService artistLikeService)
        {
            _artistLikeService = artistLikeService;
        }

        public ProfileController(IArtistLikeService artistLikeService, WebAppUserManager userManager, WebAppSignInManager signInManager)
             : base(userManager, signInManager)
        {
            _artistLikeService = artistLikeService;
        }

        private int CurrentId => User.Identity.GetUserId<int>();
        private Domain.User CurrentUser => UserManager.FindById(CurrentId);

        public IActionResult Index()
        {
            return View(Mapper.Map<Domain.User, ProfileViewModel>(CurrentUser));
        }

        public IActionResult FavouriteArtists()
        {
            return PartialView("FavouriteArtists", _artistLikeService.GetLikedArtists(CurrentId));
        }
    }
}