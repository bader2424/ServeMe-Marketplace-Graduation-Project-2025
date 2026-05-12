using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SERVE_ME_PROJECT.Data;
using SERVE_ME_PROJECT.Models;
using SERVE_ME_PROJECT.ViewModel;

namespace SERVE_ME_PROJECT.Areas.AdminControlPanel.Controllers
{
    [Area("AdminControlPanel")]
    //[Authorize(Roles = "Admin")]
    public class TypeserviceController : Controller
    {
        private readonly ServeMeContext _context;

        public TypeserviceController(ServeMeContext context)
        {
            _context = context;
        }

        // GET: AdminControlPanel/Typeservice
        public async Task<IActionResult> Index()
        {
            return View(await _context.TbTypeservice.ToListAsync());
        }

        // GET: AdminControlPanel/Typeservice/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeserviceModel = await _context.TbTypeservice
                .FirstOrDefaultAsync(m => m.Id == id);
            if (typeserviceModel == null)
            {
                return NotFound();
            }

            return View(typeserviceModel);
        }

        // GET: AdminControlPanel/Typeservice/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminControlPanel/Typeservice/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TypeserviceModel typeserviceModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(typeserviceModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(typeserviceModel);
        }

        // GET: AdminControlPanel/Typeservice/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeserviceModel = await _context.TbTypeservice.FindAsync(id);
            if (typeserviceModel == null)
            {
                return NotFound();
            }
            return View(typeserviceModel);
        }

        // POST: AdminControlPanel/Typeservice/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] TypeserviceModel typeserviceModel)
        {
            if (id != typeserviceModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(typeserviceModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeserviceModelExists(typeserviceModel.Id))
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
            return View(typeserviceModel);
        }

        // GET: AdminControlPanel/Typeservice/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeserviceModel = await _context.TbTypeservice
                .FirstOrDefaultAsync(m => m.Id == id);
            if (typeserviceModel == null)
            {
                return NotFound();
            }

            return View(typeserviceModel);
        }

        // POST: AdminControlPanel/Typeservice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var typeserviceModel = await _context.TbTypeservice.FindAsync(id);
            if (typeserviceModel != null)
            {
                _context.TbTypeservice.Remove(typeserviceModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TypeserviceModelExists(int id)
        {
            return _context.TbTypeservice.Any(e => e.Id == id);
        }

        // لوحة تحكم المشرف للمحفظة
        [HttpGet("Dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            var model = new VmAdminWalletDashboard
            {
                TotalBalance = await _context.Wallets.SumAsync(w => w.CurrentBalance),
                PendingDepositsCount = await _context.DepositRequests.CountAsync(d => d.Status == "Pending"),
                PendingWithdrawalsCount = await _context.WithdrawalRequests.CountAsync(w => w.Status == "Pending"),
                RecentTransactions = await _context.WalletTransactions
                    .Include(t => t.Wallet)
                    .ThenInclude(w => w.User)
                    .Include(t => t.TransactionType)
                    .OrderByDescending(t => t.TransactionDate)
                    .Take(10)
                    .ToListAsync(),
                SystemEarnings = await _context.WalletTransactions
                    .Where(t => t.TransactionType.Code == "FEE")
                    .SumAsync(t => t.Amount)
            };

            return View(model);
        }

        // إدارة طلبات الإيداع
        [HttpGet("Deposits")]
        public async Task<IActionResult> ManageDeposits(string status = "Pending", int page = 1)
        {
            var pageSize = 15;
            var query = _context.DepositRequests
                .Include(d => d.User)
                .Where(d => d.Status == status)
                .OrderByDescending(d => d.RequestDate);

            var totalItems = await query.CountAsync();
            var deposits = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var model = new VmAdminDeposits
            {
                Deposits = deposits,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                CurrentStatus = status,
                StatusList = new List<string> { "Pending", "Approved", "Rejected" }
            };

            return View(model);
        }

        // تفاصيل طلب الإيداع
        [HttpGet("Deposits/Details/{id}")]
        public async Task<IActionResult> DepositDetails(int id)
        {
            var deposit = await _context.DepositRequests
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.DepositRequestId == id);

            if (deposit == null)
            {
                return NotFound();
            }

            return View(deposit);
        }

        // معالجة طلب الإيداع
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessDeposit(int id, string action, string adminNotes)
        {
            var deposit = await _context.DepositRequests
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.DepositRequestId == id);

            if (deposit == null)
                return NotFound();

            if (deposit.Status == "Approved")
            {
                TempData["ErrorMessage"] = "تمت الموافقة على هذا الطلب مسبقًا.";
                return RedirectToAction(nameof(ManageDeposits), new { status = "Pending" });
            }

            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == deposit.UserId);

            if (action == "approve")
            {
                deposit.Status = "Approved";
                deposit.ProcessedDate = DateTime.UtcNow;
                deposit.AdminNotes = adminNotes;

                if (wallet == null)
                {
                    wallet = new Wallet
                    {
                        UserId = deposit.UserId,
                        CurrentBalance = deposit.Amount,
                        CreatedAt = DateTime.UtcNow,
                        LastUpdated = DateTime.UtcNow,
                        Status = "Active"
                    };
                    _context.Wallets.Add(wallet);
                }
                else
                {
                    wallet.CurrentBalance += deposit.Amount;
                    wallet.LastUpdated = DateTime.UtcNow;
                }

                var transaction = new WalletTransaction
                {
                    Wallet = wallet,
                    TransactionTypeId = 1,
                    Amount = deposit.Amount,
                    BalanceBefore = wallet.CurrentBalance - deposit.Amount,
                    BalanceAfter = wallet.CurrentBalance,
                    Description = $"إيداع معتمد من الإدارة - رقم الطلب: {deposit.DepositRequestId}",
                    TransactionDate = DateTime.UtcNow,
                    Status = "Completed",
                    DepositRequestId = deposit.DepositRequestId
                };
                _context.WalletTransactions.Add(transaction);

                TempData["SuccessMessage"] = "تمت الموافقة على طلب الإيداع وتم تحديث الرصيد.";
            }
            else if (action == "reject")
            {
                deposit.Status = "Rejected";
                deposit.ProcessedDate = DateTime.UtcNow;
                deposit.AdminNotes = adminNotes;

                TempData["SuccessMessage"] = "تم رفض طلب الإيداع.";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ManageDeposits), new { status = "Pending" });
        }


        // إدارة طلبات السحب
        [HttpGet("Withdrawals")]
        public async Task<IActionResult> ManageWithdrawals(string status = "Pending", int page = 1)
        {
            var pageSize = 15;
            var query = _context.WithdrawalRequests
                .Include(w => w.Provider)
                .Where(w => w.Status == status)
                .OrderByDescending(w => w.RequestDate);

            var totalItems = await query.CountAsync();
            var withdrawals = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var model = new VmAdminWithdrawals
            {
                Withdrawals = withdrawals,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                CurrentStatus = status,
                StatusList = new List<string> { "Pending", "Approved", "Rejected", "Completed" }
            };

            return View(model);
        }

        // تفاصيل طلب السحب
        [HttpGet("Withdrawals/Details/{id}")]
        public async Task<IActionResult> WithdrawalDetails(int id)
        {
            var withdrawal = await _context.WithdrawalRequests
                .Include(w => w.Provider)
                .ThenInclude(p => p.Wallet)
                .FirstOrDefaultAsync(w => w.WithdrawalRequestId == id);

            if (withdrawal == null)
            {
                return NotFound();
            }

            return View(withdrawal);
        }

        // معالجة طلب السحب
        [HttpPost("Withdrawals/Process")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessWithdrawal(int id, string action, string adminNotes)
        {
            var withdrawal = await _context.WithdrawalRequests
                .Include(w => w.Provider)
                .ThenInclude(p => p.Wallet)
                .FirstOrDefaultAsync(w => w.WithdrawalRequestId == id);

            if (withdrawal == null)
            {
                return NotFound();
            }

            if (action == "approve")
            {
                if (withdrawal.Provider.Wallet.CurrentBalance < withdrawal.Amount)
                {
                    TempData["ErrorMessage"] = "رصيد مقدم الخدمة غير كافي للموافقة على طلب السحب";
                    return RedirectToAction(nameof(WithdrawalDetails), new { id });
                }

                withdrawal.Status = "Approved";
                withdrawal.ProcessedDate = DateTime.UtcNow;
                // تسجيل المعاملة
                var transaction = new WalletTransaction
                {
                    WalletId = withdrawal.Provider.Wallet.WalletId,
                    TransactionTypeId = 2, // WITHDRAWAL
                    Amount = withdrawal.Amount,
                    BalanceBefore = withdrawal.Provider.Wallet.CurrentBalance,
                    BalanceAfter = withdrawal.Provider.Wallet.CurrentBalance - withdrawal.Amount,
                    Description = $"سحب معتمد من الإدارة - رقم الطلب: {withdrawal.WithdrawalRequestId}",
                    TransactionDate = DateTime.UtcNow,
                    Status = "Pending",
                    WithdrawalRequestId = withdrawal.WithdrawalRequestId
                };
                _context.WalletTransactions.Add(transaction);

                TempData["SuccessMessage"] = "تمت الموافقة على طلب السحب بنجاح";
            }
            else if (action == "reject")
            {
                withdrawal.Status = "Rejected";
                withdrawal.ProcessedDate = DateTime.UtcNow;

                TempData["SuccessMessage"] = "تم رفض طلب السحب بنجاح";
            }
            else if (action == "complete")
            {
                if (withdrawal.Status != "Approved")
                {
                    TempData["ErrorMessage"] = "لا يمكن إكمال طلب سحب غير معتمد";
                    return RedirectToAction(nameof(WithdrawalDetails), new { id });
                }

                withdrawal.Status = "Completed";
                withdrawal.ProcessedDate = DateTime.UtcNow;
                // خصم المبلغ من محفظة مقدم الخدمة
                withdrawal.Provider.Wallet.CurrentBalance -= withdrawal.Amount;
                withdrawal.Provider.Wallet.LastUpdated = DateTime.UtcNow;

                // تحديث حالة المعاملة
                var transaction = await _context.WalletTransactions
                    .FirstOrDefaultAsync(t => t.WithdrawalRequestId == withdrawal.WithdrawalRequestId);

                if (transaction != null)
                {
                    transaction.Status = "Completed";
                    transaction.BalanceAfter = withdrawal.Provider.Wallet.CurrentBalance;
                }

                TempData["SuccessMessage"] = "تم إكمال عملية السحب بنجاح وتم خصم المبلغ من محفظة مقدم الخدمة";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ManageWithdrawals), new { status = "Pending" });
        }

        // سجل المعاملات
        [HttpGet("Transactions")]
        public async Task<IActionResult> ViewTransactions(int page = 1, string search = null)
        {
            var pageSize = 20;
            var query = _context.WalletTransactions
                .Include(t => t.Wallet)
                .ThenInclude(w => w.User)
                .Include(t => t.TransactionType)
                .OrderByDescending(t => t.TransactionDate);

            if (!string.IsNullOrEmpty(search))
            {
                query = (IOrderedQueryable<WalletTransaction>)query.Where(t =>
                    t.Wallet.User.FullName.Contains(search) ||
                    t.Description.Contains(search) ||
                    t.TransactionType.Name.Contains(search));
            }

            var totalItems = await query.CountAsync();
            var transactions = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var model = new VmAdminTransactions
            {
                Transactions = transactions,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                SearchTerm = search
            };

            return View(model);
        }

        // تفاصيل المعاملة
        [HttpGet("Transactions/Details/{id}")]
        public async Task<IActionResult> TransactionDetails(int id)
        {
            var transaction = await _context.WalletTransactions
                .Include(t => t.Wallet)
                .ThenInclude(w => w.User)
                .Include(t => t.TransactionType)
                .Include(t => t.Order)
                .Include(t => t.Service)
                .Include(t => t.DepositRequest)
                .Include(t => t.WithdrawalRequest)
                .FirstOrDefaultAsync(t => t.TransactionId == id);

            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }


    }
}
