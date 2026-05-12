using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SERVE_ME_PROJECT.Data;
using SERVE_ME_PROJECT.Models;
using SERVE_ME_PROJECT.ViewModel;

namespace SERVE_ME_PROJECT.Areas.Provider.Controllers
{
    [Area("Provider")]
    [Authorize(Roles = "provider")]
    public class HomeController : Controller
    {
        private readonly ServeMeContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ServeMeContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        // الأكشن لعرض الإحصائيات
        public IActionResult Staticts()
        {
            // جلب إجمالي عدد الطلبات من جدول Orders
            var totalOrders = _context.TbOrder.Count()  ;

            // جلب إجمالي عدد العملاء من جدول Users (المستخدمين)
            var totalCustomers = _context.Users.Count() ;

            // جلب إجمالي عدد مقدمي الخدمات من جدول ServiceProviders
            var totalProviders = _context.TbServiceProvider.Count();

            // جلب إجمالي عدد الخدمات من جدول Services
            var totalServices = _context.TbService.Count();

            // جلب إجمالي قيمة الطلبات من جدول Orders
            var totalOrderValue = _context.TbOrder.Sum(o => o.Price);

            // تعريف كائن من نوع StatisticsViewModel وتمرير الإحصائيات إليها
            var stats = new VmStatisticsProviders
            {
                TotalOrders = totalOrders,
                TotalCustomers = totalCustomers,
                TotalProviders = totalProviders,
                TotalServices = totalServices,
                TotalOrderValue = totalOrderValue
            };

            // إرجاع الـ View مع تمثيل الـ ViewModel (البيانات)
            return View(stats);
        }
        public IActionResult Comments()
        {
            return View();
        }

        public async Task<IActionResult> Profile()
        {
            var userId = _userManager.GetUserId(User);
            var orders = await _context.TbOrder
                .Include(o => o.Service)
                .Include(o => o.OrderStat)
                .Where(o => o.UserId == userId)
                .ToListAsync();

            return View(orders);
        }

        public IActionResult Ordres()
        {
            return View();
        }

        // عرض جميع الخدمات الخاصة بمزود الخدمة
        public async Task<IActionResult> MyServices()
        {
            var user = await _userManager.GetUserAsync(User);

            var services = await _context.TbService
                .Where(s => s.UserId == user.Id)
                .Include(s => s.Category)
                .Include(s => s.City)
                .Include(s => s.TypeService)
                .ToListAsync();

            return View(services);
        }

        // عرض نموذج إضافة خدمة جديدة
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.TbCategory, "Id", "Name");
            ViewBag.Cities = new SelectList(_context.TbCity, "Id", "Name");
            ViewBag.ServiceTypes = new SelectList(_context.TbTypeservice, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ServiceModel model, List<IFormFile> Images)
        {
            if (ModelState.IsValid)
            {

                var user = await _userManager.GetUserAsync(User);
                model.UserId = user.Id;
                model.IsApproved = false;

                _context.TbService.Add(model);
                await _context.SaveChangesAsync();

                // إضافة الصور
                foreach (var image in Images)
                {
                    if (image.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }

                        _context.TbServiceImge.Add(new ServiceImgeModel
                        {
                            ServiceId = model.Id,
                            ImagePath = "/img/" + fileName
                        });
                    }
                }
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(MyServices));
            }

            ViewBag.Categories = new SelectList(_context.TbCategory, "Id", "Name", model.CategoryId);
            ViewBag.Cities = new SelectList(_context.TbCity, "Id", "Name", model.CityId);
            ViewBag.ServiceTypes = new SelectList(_context.TbTypeservice, "Id", "Name", model.TypeServiceId);
            return View(model);
        }

        // عرض تفاصيل خدمة واحدة
        public async Task<IActionResult> Details(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            var service = await _context.TbService
                .Include(s => s.Images)
                .Include(s => s.City)
                .Include(s => s.Category)
                .Include(s => s.TypeService)
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == user.Id);

            if (service == null) return NotFound();

            return View(service);
        }

        // عرض نموذج تعديل خدمة
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            var service = await _context.TbService
            .FirstOrDefaultAsync(s => s.Id == id && s.UserId == user.Id);

            if (service == null) return NotFound();

            ViewBag.Categories = new SelectList(_context.TbCategory, "Id", "Name", service.CategoryId);
            ViewBag.Cities = new SelectList(_context.TbCity, "Id", "Name", service.CityId);
            ViewBag.ServiceTypes = new SelectList(_context.TbTypeservice, "Id", "Name", service.TypeServiceId);

            return View(service);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ServiceModel model)
        {
            if (ModelState.IsValid)
            {
                var service = await _context.TbService.FindAsync(model.Id);

                if (service == null) return NotFound();

                service.Name = model.Name;
                service.Description = model.Description;
                service.CategoryId = model.CategoryId;
                service.CityId = model.CityId;
                service.TypeServiceId = model.TypeServiceId;

                service.IsApproved = false; // تعديل الخدمة يجعلها تنتظر موافقة جديدة

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MyServices));
            }

            ViewBag.Categories = new SelectList(_context.TbCategory, "Id", "Name", model.CategoryId);
            ViewBag.Cities = new SelectList(_context.TbCity, "Id", "Name", model.CityId);
            ViewBag.ServiceTypes = new SelectList(_context.TbTypeservice, "Id", "Name", model.TypeServiceId);

            return View(model);
        }


        public async Task<IActionResult> ProfileEdit()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> ProfileEdit(ApplicationUser model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null) return NotFound();

            if (ModelState.IsValid)
            {
                user.FullName = model.FullName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.City = model.City;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                    return View("Index");

                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }
    }
}
    

