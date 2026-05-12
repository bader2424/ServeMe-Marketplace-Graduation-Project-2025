using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SERVE_ME_PROJECT.Data;
using SERVE_ME_PROJECT.Models;
using SERVE_ME_PROJECT.ViewModel;

namespace SERVE_ME_PROJECT.Areas.AdminControlPanel.Controllers
{
    [Area("AdminControlPanel")]
    public class HomeController : Controller
    {
        private readonly ServeMeContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public HomeController(ServeMeContext context, UserManager<ApplicationUser> userManager,
                      RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public IActionResult Index()
        {
            var vm = new VmStatistics();

            try
            {
                vm.TotalOrders = _context.TbOrder.Count();
           //    vm.TotalCustomers = _context.TbUser.Count(u => u.Role == "Customer");
            //    vm.TotalProviders = _context.TbUser.Count(u => u.Role == "Provider");
                vm.TotalServices = _context.TbService.Count();
                vm.TotalOrderValue = _context.TbOrder
                    .Select(o => (decimal?)(o.Price * o.Quantity))
                    .Sum() ?? 0m;
            }
            catch (Exception ex)
            {
                // يمكن تسجيل الخطأ هنا إذا لزم
            }

            return View(vm);
        }

        public IActionResult Dashboard()
        {
            return View();
        }


        public async Task<IActionResult> Statistics()
        {  // جلب إجمالي عدد الطلبات من جدول Orders
            var totalOrders = _context.TbOrder.Count();

            // جلب إجمالي عدد العملاء من جدول Users (المستخدمين)
            var totalCustomers = _context.Users.Count();

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
        public async Task<IActionResult> Users()
        {
            var users = _userManager.Users.OfType<ApplicationUser>().ToList();
            return View(users);

        }
    }
}
