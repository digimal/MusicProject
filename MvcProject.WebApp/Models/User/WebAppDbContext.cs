using Microsoft.AspNet.Identity.EntityFramework;
using MvcProject.Domain;

namespace MvcProject.WebApp.Models.User
{
    public class WebAppDbContext 
        : IdentityDbContext<
            Domain.User, 
            Role, 
            int, 
            UserLogin, 
            UserRole, 
            UserClaim>
    {
        public WebAppDbContext() : base("MusicProject") { }

        public static WebAppDbContext Create() => new WebAppDbContext();
    }
}