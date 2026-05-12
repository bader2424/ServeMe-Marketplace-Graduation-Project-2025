using SERVE_ME_PROJECT.Models;

namespace SERVE_ME_PROJECT.ViewModel
{
    public class VmAdminDeposits
    {
        public List<DepositRequest> Deposits { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string CurrentStatus { get; set; }
        public List<string> StatusList { get; set; }
    }
}
