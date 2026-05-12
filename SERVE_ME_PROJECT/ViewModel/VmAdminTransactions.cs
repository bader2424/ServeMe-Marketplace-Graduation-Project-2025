using SERVE_ME_PROJECT.Models;

namespace SERVE_ME_PROJECT.ViewModel
{
    public class VmAdminTransactions
    {
        public List<WalletTransaction> Transactions { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string SearchTerm { get; set; }
    }
}
