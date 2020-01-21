using System.Linq;
using Microsoft.AspNetCore.Rewrite;

namespace LogisticsBooking.FrontEnd.Services
{
    public class RewriteRules
    {
        public static void RedirectRequests(RewriteContext context)
        {
            //Your logic

            var request = context.HttpContext.Request;
            var path = request.Path.Value;

            var userLangs = request.Headers["Accept-Language"].ToString();
            var firstLang = userLangs.Split(',').FirstOrDefault();
            var defultCulture = string.IsNullOrEmpty(firstLang) ? "en" : firstLang.Substring(0,2);

            //Add your conditions of redirecting
            if ((path.Split("/")[1] != "en") && (path.Split("/")[1] != "de"))// If the url does not contain culture
            {
                context.HttpContext.Response.Redirect($"/{defultCulture}{ request.Path.Value }");
            }

        }
    }
}