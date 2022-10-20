using Microsoft.AspNet.Identity.EntityFramework;
using MvcProject.Domain;

namespace MvcProject.WebApp.Models.User
{

    public class WebAppUserStore : UserStore<Domain.User, Role, int, UserLogin, UserRole, UserClaim>
    {
        public WebAppUserStore(WebAppDbContext context) : base(context) { }
    }
}