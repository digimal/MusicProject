using Microsoft.AspNetCore.Http;

namespace MvcProject.WebApp.Helpers.Extensions
{
    public static class HttpRequestExtensions
    {
        private const string RequestedWithHeader = "X-Requested-With";
        private const string RefererHeader = "Referer";

        private const string XmlHttpRequest = "XMLHttpRequest";

        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (TryGetHeader(request, RequestedWithHeader, out string header))
            {
                return header == XmlHttpRequest;
            }

            return false;
        }

        public static string GetUrlReferrer(this HttpRequest request)
        {
            if (TryGetHeader(request, RefererHeader, out string header))
            {
                return header;
            }

            return null;
        }

        private static bool TryGetHeader(HttpRequest request, string headerName, out string header)
        {
            if (request?.Headers != null)
            {
                header = request.Headers[headerName];
                return true;
            }

            header = null;
            return false;
        }
    }
}
