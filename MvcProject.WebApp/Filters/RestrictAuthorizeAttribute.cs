namespace MvcProject.WebApp.Filters
{
    public class RestrictAuthorizeAttribute : Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext filterContext)
        {
            if(filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new Microsoft.AspNetCore.Mvc.RedirectResult(@"\Artist\Index");
            }
        }
    }
}