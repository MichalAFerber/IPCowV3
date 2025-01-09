using Microsoft.AspNetCore.Mvc;
using IPCowV3.BusinessLogic;

namespace IPCowV3.Controllers
{
    public class ClientInfoController : Controller
    {
        public string JavascriptVersion { get; set; }
        public string ScreenResolution { get; set; }

        public ClientInfoController()
        {
            JavascriptVersion = string.Empty; // or any default value
            ScreenResolution = string.Empty; // or any default value
        }

        // Your existing methods
    }
}