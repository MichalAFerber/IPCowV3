using Microsoft.AspNetCore.Mvc;
using IPCowV3.BusinessLogic;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace IPCowV3.Controllers
{
    public class HomeController : Controller
    {
        private readonly IpAddressService _ipAddressService;

        public HomeController(IpAddressService ipAddressService)
        {
            _ipAddressService = ipAddressService;
        }

        [Route("")]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            var (ipv4, ipv6) = _ipAddressService.GetIpAddresses();
            var userAgent = _ipAddressService.GetUserAgent();

            LocationData? locationData = null;
            if (!string.IsNullOrEmpty(ipv4))
            {
                locationData = await GetLocationData(ipv4);
            }

            ViewData["IPv4"] = ipv4 ?? "Not available";
            ViewData["IPv6"] = ipv6 ?? "Not available";
            ViewData["UserAgent"] = userAgent ?? "Not available";
            ViewData["Hostname"] = locationData?.Hostname ?? "Not available";
            ViewData["Country"] = locationData?.Country ?? "Not available";
            ViewData["Region"] = locationData?.Region ?? "Not available";
            ViewData["City"] = locationData?.City ?? "Not available";
            ViewData["ISP"] = locationData?.ISP ?? "Not available";
            ViewData["Latitude"] = locationData?.Latitude ?? "Not available";
            ViewData["Longitude"] = locationData?.Longitude ?? "Not available";
            ViewData["Timezone"] = locationData?.Timezone ?? "Not available";

            return View("~/Views/Index.cshtml");
        }

        public IActionResult ServerVariables()
        {
            var serverVariables = new Dictionary<string, string>();

            // Add HTTP headers
            foreach (var header in Request.Headers)
            {
                serverVariables[header.Key] = header.Value.ToString();
            }

            // Add additional server variables
            serverVariables["Remote IP Address"] = HttpContext.Connection.RemoteIpAddress?.ToString();
            serverVariables["Local IP Address"] = HttpContext.Connection.LocalIpAddress?.ToString();
            serverVariables["Request Method"] = HttpContext.Request.Method;
            serverVariables["Request Path"] = HttpContext.Request.Path;
            serverVariables["Query String"] = HttpContext.Request.QueryString.Value;
            serverVariables["User Agent"] = HttpContext.Request.Headers["User-Agent"].ToString();
            serverVariables["Referer"] = HttpContext.Request.Headers["Referer"].ToString();

            // Add common HTTP headers
            var commonHeaders = new List<string>
        {
            "Accept", "Accept-Charset", "Accept-Encoding", "Accept-Language", "Authorization",
            "Age", "Cache-Control", "Clear-Site-Data", "Connection", "Content-Language", "Content-Length",
            "Content-Location", "Content-MD5", "Content-Range", "Content-Security-Policy", "Content-Type",
            "Cookie", "Date", "ETag", "Expect", "Expires", "Forwarded", "From", "Host", "If-Match",
            "If-Modified-Since", "If-None-Match", "If-Range", "If-Unmodified-Since", "Last-Modified",
            "Link", "Location", "Max-Forwards", "Origin", "Pragma", "Proxy-Authenticate", "Proxy-Authorization",
            "Range", "Referer", "Retry-After", "Server", "TE", "Trailer", "Transfer-Encoding", "Upgrade",
            "User-Agent", "Vary", "Via", "Warning", "WWW-Authenticate", "X-Content-Type-Options"
        };

            foreach (var header in commonHeaders)
            {
                if (Request.Headers.ContainsKey(header))
                {
                    serverVariables[$"HTTP {header}"] = Request.Headers[header].ToString();
                }
            }

            var sortedServerVariables = serverVariables.OrderBy(kv => kv.Key).ToDictionary(kv => kv.Key, kv => kv.Value);
            return View(sortedServerVariables);
        }

        private async Task<LocationData?> GetLocationData(string ipAddress)
        {
            try
            {
                using var client = new HttpClient();
                var response = await client.GetStringAsync($"https://api.ipgeolocation.io/ipgeo?apiKey=fa39b8656f6d4e4aa34762675213f1ab&ip={ipAddress}&fields=city&output=xml");
                return JsonConvert.DeserializeObject<LocationData>(response);
            }
            catch
            {
                return null;
            }
        }

        [Route("Privacy")]
        public IActionResult Privacy()
        {
            return View("~/Views/Privacy.cshtml");
        }
    }

    public class LocationData
    {
        public string? Hostname { get; set; }
        public string? Country { get; set; }
        public string? Region { get; set; }
        public string? City { get; set; }
        public string? ISP { get; set; }
        public string? ASN { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? Timezone { get; set; }
    }
}