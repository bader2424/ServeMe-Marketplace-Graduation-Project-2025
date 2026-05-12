using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SERVE_ME_PROJECT.Models;
using SERVE_ME_PROJECT.Data;

namespace SERVE_ME_PROJECT.Areas.AdminControlPanel.Controllers
{
    [Area("Provider")]
    [Authorize(Roles = "provider")]
    public class BaseProviderController : Controller
    {
        protected readonly UserManager<ApplicationUser> _userManager;
        private readonly ServeMeContext _context;


        public BaseProviderController(ServeMeContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public override async void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!string.IsNullOrEmpty(userId))
            {
                var provider = await _userManager.FindByIdAsync(userId);

                ViewBag.ProviderFullName = provider.FullName;
                ViewBag.ProviderImage = string.IsNullOrEmpty(provider.ProfileImagePath)
                    ? "~/provider/images/pofl.jpg"
                    : provider.ProfileImagePath;
            }

            base.OnActionExecuting(context);
        }
    }
}
