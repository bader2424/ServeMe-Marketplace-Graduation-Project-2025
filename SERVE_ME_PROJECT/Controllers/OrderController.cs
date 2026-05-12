using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using SERVE_ME_PROJECT.Models;
using SERVE_ME_PROJECT.Data;
using System.Security.Claims;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using SERVE_ME_PROJECT.ViewModel;


namespace SERVE_ME_PROJECT.Controllers
{
    public class OrderController : Controller
    {
        private readonly ServeMeContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public OrderController(ServeMeContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // طلباتي (للعميل)
        public async Task<IActionResult> MyOrders(int page = 1, int pageSize = 10)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = _context.TbOrder
                .Where(o => o.UserId == userId)
                .Include(o => o.Service)
                .Include(o => o.OrderStat)
                .OrderByDescending(o => o.OrderDate);

            var totalItems = await query.CountAsync();
            var orders = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var model = new VmMyOrders
            {
                Orders = orders,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                PageSize = pageSize
            };

            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var order = await _context.TbOrder
                .Include(o => o.Service)
                .Include(o => o.OrderStat)
                .Include(o => o.Service.User) // إذا كنت تريد عرض مزود الخدمة
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }


    }

}
