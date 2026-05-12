using SERVE_ME_PROJECT.Models;

namespace SERVE_ME_PROJECT.ViewModel
{
    public class VmWalletDashboard
    {
        public decimal Balance { get; set; }
        public List<WalletTransaction> RecentTransactions { get; set; }
        public int PendingDeposits { get; set; }
        public int PendingWithdrawals { get; set; }
    }
}
