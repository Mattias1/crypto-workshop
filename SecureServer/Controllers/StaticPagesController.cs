using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.RegularExpressions;

namespace SecureServer.Controllers {
    public class StaticPagesController : Controller {
        [HttpGet]
        public IActionResult Home() {
            return Content(HtmlContent("Crypto workshops", "For the challenges, go to TODO"), "text/html");
        }

        [HttpGet("/error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(HttpStatusCode? status) {
            string errorMessage = Regex.Replace(status?.ToString() ?? "Error", "(?<=[a-z])([A-Z])", " $1");
            Response.StatusCode = (int?)status ?? 500;
            return Content(HtmlContent($"Error - {(int?)status ?? 500}", errorMessage), "text/html");
        }

        public string HtmlContent(string title, string content) {
            // Yeah, I'm too lazy to make proper views.
            return @"
<!doctype HTML>
<html>
    <head>
        <meta http-equiv='Content-Type' content='text/html; charset=utf-8'>
        <title>Crypto workshop</title>
        <link rel='stylesheet' type='text/css' href='/css/site.css'>
    </head>
    <body>
        <h1>" + title + @"</h1><p>" + content + @"</p>
    </body>
</html>";
        }
    }
}
