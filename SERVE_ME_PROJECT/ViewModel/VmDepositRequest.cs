using System.ComponentModel.DataAnnotations;

namespace SERVE_ME_PROJECT.ViewModel
{
    public class VmDepositRequest
    {
        [Required(ErrorMessage = "المبلغ مطلوب")]
        [Range(50, 50000, ErrorMessage = "يجب أن يكون المبلغ بين 50 و50,000 ريال")]
        public decimal Amount { get; set; }

     //   [Required(ErrorMessage = "البنك المحول منه مطلوب")]
       // public string SelectedBank { get; set; }

     //   public List<BankInfo> AvailableBanks { get; set; }

        [Required(ErrorMessage = "رقم المرجع البنكي مطلوب")]
        [StringLength(50, ErrorMessage = "يجب ألا يتجاوز رقم المرجع 50 حرفًا")]
        public string TransactionReference { get; set; }

        [Required(ErrorMessage = "صورة الإيداع مطلوبة")]
        public IFormFile DepositProofImage { get; set; }
    }
}
