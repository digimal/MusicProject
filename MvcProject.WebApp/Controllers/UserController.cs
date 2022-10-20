using MvcProject.Bll.ViewModels.User;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using MvcProject.WebApp.Models.User;
using MvcProject.WebApp.Filters;
using MvcProject.WebApp.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MvcProject.WebApp.Controllers
{
    public class UserController : BaseUserManagementController
    {

        public UserController(WebAppUserManager userManager, WebAppSignInManager signInManager) 
            : base(userManager, signInManager) { }

        public UserController() { }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult LogOut()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        [AllowAnonymous]
        [RestrictAuthorize]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Login(UserLoginViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var user = UserManager.FindByEmail(model.Email);
            if(user == null)
            {
                ModelState.AddModelError("CredentialsError", "Invalid credentials.");
                return View(model);
            }

            if (!user.EmailConfirmed)
            { 
                ModelState.AddModelError("EmailConfirmationError", "Account was not confirmed via e-mail.");
                return View(model);
            }        
            var result = SignInManager.PasswordSignIn(user.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToAction("Index", "Artist");
                case SignInStatus.RequiresVerification:
                    return View("Info", "What? It's not enabled yet!");
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("CredentialsError", "Invalid credentials.");
                    return View(model);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [RestrictAuthorize]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegistrationViewModel model)
        {
            if(ModelState.IsValid)
            {
                var newUser = AutoMapper.Mapper.Map<UserRegistrationViewModel, Domain.User>(model);
                var result = await UserManager.CreateAsync(newUser, model.Password);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByEmailAsync(model.Email);
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("Confirm", "User", new { email = user.Email, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(user.Id, "JAMS registration confirmation",
                               "To complete your registration click next link :: <a href=\""
                                                               + callbackUrl + "\">Register</a>");
                    return View("FinishRegistration");
                }
                AddErrors(result);
            }
            return View(model);
        }

        public async Task<IActionResult> SendCode(string email)
        {
            var user = UserManager.FindByEmail(email);
            if (!user.EmailConfirmed)
            {
                string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                var callbackUrl = Url.Action("Confirm", "User", new { email = user.Email, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "JAMS registration confirmation",
                           "To complete your registration click next link :: <a href=\""
                                                           + callbackUrl + "\">Register</a>");
                return View("Info", new
                {
                    message = "Please, finish your registration by clicking the link sent to your e-mail."
                });
            }
            return InvokeNotFound();
        }

        [AllowAnonymous]
        public async Task<IActionResult> Confirm(string email, string code)
        {
            Domain.User user;
            if(    email == null 
                || code == null 
                || (user = UserManager.FindByEmail(email)) == null)
            {
                return InvokeNotFound();
            }
            var result = await UserManager.ConfirmEmailAsync(user.Id, code);
            return result.Succeeded 
                ? View("EmailConfirmed")
                : View("UserNotConfirmed");
        }

        [AllowAnonymous]
        [RequireHttps]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "User", new { ReturnUrl = returnUrl }));
        }


        [AllowAnonymous]
        [RequireHttps]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                //case SignInStatus.RequiresVerification:
                // return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [RequireHttps]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return RedirectToAction("ExternalLoginFailure");
                }
                var user = new Domain.User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    var regUser = UserManager.FindByName(user.UserName);
                    result = await UserManager.AddLoginAsync(regUser.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(regUser, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        public IActionResult ExternalLoginFailure()
        {
            return View();
        }

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}