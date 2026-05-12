using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SERVE_ME_PROJECT.Data;
using SERVE_ME_PROJECT.Models;

namespace SERVE_ME_PROJECT.Areas.AdminControlPanel.Controllers
{
    [Area("AdminControlPanel")]
    public class ServiceProviderModelsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ServeMeContext _context;

        public ServiceProviderModelsController(UserManager<ApplicationUser> userManager, ServeMeContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        //--------------------------------
        public async Task<IActionResult> Index2()
        {
            return View(await _context.TbService.ToListAsync());
        }

        //--------------------------------
        // GET: AdminControlPanel/ServiceProviderModels
        public async Task<IActionResult> Index()
        {
            var serveMeContext = _context.TbServiceProvider.Include(s => s.User);
            return View(await serveMeContext.ToListAsync());
        }

        // GET: AdminControlPanel/ServiceProviderModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceProviderModel = await _context.TbServiceProvider
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serviceProviderModel == null)
            {
                return NotFound();
            }

            return View(serviceProviderModel);
        }

        private bool ServiceProviderModelExists(int id)
        {
            return _context.TbServiceProvider.Any(e => e.Id == id);
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Requests()
        {
            var requests = await _context.TbServiceProvider
                .Include(r => r.User)
                .Where(r => r.Status == "قيد المراجعة")
                .ToListAsync();

            return View(requests);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> ApproveProvider(int requestId)
        {
            var request = await _context.TbServiceProvider.Include(r => r.User).FirstOrDefaultAsync(r => r.Id == requestId);
            if (request == null) return NotFound();

            request.Status = "مقبول";
            await _userManager.AddToRoleAsync(request.User, "provider");

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> ReApproveProvider(int requestId)
        {
            var request = await _context.TbServiceProvider
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == requestId);

            if (request == null) return NotFound();

            // تحديث الحالة
            request.Status = "مقبول";

            var user = request.User;

            // جلب الأدوار الحالية للمستخدم
            var roles = await _userManager.GetRolesAsync(user);

            // إزالة كل الأدوار السابقة
            await _userManager.RemoveFromRolesAsync(user, roles);

            // إضافة دور "customer" فقط (بدون مسافة زائدة)
            await _userManager.AddToRoleAsync(user, "customer ");

            // حفظ التغييرات
            await _context.SaveChangesAsync();

            return RedirectToAction("Requests");
        }

        // عرض الخدمات غير المعتمدة
        public async Task<IActionResult> PendingServices(string searchTerm)
        {
            var query = _context.TbService
                .Include(s => s.User)
                .Include(s => s.Category)
                .Where(s => !s.IsApproved) // جلب الخدمات غير الموافق عليها
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(s =>
                    s.Name.Contains(searchTerm) ||
                    s.User.UserName.Contains(searchTerm)
                );
            }

            var services = await query.ToListAsync();
            return View(services);
        }



        // تفاصيل الخدمة
        public async Task<IActionResult> DetailsService(int id)
        {
            var service = await _context.TbService
                .Include(s => s.User)
                .Include(s => s.City)
                .Include(s => s.Category)
                .Include(s => s.TypeService)
                .Include(s => s.Images)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (service == null)
                return NotFound();

            return View(service);
        }

        // اعتماد خدمة
        public async Task<IActionResult> Approve(int id)
        {
            var service = await _context.TbService.FirstOrDefaultAsync(s => s.Id == id);
            if (service == null) return NotFound();

            service.IsApproved = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(PendingServices));
        }

        // رفض خدمة
        public async Task<IActionResult> Reject(int id)
        {
            var service = await _context.TbService.FindAsync(id);
            if (service == null) return NotFound();

            _context.TbService.Remove(service);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(PendingServices));
        }
    }
}
