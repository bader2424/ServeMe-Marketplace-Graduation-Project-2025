using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SERVE_ME_PROJECT.Data;
using SERVE_ME_PROJECT.Models;

namespace SERVE_ME_PROJECT.Controllers
{
    public class TourismController : CatalogControllerBase
    {
        private const string CatalogControllerName = "Tourism";

        public TourismController(ServeMeContext context, UserManager<ApplicationUser> userManager)
            : base(context, userManager)
        {
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return CatalogIndex(CatalogControllerName);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Services(int categoryId = 46, int page = 1)
        {
            return CatalogServices(CatalogControllerName, categoryId, page);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Services(string[] cities, int? categoryId, int page = 1)
        {
            return CatalogServices(CatalogControllerName, cities, categoryId, page);
        }

        [AllowAnonymous]
        public IActionResult Details_Services(int id)
        {
            return CatalogDetails(CatalogControllerName, id);
        }

        [HttpPost]
        [Authorize]
        public Task<IActionResult> AddComment(string content, int rating, int serviceId)
        {
            return CatalogAddComment(CatalogControllerName, content, rating, serviceId);
        }
    }
}
