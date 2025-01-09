using Microsoft.AspNetCore.Mvc;
using IPCowV3.BusinessLogic;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

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

        private async Task<LocationData?> GetLocationData(string ipAddress)
        {
            using var client = new HttpClient();
            var response = await client.GetStringAsync($"https://api.ipgeolocation.io/ipgeo?apiKey=e37a08b322714a44909771955a2fb60c&include=liveHostname&ip={ipAddress}");
            return JsonConvert.DeserializeObject<LocationData>(response);
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