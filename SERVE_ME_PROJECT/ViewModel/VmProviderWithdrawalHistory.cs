using SERVE_ME_PROJECT.Models;

namespace SERVE_ME_PROJECT.ViewModel
{
    public class VmProviderWithdrawalHistory
    {
        public List<WithdrawalRequest> Withdrawals { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
    }
}
