using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SERVE_ME_PROJECT.Data;
using SERVE_ME_PROJECT.Models;
using SERVE_ME_PROJECT.ViewModel;

namespace SERVE_ME_PROJECT.Controllers
{
    public abstract class CatalogControllerBase : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        protected readonly ServeMeContext Context;

        protected CatalogControllerBase(ServeMeContext context, UserManager<ApplicationUser> userManager)
        {
            Context = context;
            _userManager = userManager;
        }

        protected IActionResult CatalogIndex(string controllerName)
        {
            ViewBag.CatalogController = controllerName;

            var vmservices = new VmServices
            {
                BannerModel = Context.TbBanner.ToList(),
                ServiceModel = Context.TbService.ToList()
            };

            SetCategory("Category1", 45);
            SetCategory("Category2", 46);
            SetCategory("Category3", 47);
            SetCategory("Category4", 48);
            SetCategory("Category5", 49);
            SetCategory("Category6", 50);
            SetCategory("Category7", 51);
            SetCategory("Category8", 52);
            SetCategory("Category9", 53);

            return View("~/Views/Graduation/Index.cshtml", vmservices);
        }

        protected IActionResult CatalogServices(string controllerName, int categoryId, int page = 1)
        {
            ViewBag.CatalogController = controllerName;

            const int pageSize = 9;
            var servicesQuery = Context.TbService
                .Where(s => s.CategoryId == categoryId && s.IsApproved)
                .Include(s => s.Images)
                .Include(s => s.Category);

            var totalCount = servicesQuery.Count();
            var services = servicesQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.CategoryId = categoryId;

            return View("~/Views/Graduation/Services.cshtml", services);
        }

        protected IActionResult CatalogServices(string controllerName, string[] cities, int? categoryId, int page = 1)
        {
            ViewBag.CatalogController = controllerName;
            cities ??= [];

            var query = Context.TbService
                .Include(s => s.City)
                .Include(s => s.Images)
                .Include(s => s.Category)
                .Where(s => s.IsApproved)
                .AsQueryable();

            if (cities.Length > 0 && !cities.Contains("الكل"))
            {
                query = query.Where(s => cities.Contains(s.City.Name));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(s => s.CategoryId == categoryId.Value);
            }

            const int pageSize = 9;
            var totalItems = query.Count();
            var services = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.CategoryId = categoryId;
            ViewBag.Cities = Context.TbCity.Select(c => c.Name).ToList();

            return View("~/Views/Graduation/Services.cshtml", services);
        }

        protected IActionResult CatalogDetails(string controllerName, int id)
        {
            ViewBag.CatalogController = controllerName;

            var service = Context.TbService
                .Where(s => s.Id == id)
                .Include(s => s.Images)
                .Include(s => s.Comments)
                    .ThenInclude(c => c.User)
                .FirstOrDefault();

            return service == null
                ? NotFound()
                : View("~/Views/Graduation/Details_Services.cshtml", service);
        }

        protected async Task<IActionResult> CatalogAddComment(string controllerName, string content, int rating, int serviceId)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                Context.TbComment.Add(new CommentModel
                {
                    Content = content,
                    RatingValue = rating,
                    CommentDate = DateTime.UtcNow,
                    ServiceId = serviceId,
                    UserId = userId ?? string.Empty
                });

                await Context.SaveChangesAsync();
            }

            return RedirectToAction("Details_Services", controllerName, new { id = serviceId });
        }

        private void SetCategory(string viewBagName, int categoryId)
        {
            ViewData[viewBagName] = Context.TbService
                .Where(s => s.CategoryId == categoryId && s.IsApproved)
                .Select(s => new
                {
                    Service = s,
                    ImagePath = Context.TbServiceImge
                        .Where(img => img.ServiceId == s.Id)
                        .Select(img => img.ImagePath)
                        .FirstOrDefault()
                })
                .ToList();
        }
    }
}
