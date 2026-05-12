using Microsoft.AspNetCore.Mvc;

namespace SERVE_ME_PROJECT.Areas.AdminControlPanel.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
