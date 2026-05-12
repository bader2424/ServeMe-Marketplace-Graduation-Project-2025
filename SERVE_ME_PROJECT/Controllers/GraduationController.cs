using Microsoft.AspNetCore.Mvc;
using SERVE_ME_PROJECT.Data;
using SERVE_ME_PROJECT.ViewModel;
using SERVE_ME_PROJECT.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace SERVE_ME_PROJECT.Controllers
{
    public class GraduationController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<GraduationController> _logger;
        private readonly ServeMeContext _context;

        // ✅ تم تمرير UserManager في الكونستركتر
        public GraduationController(ILogger<GraduationController> logger, ServeMeContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager; // ✅ تخزينه داخل المتغير الخاص
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            VmServices vmservices = new VmServices();

            vmservices.BannerModel = _context.TbBanner.ToList();
            vmservices.ServiceModel = _context.TbService.ToList();


            ViewBag.Category1 = _context.TbService.Where(s => s.CategoryId == 45)
                .Select(s => new
                {
                    Service = s,
                    ImagePath = _context.TbServiceImge
                        .Where(img => img.ServiceId == s.Id)
                        .Select(img => img.ImagePath)
                        .FirstOrDefault()
                })
                .ToList();
            ViewBag.Category2 = _context.TbService.Where(s => s.CategoryId == 46)
                   .Select(s => new
                   {
                       Service = s,
                       ImagePath = _context.TbServiceImge
                        .Where(img => img.ServiceId == s.Id)
                        .Select(img => img.ImagePath)
                        .FirstOrDefault()
                   })
                   .ToList();
            ViewBag.Category3 = _context.TbService.Where(s => s.CategoryId == 47)
                .Select(s => new
                    {
                        Service = s,
                        ImagePath = _context.TbServiceImge
                        .Where(img => img.ServiceId == s.Id)
                        .Select(img => img.ImagePath)
                        .FirstOrDefault()         })
                .ToList();
    
            ViewBag.Category4 = _context.TbService.Where(s => s.CategoryId == 48)
                .Select(s => new
                {
                    Service = s,
                    ImagePath = _context.TbServiceImge
                        .Where(img => img.ServiceId == s.Id)
                        .Select(img => img.ImagePath)
                        .FirstOrDefault()
                })
                .ToList();
            ViewBag.Category5 = _context.TbService.Where(s => s.CategoryId == 49)
                .Select(s => new
                {
                    Service = s,
                    ImagePath = _context.TbServiceImge
                        .Where(img => img.ServiceId == s.Id)
                        .Select(img => img.ImagePath)
                        .FirstOrDefault()
                })
                .ToList();
            ViewBag.Category6 = _context.TbService.Where(s => s.CategoryId == 50)
                .Select(s => new
                {
                    Service = s,
                    ImagePath = _context.TbServiceImge
                        .Where(img => img.ServiceId == s.Id)
                        .Select(img => img.ImagePath)
                        .FirstOrDefault()
                })
                .ToList();
            ViewBag.Category7 = _context.TbService.Where(s => s.CategoryId == 51)
                .Select(s => new
                {
                    Service = s,
                    ImagePath = _context.TbServiceImge
                        .Where(img => img.ServiceId == s.Id)
                        .Select(img => img.ImagePath)
                        .FirstOrDefault()
                })
                .ToList();
            ViewBag.Category8 = _context.TbService.Where(s => s.CategoryId == 52)
                .Select(s => new
                {
                    Service = s,
                    ImagePath = _context.TbServiceImge
                        .Where(img => img.ServiceId == s.Id)
                        .Select(img => img.ImagePath)
                        .FirstOrDefault()
                })
                .ToList();
            ViewBag.Category9 = _context.TbService.Where(s => s.CategoryId == 53)
                .Select(s => new
                {
                    Service = s,
                    ImagePath = _context.TbServiceImge
                        .Where(img => img.ServiceId == s.Id)
                        .Select(img => img.ImagePath)
                        .FirstOrDefault()
                })
                .ToList();
            

            return View(vmservices);

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Services(int categoryId, int page = 1)
        {
            int pageSize = 3;

            var servicesQuery = _context.TbService
                .Where(s => s.CategoryId == categoryId && s.IsApproved)
                .Include(s => s.Images)
                .Include(s => s.Category);

            var totalCount = servicesQuery.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var services = servicesQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;
            ViewBag.CategoryId = categoryId;

            return View(services);
        }

        [AllowAnonymous]
        public IActionResult Details_Services(int id)
        {
            var service = _context.TbService
                .Where(s => s.Id == id)
                .Include(s => s.Images) // تحميل الصور المرتبطة بالخدمة
                .Include(s => s.Comments)
                    .ThenInclude(c => c.User) //     لتحميل بيانات المستخدم
                .FirstOrDefault();

            if (service == null)
            {
                return NotFound();
            }

            // Debugging: تأكد من الصور الموجودة
            if (service.Images == null || !service.Images.Any())
            {
                Console.WriteLine("No images found for this service.");
            }

            return View(service); // تمرير الخدمة مع التعليقات والصور
        }
        // أكشن لإضافة تعليق
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddComment(string content, int rating, int serviceId)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);

                var comment = new CommentModel
                {
                    Content = content,
                    RatingValue = rating,
                    CommentDate = DateTime.UtcNow,
                    ServiceId = serviceId,
                    UserId = userId
                };

                _context.TbComment.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details_Services", new { id = serviceId });
            }

            return RedirectToAction("Details_Services", new { id = serviceId });
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Services(string[] cities, int? categoryId, int page = 1)
        {
            // جلب البيانات من قاعدة البيانات مع تصفية حسب المدينة
            var query = _context.TbService.Include(s => s.City).AsQueryable(); // تأكد من أنك تتضمن الكائن City المرتبط بالخدمة

            // تصفية حسب المدينة
            if (cities != null && cities.Length > 0)
            {
                // إذا كان خيار "الكل" غير محدد، نُصفِّي حسب المدن المختارة
                if (!cities.Contains("الكل"))
                {
                    query = query.Where(s => cities.Contains(s.City.Name)); // تصفية حسب أسماء المدن
                }
            }

            // تصفية حسب التصنيف (إن وجد)
            if (categoryId.HasValue)
            {
                query = query.Where(s => s.CategoryId == categoryId.Value);
            }

            // تطبيق التصفح (Pagination)
            int pageSize = 9; // عدد المنتجات في كل صفحة
            var totalItems = query.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var services = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // تمرير البيانات إلى الـ View
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;
            ViewBag.CategoryId = categoryId;
            ViewBag.Cities = _context.TbCity.Select(c => c.Name).ToList(); // جلب المدن المرتبطة بالخدمة

            return View(services);
        }

    }
}
