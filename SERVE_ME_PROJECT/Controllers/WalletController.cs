using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SERVE_ME_PROJECT.Data;
using SERVE_ME_PROJECT.Models;
using SERVE_ME_PROJECT.ViewModel;

namespace SERVE_ME_PROJECT.Controllers
{
   // [Authorize]
    public class WalletController : Controller
    {
        private readonly ServeMeContext _context;
        private readonly ILogger<BlogController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public WalletController(
            ServeMeContext context,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment hostingEnvironment,
            ILogger<BlogController> logger)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        // عرض لوحة التحكم الرئيسية للمحفظة
        public async Task<IActionResult> Dashboard()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            var wallet = await GetOrCreateWalletAsync(userId);

            var model = new VmWalletDashboard
            {
                Balance = wallet.CurrentBalance,
                RecentTransactions = await _context.WalletTransactions
                    .Where(t => t.WalletId == wallet.WalletId)
                    .OrderByDescending(t => t.TransactionDate)
                    .Take(5)
                    .Include(t => t.TransactionType)
                    .ToListAsync(),
                PendingDeposits = await _context.DepositRequests
                    .Where(d => d.UserId == userId && d.Status == "Pending")
                    .CountAsync(),
                PendingWithdrawals = await _context.WithdrawalRequests
                    .Where(w => w.ProviderId == userId && w.Status == "Pending")
                    .CountAsync()
            };

            return View(model);
        }

        // عرض نموذج إيداع الأموال
        [HttpGet]
        public IActionResult RequestDeposit()
        {
            var model = new VmDepositRequest
            {
        //        AvailableBanks = GetBankList()
            };
            return View(model);
        }

        // معالجة طلب الإيداع
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestDeposit(VmDepositRequest model)
        {
            if (!ModelState.IsValid)
            {
            //    model.AvailableBanks = GetBankList();
                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var wallet = await GetOrCreateWalletAsync(userId);

            try
            {
                // حفظ صورة الإيداع
                string imagePath = await SaveDepositProofImage(model.DepositProofImage);

                var depositRequest = new DepositRequest
                {
                    UserId = userId,
                    Amount = model.Amount,
                    DepositProofImage = imagePath,
                    Status = "Pending",
                    RequestDate = DateTime.UtcNow,
                   // BankName = model.SelectedBank,
                   AdminNotes="اب",
                    TransactionReference = model.TransactionReference
                };

                _context.DepositRequests.Add(depositRequest);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "تم تقديم طلب الإيداع بنجاح وسيتم مراجعته خلال 24 ساعة";
                return RedirectToAction(nameof(DepositHistory));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ أثناء طلب الإيداع"); // لو لديك Logger
                ModelState.AddModelError("", "حدث خطأ أثناء معالجة طلبك. يرجى المحاولة لاحقاً.");
           //     model.AvailableBanks = GetBankList();
                return View(model);
            }
        }

        // عرض سجل الإيداعات
        public async Task<IActionResult> DepositHistory(int page = 1, int pageSize = 10)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = _context.DepositRequests
                .Where(d => d.UserId == userId)
                .OrderByDescending(d => d.RequestDate);

            var totalItems = await query.CountAsync();
            var deposits = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var model = new VmDepositHistory
            {
                Deposits = deposits,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                PageSize = pageSize
            };

            return View(model);
        }

        // عرض تفاصيل معاملة محددة
        public async Task<IActionResult> TransactionDetails(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var wallet = await GetOrCreateWalletAsync(userId);

            var transaction = await _context.WalletTransactions
                .Include(t => t.TransactionType)
                .Include(t => t.Order)
                .Include(t => t.Service)
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
                    Status = "Active",
                    Notes = "رصيد مبدئي" // أو مثلاً string.Empty

                };
                _context.Wallets.Add(wallet);
                await _context.SaveChangesAsync();
            }

            return wallet;
        }

        private async Task<string> SaveDepositProofImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                throw new ArgumentException("صورة الإيداع مطلوبة");
            }

            var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads/deposits");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = $"{Guid.NewGuid()}_{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(imageFile.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return $"/uploads/deposits/{uniqueFileName}";
        }

        #endregion
    }
}
