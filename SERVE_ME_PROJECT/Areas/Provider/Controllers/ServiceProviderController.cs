using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
    [Area("provider")]
    [Authorize(Roles = "provider")]
    public class ServiceProviderController : Controller
    {
        private readonly ServeMeContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public ServiceProviderController(ServeMeContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Provider/ServiceProvider
        public async Task<IActionResult> Index()
        {
            var serveMeContext = _context.TbServiceProvider.Include(s => s.User);
            return View(await serveMeContext.ToListAsync());
        }

        // GET: Provider/ServiceProvider/Details/5
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

        // GET: Provider/ServiceProvider/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.TbUser, "Id", "Email");
            return View();
        }

        // POST: Provider/ServiceProvider/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NameShop,Description,Logo,PhotoRecord,JoinDate,UserId")] ServiceProviderModel serviceProviderModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(serviceProviderModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.TbUser, "Id", "Email", serviceProviderModel.UserId);
            return View(serviceProviderModel);
        }

        // GET: Provider/ServiceProvider/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceProviderModel = await _context.TbServiceProvider.FindAsync(id);
            if (serviceProviderModel == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.TbUser, "Id", "Email", serviceProviderModel.UserId);
            return View(serviceProviderModel);
        }

        // POST: Provider/ServiceProvider/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameShop,Description,Logo,PhotoRecord,JoinDate,UserId")] ServiceProviderModel serviceProviderModel)
        {
            if (id != serviceProviderModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serviceProviderModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceProviderModelExists(serviceProviderModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.TbUser, "Id", "Email", serviceProviderModel.UserId);
            return View(serviceProviderModel);
        }

        // GET: Provider/ServiceProvider/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Provider/ServiceProvider/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serviceProviderModel = await _context.TbServiceProvider.FindAsync(id);
            if (serviceProviderModel != null)
            {
                _context.TbServiceProvider.Remove(serviceProviderModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceProviderModelExists(int id)
        {
            return _context.TbServiceProvider.Any(e => e.Id == id);
        }
        // إجراء عرض تفاصيل السحب
        [HttpGet]
        public async Task<IActionResult> WithdrawalDetails(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var withdrawal = await _context.WithdrawalRequests
                .FirstOrDefaultAsync(w => w.WithdrawalRequestId == id && w.ProviderId == userId);

            if (withdrawal == null)
            {
                return NotFound();
            }

            return View(withdrawal);
        }

        // إجراء إلغاء السحب
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelWithdrawal(int id, string cancelReason)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var withdrawal = await _context.WithdrawalRequests
                .FirstOrDefaultAsync(w => w.WithdrawalRequestId == id && w.ProviderId == userId && w.Status == "Pending");

            if (withdrawal == null)
            {
                return NotFound();
            }

            withdrawal.Status = "Cancelled";
            withdrawal.ProcessedDate = DateTime.UtcNow;

            _context.Update(withdrawal);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "تم إلغاء طلب السحب بنجاح";
            return RedirectToAction(nameof(WithdrawalHistory));
        }
        // لوحة تحكم مزود الخدمة
        public async Task<IActionResult> Dashboard()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            var wallet = await GetOrCreateWalletAsync(userId);
            var now = DateTime.UtcNow;
            var monthStart = new DateTime(now.Year, now.Month, 1);

            var model = new VmProviderWalletDashboard
            {
                Balance = wallet.CurrentBalance,
                TotalEarnings = await _context.WalletTransactions
                    .Where(t => t.WalletId == wallet.WalletId &&
                               t.TransactionType.IsCredit &&
                               t.TransactionType.Code == "SERVICE_INCOME")
                    .SumAsync(t => t.Amount),
                MonthlyEarnings = await _context.WalletTransactions
                    .Where(t => t.WalletId == wallet.WalletId &&
                               t.TransactionType.IsCredit &&
                               t.TransactionType.Code == "SERVICE_INCOME" &&
                               t.TransactionDate >= monthStart)
                    .SumAsync(t => t.Amount),
                PendingWithdrawals = await _context.WithdrawalRequests
                    .Where(w => w.ProviderId == userId && w.Status == "Pending")
                    .CountAsync(),
                RecentTransactions = await _context.WalletTransactions
                    .Where(t => t.WalletId == wallet.WalletId)
                    .OrderByDescending(t => t.TransactionDate)
                    .Take(5)
                    .Include(t => t.TransactionType)
                    .Include(t => t.Order)
                    .ToListAsync()
            };

            return View(model);
        }

        // طلبات السحب
        [HttpGet]
        public async Task<IActionResult> RequestWithdrawal()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var wallet = await GetOrCreateWalletAsync(userId);

            var model = new VmProviderWithdrawalRequest
            {
                CurrentBalance = wallet.CurrentBalance,
                AvailableBanks = GetBankList(),
                MinWithdrawalAmount = 100,
                MaxWithdrawalAmount = Math.Min(50000, wallet.CurrentBalance)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestWithdrawal(VmProviderWithdrawalRequest model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var wallet = await GetOrCreateWalletAsync(userId);

            if (!ModelState.IsValid)
            {
                model.AvailableBanks = GetBankList();
                model.CurrentBalance = wallet.CurrentBalance;
                model.MinWithdrawalAmount = 100;
                model.MaxWithdrawalAmount = Math.Min(50000, wallet.CurrentBalance);
                return View(model);
            }

            if (model.Amount > wallet.CurrentBalance)
            {
                ModelState.AddModelError("Amount", "لا يمكن سحب مبلغ أكبر من الرصيد المتاح");
                model.AvailableBanks = GetBankList();
                model.CurrentBalance = wallet.CurrentBalance;
                model.MinWithdrawalAmount = 100;
                model.MaxWithdrawalAmount = Math.Min(50000, wallet.CurrentBalance);
                return View(model);
            }

            try
            {
                var withdrawalRequest = new WithdrawalRequest
                {
                    ProviderId = userId,
                    Amount = model.Amount,
                    BankAccountDetails = $"{model.SelectedBank} - {model.AccountNumber}",
                    Status = "Pending",
                    RequestDate = DateTime.UtcNow,
                    AccountHolderName = model.AccountHolderName,
                    AccountNumber = model.AccountNumber,
                    BankName = model.SelectedBank,
                    BranchCode = model.BranchCode
                };

                _context.WithdrawalRequests.Add(withdrawalRequest);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "تم تقديم طلب السحب بنجاح وسيتم معالجته خلال 48 ساعة عمل";
                return RedirectToAction(nameof(WithdrawalHistory));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "حدث خطأ أثناء معالجة طلبك. يرجى المحاولة لاحقاً.");
                model.AvailableBanks = GetBankList();
                model.CurrentBalance = wallet.CurrentBalance;
                model.MinWithdrawalAmount = 100;
                model.MaxWithdrawalAmount = Math.Min(50000, wallet.CurrentBalance);
                return View(model);
            }
        }

        // سجل السحوبات
        public async Task<IActionResult> WithdrawalHistory(int page = 1, int pageSize = 10)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = _context.WithdrawalRequests
                .Where(w => w.ProviderId == userId)
                .OrderByDescending(w => w.RequestDate);

            var totalItems = await query.CountAsync();
            var withdrawals = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var model = new VmProviderWithdrawalHistory
            {
                Withdrawals = withdrawals,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                PageSize = pageSize
            };

            return View(model);
        }

        // سجل الأرباح
        public async Task<IActionResult> EarningsHistory(int page = 1, int pageSize = 10)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var wallet = await GetOrCreateWalletAsync(userId);

            var query = _context.WalletTransactions
                .Where(t => t.WalletId == wallet.WalletId &&
                           t.TransactionType.IsCredit &&
                           t.TransactionType.Code == "SERVICE_INCOME")
                .Include(t => t.TransactionType)
                .Include(t => t.Order)
                .OrderByDescending(t => t.TransactionDate);

            var totalItems = await query.CountAsync();
            var transactions = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var model = new VmProviderEarningsHistory
            {
                Earnings = transactions,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                PageSize = pageSize,
                TotalEarnings = await query.SumAsync(t => t.Amount)
            };

            return View(model);
        }

        // تفاصيل المعاملة
        public async Task<IActionResult> TransactionDetails(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var wallet = await GetOrCreateWalletAsync(userId);

            var transaction = await _context.WalletTransactions
                .Include(t => t.TransactionType)
                .Include(t => t.Order)
                .ThenInclude(o => o.Service)
                .Include(t => t.Order)
                .ThenInclude(o => o.User)
                .FirstOrDefaultAsync(t => t.TransactionId == id && t.WalletId == wallet.WalletId);

            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        #region Helper Methods
        private async Task<Wallet> GetOrCreateWalletAsync(string userId)
        {
            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);

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

            return wallet;
        }

        private List<BankInfo> GetBankList()
        {
            return new List<BankInfo>
            {
                new BankInfo { Id = "AlRajhi", Name = "البنك الأهلي" },
                new BankInfo { Id = "Alinma", Name = "الإنماء" },
                new BankInfo { Id = "Riyad", Name = "رياض" },
                new BankInfo { Id = "SABB", Name = "ساب" },
                new BankInfo { Id = "AlBilad", Name = "البلاد" }
            };
        }
        #endregion

        // طلبات الخدمات (لمزود الخدمة)
        [Authorize(Roles = "provider")]
        public async Task<IActionResult> ServiceRequests(int page = 1, int pageSize = 10)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var provider = await _userManager.FindByIdAsync(userId);

            ViewBag.ProviderFullName = provider.FullName;
            ViewBag.ProviderImage = string.IsNullOrEmpty(provider.ProfileImagePath)
                ? "~/provider/images/pofl.jpg"
                : provider.ProfileImagePath;

            var query = _context.TbOrder
                .Where(o => o.Service.UserId == userId)
                .Include(o => o.Service)
                .Include(o => o.User)
                .Include(o => o.OrderStat)
                .OrderByDescending(o => o.OrderDate);

            var totalItems = await query.CountAsync();
            var orders = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var model = new VmServiceRequests
            {
                Orders = orders,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                PageSize = pageSize
            };

            return View(model);
        }


        [HttpGet]
        [Authorize(Roles = "provider")]
        public async Task<IActionResult> OrderDetails(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var order = await _context.TbOrder
                .Include(o => o.Service)
                .Include(o => o.User)
                .Include(o => o.OrderStat)
                .FirstOrDefaultAsync(o => o.Id == id && o.Service.UserId == userId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }


        // قبول طلب الخدمة (لمزود الخدمة)
        [HttpPost]
        [Authorize(Roles = "provider")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptOrder(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var order = await _context.TbOrder
                .Include(o => o.Service)
                    .ThenInclude(s => s.TypeService)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == id && o.Service.UserId == userId && o.StatOrderId == 1); // 1 = Pending

            if (order == null)
                return NotFound();

            var customerWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == order.UserId);
            var providerWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);

            if (customerWallet == null || providerWallet == null)
                return NotFound();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // تحديث حالة الطلب إلى "مقبول"
                order.StatOrderId = 2; // 2 = Accepted
                _context.Update(order);

                // تحويل المبلغ إلى مزود الخدمة
                providerWallet.CurrentBalance += order.Price;
                providerWallet.LastUpdated = DateTime.UtcNow;
                _context.Update(providerWallet);

                // تحديث المعاملة الأصلية من العميل إلى "مكتملة"
                var originalTransaction = await _context.WalletTransactions
                    .FirstOrDefaultAsync(t => t.OrderId == order.Id && t.Status == "Pending");

                if (originalTransaction != null)
                {
                    originalTransaction.Status = "Completed";
                    _context.Update(originalTransaction);
                }

                // إعداد وصف المعاملة بناءً على نوع الخدمة (بيع أو تأجير)
                string description = $"دفعة مقابل خدمة #{order.Service.Id} - {order.Service.Name}";

                if (order.Service.TypeService?.Name == "تأجير")
                {
                    description += $" (تأجير {order.Quantity} × {order.RentalDays} يوم، من {order.RentalStartDate?.ToString("yyyy-MM-dd")})";
                }
                else
                {
                    description += $" (شراء {order.Quantity} وحدة)";
                }

                // تسجيل معاملة الدخل للمزود
                var providerTransaction = new WalletTransaction
                {
                    WalletId = providerWallet.WalletId,
                    Amount = order.Price,
                    TransactionTypeId = 4, // 4 = خدمة واردة
                    BalanceBefore = providerWallet.CurrentBalance - order.Price,
                    BalanceAfter = providerWallet.CurrentBalance,
                    Description = description,
                    TransactionDate = DateTime.UtcNow,
                    Status = "Completed",
                    OrderId = order.Id
                };

                _context.WalletTransactions.Add(providerTransaction);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                TempData["SuccessMessage"] = "تم قبول الطلب وتحويل المبلغ إلى حسابك.";
                return RedirectToAction(nameof(ServiceRequests));
            }
            catch
            {
                await transaction.RollbackAsync();
                TempData["ErrorMessage"] = "حدث خطأ أثناء معالجة الطلب. لم يتم تنفيذ أي عملية مالية.";
                return RedirectToAction(nameof(ServiceRequests));
            }
        }


        // رفض طلب الخدمة (لمزود الخدمة)
        [HttpPost]
        [Authorize(Roles = "provider")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectOrder(int id, string rejectionReason)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await _context.TbOrder
                .Include(o => o.Service)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == id && o.Service.UserId == userId && o.StatOrderId == 1); // 1 = Pending

            if (order == null)
            {
                return NotFound();
            }

            var customerWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == order.UserId);

            if (customerWallet == null)
            {
                return NotFound();
            }

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                // تحديث حالة الطلب
                order.StatOrderId = 3; // 3 = Rejected
                _context.Update(order);

                // إعادة المبلغ إلى العميل
                customerWallet.CurrentBalance += order.Price;
                customerWallet.LastUpdated = DateTime.UtcNow;
                _context.Update(customerWallet);

                // تحديث حالة المعاملة الأصلية من Pending إلى Refunded
                var originalTransaction = await _context.WalletTransactions
                    .FirstOrDefaultAsync(t => t.OrderId == order.Id && t.Status == "Pending");

                if (originalTransaction != null)
                {
                    originalTransaction.Status = "Refunded";
                    originalTransaction.Description += $" - تم الرفض بسبب: {rejectionReason}";
                    _context.Update(originalTransaction);
                }

                // تسجيل معاملة الاسترداد
                var refundTransaction = new WalletTransaction
                {
                    WalletId = customerWallet.WalletId,
                    Amount = order.Price,
                    TransactionTypeId = 7, // REFUND
                    BalanceBefore = customerWallet.CurrentBalance - order.Price,
                    BalanceAfter = customerWallet.CurrentBalance,
                    Description = $"استرداد مبلغ لطلب مرفوض #{order.Id} - السبب: {rejectionReason}",
                    TransactionDate = DateTime.UtcNow,
                    Status = "Completed",
                    OrderId = order.Id
                };

                _context.WalletTransactions.Add(refundTransaction);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // إرسال إشعار للعميل (يمكن تنفيذه عبر SignalR أو البريد الإلكتروني)
                // await SendNotificationToCustomer(order.UserId, order.Id, rejectionReason);

                TempData["SuccessMessage"] = "تم رفض الطلب بنجاح وتم إعادة المبلغ إلى العميل";
                return RedirectToAction(nameof(ServiceRequests));
            }
            catch
            {
                await transaction.RollbackAsync();
                TempData["ErrorMessage"] = "حدث خطأ أثناء معالجة الطلب";
                return RedirectToAction(nameof(ServiceRequests));
            }
        }
    }
}

