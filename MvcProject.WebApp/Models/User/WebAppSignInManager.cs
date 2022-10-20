using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MvcProject.WebApp.Models.User
{
    public class WebAppSignInManager : SignInManager<Domain.User, int>
    {
        public WebAppSignInManager(
            WebAppUserManager userManager, 
            IAuthenticationManager authenticationManager) 
            : base(userManager, authenticationManager)
        {
        }

        public static WebAppSignInManager Create(
            IdentityFactoryOptions<WebAppSignInManager> options, 
            IOwinContext context)
        {
            return new WebAppSignInManager(
                context.GetUserManager<WebAppUserManager>(), 
                context.Authentication);
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(Domain.User user)
        {
            return UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
        }
    }
}