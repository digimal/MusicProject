using Microsoft.AspNetCore.Identity;
using MvcProject.Domain;

namespace MvcProject.WebNewApp.Helpers
{
    public static class UserHelper
    {
        public static int? GetCurrentUserId(this UserManager<User> userManager, HttpContext context)
        {
            return int.TryParse(userManager.GetUserId(context.User), out int userId) ? userId : null;
        }
    }
}
