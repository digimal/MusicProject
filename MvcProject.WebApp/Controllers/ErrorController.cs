using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using MvcProject.WebApp.Helpers.Extensions;
using MvcProject.WebApp.Models.Error;

namespace MvcProject.WebApp.Controllers
{
    public class ErrorController : BaseController
    {
        public IActionResult Default()
        {
            return View(new ErrorViewModel() { Message = "Oups...  " });
        }

        public IActionResult NotFound(string url)
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            var displayUrl = Request.GetDisplayUrl();
            var model = new ErrorUrlViewModel
            {
                RequestedUrl = url == null
                    ? displayUrl
                    : displayUrl.Contains(url) & Request.GetDisplayUrl() != url
                        ? displayUrl
                        : url
            };

            var referrer = Request.GetUrlReferrer();

            model.ReferrerUrl = referrer != null && referrer != model.RequestedUrl
                ? referrer 
                : null;

            WriteToLog(Serilog.Events.LogEventLevel.Error, $"Error 404: {model.RequestedUrl} was  not found");

            return View("NotFound", model);
        }

        public IActionResult Unauthorized()
        {
            var referrer = Request.GetUrlReferrer();

            var model = new ErrorUrlViewModel
            {
                ReferrerUrl = referrer != null && referrer != Request.GetDisplayUrl()
                    ? referrer
                    : null
            };
            return View(model);
        }

        internal static string GetResponseActionName(int statusCode)
        {
            return ResponseActions.ContainsKey(statusCode)
                ? ResponseActions[statusCode]
                : "Default";
        }

        private static Dictionary<int, string> ResponseActions = new Dictionary<int, string>()
        {
            { 404, "NotFound" },
            { 401, "Unauthorized" }
        }; 
    }
}