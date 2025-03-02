using Microsoft.AspNetCore.Mvc;

namespace PartyInvitationApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Simple welcome page
            return View();
        }
    }
}
