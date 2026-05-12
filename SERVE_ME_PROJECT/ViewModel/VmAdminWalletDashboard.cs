using SERVE_ME_PROJECT.Models;

namespace SERVE_ME_PROJECT.ViewModel
{
    public class VmAdminWalletDashboard
    {
        public decimal TotalBalance { get; set; }
        public int PendingDepositsCount { get; set; }
        public int PendingWithdrawalsCount { get; set; }
        public decimal SystemEarnings { get; set; }
        public List<WalletTransaction> RecentTransactions { get; set; }
    }
}
