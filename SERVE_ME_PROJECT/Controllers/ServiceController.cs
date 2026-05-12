using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SERVE_ME_PROJECT.Data;
using SERVE_ME_PROJECT.Models;
using SERVE_ME_PROJECT.ViewModel;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SERVE_ME_PROJECT.Controllers
{
    public class ServiceController : Controller
    {
        private readonly ServeMeContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ServiceController(ServeMeContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // عرض تفاصيل الخدمة مع نموذج الطلب
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Order(int id)
        {
            var service = await _context.TbService
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == id && s.IsApproved);

            if (service == null)
            {
                return NotFound();
            }

            var model = new VmServiceOrder  
            {
                ServiceId = service.Id,
                ServiceName = service.Name,
                ProviderName = service.User.FullName,
                Price = service.Price,
                Description = service.Description
            };

            return View(model);
        }

        // معالجة طلب الخدمة
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Order(VmServiceOrder model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            var service = await _context.TbService
                .Include(s => s.User)
                .Include(s => s.TypeService) // لجلب نوع الخدمة
                .FirstOrDefaultAsync(s => s.Id == model.ServiceId);

            if (service == null)
                return NotFound();

            // تعبئة بيانات العرض
            model.ServiceName = service.Name;
            model.Description = service.Description;
            model.Price = service.Price;
            model.ProviderName = service.User?.FullName ?? "مزود غير معروف";

            if (model.Quantity < 1)
            {
                ModelState.AddModelError(nameof(model.Quantity), "الكمية يجب أن تكون 1 على الأقل.");
                return View(model);
            }

            var customerWallet = await GetOrCreateWalletAsync(userId);
            var providerWallet = await GetOrCreateWalletAsync(service.UserId);

            // حساب السعر حسب النوع
            decimal totalPrice;

            if (service.TypeService?.Name == "تأجير")
            {
                if (!model.RentalStartDate.HasValue || !model.RentalDays.HasValue || model.RentalDays < 1)
                {
                    ModelState.AddModelError("", "يرجى تحديد تاريخ البدء وعدد أيام التأجير بشكل صحيح.");
                    return View(model);
                }

                totalPrice = service.Price * model.Quantity * model.RentalDays.Value;
            }
            else // بيع
            {
                totalPrice = service.Price * model.Quantity;
            }

            if (customerWallet.CurrentBalance < totalPrice)
            {
                ModelState.AddModelError("", "رصيدك غير كافٍ لإتمام هذه العملية.");
                return View(model);
            }

            try
            {
                customerWallet.CurrentBalance -= totalPrice;
                customerWallet.LastUpdated = DateTime.UtcNow;

                var order = new OrderModel
                {
                    ServiceId = service.Id,
                    UserId = userId,
                    Price = totalPrice,
                    Quantity = model.Quantity,
                    OrderDate = DateTime.UtcNow,
                    StatOrderId = 1,

                    // بيانات التأجير (قد تكون null إن كانت الخدمة بيع)
                    RentalStartDate = model.RentalStartDate,
                    RentalDays = model.RentalDays
                };

                _context.TbOrder.Add(order);
                await _context.SaveChangesAsync();

                var transaction = new WalletTransaction
                {
                    WalletId = customerWallet.WalletId,
                    Amount = totalPrice,
                    TransactionTypeId = 3,
                    BalanceBefore = customerWallet.CurrentBalance + totalPrice,
                    BalanceAfter = customerWallet.CurrentBalance,
                    Description = $"طلب خدمة: {service.Name} من {model.ProviderName}",
                    TransactionDate = DateTime.UtcNow,
                    Status = "Pending",
                    OrderId = order.Id
                };

                _context.WalletTransactions.Add(transaction);
                _context.Update(customerWallet);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "تم إرسال طلبك لمزود الخدمة بنجاح!";
                return RedirectToAction("MyOrders", "Order");
            }
            catch
            {
                ModelState.AddModelError("", "حدث خطأ أثناء تنفيذ الطلب.");
                return View(model);
            }
        }

        //انشاء المحفة اذا لم تكن موجودة
        private async Task<Wallet> GetOrCreateWalletAsync(string userId)
        {
            var wallet = await _context.Wallets
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (wallet == null)
            {
                wallet = new Wallet
                {
                    UserId = userId,
                    CurrentBalance = 0,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdated = DateTime.UtcNow,
                    Status = "Active"
                };
                _context.Wallets.Add(wallet);
                await _context.SaveChangesAsync();
            }

            // إعادة تحميل بيانات المحفظة محدثة
            return await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
        }

    }
}
