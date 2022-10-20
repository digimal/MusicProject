using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MvcProject.Domain;

namespace MvcProject.WebNewApp.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly UserManager<User> userManager;

        public BaseController(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        protected int GetUserId()
        {
            return int.Parse(userManager.GetUserId(HttpContext.User));
        }
    }
}
