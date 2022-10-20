using MvcProject.WebApp.Models.User;

namespace MvcProject.WebApp.Controllers
{
    public abstract class BaseUserManagementController : BaseController
    {
            
        WebAppUserManager userManager;
        WebAppSignInManager signInManager;

        protected WebAppUserManager UserManager
        {
            get => userManager ?? HttpContext.GetOwinContext().GetUserManager<Models.User.WebAppUserManager>();
            set => userManager = value;
        }
        protected WebAppSignInManager SignInManager
        {
            get => signInManager ?? HttpContext.GetOwinContext().Get<Models.User.WebAppSignInManager>();
            set => signInManager = value;
        }

        public BaseUserManagementController() { }

        public BaseUserManagementController(WebAppUserManager userManager, WebAppSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (userManager != null)
                {
                    userManager.Dispose();
                    userManager = null;
                }
                if (signInManager != null)
                {
                    signInManager.Dispose();
                    signInManager = null;
                }
            }
            base.Dispose(disposing);
        }

    }
}