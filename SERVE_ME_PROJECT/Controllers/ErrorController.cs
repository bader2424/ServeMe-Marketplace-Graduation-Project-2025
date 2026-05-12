using Microsoft.AspNetCore.Mvc;

namespace SERVE_ME_PROJECT.Controllers
{
    [Microsoft.AspNetCore.Authorization.AllowAnonymous]
    public class ErrorController : Controller
    {
        [Route("Error/NotFound")]
        public IActionResult NotFoundPage()
        {
            return View("NotFound");
        }

        [Route("Error/AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View("AccessDenied");
        }
    }
}
