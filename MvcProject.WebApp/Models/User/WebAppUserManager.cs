using Microsoft.Owin;
using MvcProject.Domain;
using System;

namespace MvcProject.WebApp.Models.User
{
    public class WebAppUserManager : UserManager<Domain.User>
    {
        public WebAppUserManager(UserStore<Domain.User, Role, int, UserLogin, UserRole, UserClaim> store) : base(store)
        {
            this.PasswordHasher = new Services.WebAppPasswordService(new System.Security.Cryptography.SHA512Managed());
            this.EmailService = new Services.WebAppEmailService();
        }

        public static WebAppUserManager Create(
            IdentityFactoryOptions<WebAppUserManager> options,
            IOwinContext context)
        {
            WebAppUserManager manager =
                new WebAppUserManager(
                    new UserStore<Domain.User, Role, int, UserLogin, UserRole, UserClaim>
                    (context.Get<WebAppDbContext>()));

            manager.UserValidator = new UserValidator<Domain.User, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireDigit = false,
                RequireLowercase = false,
                RequireNonLetterOrDigit = false,
                RequireUppercase = false
            };

            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<Domain.User, int>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }
}