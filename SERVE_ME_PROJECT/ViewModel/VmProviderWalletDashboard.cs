using SERVE_ME_PROJECT.Models;

namespace SERVE_ME_PROJECT.ViewModel
{
    public class VmProviderWalletDashboard
    {
        public decimal Balance { get; set; }
        public decimal TotalEarnings { get; set; }
        public decimal MonthlyEarnings { get; set; }
        public int PendingWithdrawals { get; set; }
        public List<WalletTransaction> RecentTransactions { get; set; }

    }
}
