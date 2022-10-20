using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using MvcProject.Shared.Logging;

namespace MvcProject.WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error()
        {
            var ex = Server.GetLastError();
            ILocalLogger localLogger = DbLogger.Create();
            //Serilog.Debugging.SelfLog.Enable(TextWriter.Synchronized(File.CreateText("D:/looger.txt")));
            localLogger.Write(new Shared.Logging.Log()
            {
                Level = Serilog.Events.LogEventLevel.Error,
                Message = ex.ToString(),
                TimeStamp = DateTime.Now,
                UserId = User.Identity.IsAuthenticated ? User.Identity.GetUserId<int>() : 1
            });
            Server.ClearError();
            var routeData = new RouteData();
            routeData.Values["controller"] = "Error";
            Response.StatusCode = Response.StatusCode == 200 ? 404 : Response.StatusCode;
            routeData.Values["action"] = Controllers.ErrorController.GetResponseActionName(Response.StatusCode);
            switch (Response.StatusCode)
            {
                case 404: routeData.Values["url"] = Request.Url.OriginalString; break;
            };
            Response.RedirectToRoute(routeData.Values);
        }

        protected void Application_EndRequest(Object sender, EventArgs e)
        {
            if (Response.Status.Substring(0, 3).Equals("401"))
            {
                Response.ClearContent();
                var routeData = new RouteData();
                routeData.Values["controller"] = "Error";
                routeData.Values["action"] = Controllers.ErrorController.GetResponseActionName(401);
                Response.RedirectToRoute(routeData.Values);
            }
        }
    }
}
