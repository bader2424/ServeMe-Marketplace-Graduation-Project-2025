using SERVE_ME_PROJECT.Models;

namespace SERVE_ME_PROJECT.ViewModel
{
    public class VmProviderEarningsHistory
    {
        public List<WalletTransaction> Earnings { get; set; }
        public decimal TotalEarnings { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
    }
}
