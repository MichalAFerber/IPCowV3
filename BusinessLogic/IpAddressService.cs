using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Http;

namespace IPCowV3.BusinessLogic
{
    public class IpAddressService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IpAddressService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public (string? IPv4, string? IPv6) GetIpAddresses()
        {
            var ipAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress;
            if (ipAddress == null)
            {
                return (null, null);
            }

            string? ipv4 = null;
            string? ipv6 = null;

            if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                ipv4 = ipAddress.ToString();
            }
            else if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
            {
                ipv6 = ipAddress.ToString();
            }

            return (ipv4, ipv6);
        }

        public string? GetUserAgent()
        {
            return _httpContextAccessor.HttpContext?.Request?.Headers["User-Agent"].ToString();
        }
    }
}