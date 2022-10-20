using MvcProject.Shared.Logging;
using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;

namespace MvcProject.WebApp.Controllers
{
    public abstract class BaseController : Microsoft.AspNetCore.Mvc.Controller
    {
        ILocalLogger localLogger = DbLogger.Create();

        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;

            WriteToLog(Serilog.Events.LogEventLevel.Error, filterContext.Exception.ToString());

            var response = filterContext.HttpContext.Response;
            var request = filterContext.HttpContext.Request;

            var routeData = new RouteData();
            routeData.Values["controller"] = "Error";

            response.StatusCode = response.StatusCode == 200 ? 404 : response.StatusCode;
            routeData.Values["action"] = Controllers.ErrorController.GetResponseActionName(Response.StatusCode);
            switch (Response.StatusCode)
            {
                case 404: routeData.Values["url"] = Request.Url.OriginalString; break;
            };
            filterContext.Result = RedirectToRoute(routeData.Values);

        }

        protected void WriteToLog(Serilog.Events.LogEventLevel level, string message)
        {
            localLogger.Write(new Log()
            {
                Level = level,
                Message = message,
                TimeStamp = DateTime.Now,
                UserId = User.Identity.IsAuthenticated ? User.Identity.GetUserId<int>() : 1
            });
        }

        protected void WriteToLog(Serilog.Events.LogEventLevel level, string message, DateTime timestamp, int userId)
        {
            localLogger.Write(new Log()
            {
                Level = level,
                Message = message,
                TimeStamp = timestamp,
                UserId = userId
            });
        }

        protected IActionResult InvokeNotFound(string url)
        {
            return RedirectToAction("NotFound", "Error", new { url } );
        }

        protected IActionResult InvokeNotFound()
        {
            return InvokeNotFound(Request.Url.OriginalString);
        }
    }
}