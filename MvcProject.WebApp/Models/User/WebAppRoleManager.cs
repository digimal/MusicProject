using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MvcProject.Domain;

namespace MvcProject.WebApp.Models.User
{
    public class WebAppRoleManager : RoleManager<Role, int>
    {
        public WebAppRoleManager(RoleStore<Role, int, UserRole> store) : base(store) { }
    }

    public class WebAppRoleStore : RoleStore<Role, int, UserRole>
    {
        public WebAppRoleStore(IdentityDbContext context) : base(context) { }
    }
}