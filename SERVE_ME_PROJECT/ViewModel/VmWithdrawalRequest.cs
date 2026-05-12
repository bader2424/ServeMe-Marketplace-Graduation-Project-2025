using System.ComponentModel.DataAnnotations;

namespace SERVE_ME_PROJECT.ViewModel
{
    public class VmWithdrawalRequest
    {
        [Required(ErrorMessage = "المبلغ مطلوب")]
        [Range(100, 50000, ErrorMessage = "يجب أن يكون المبلغ بين 100 و50,000 ريال")]
        public decimal Amount { get; set; }

        public decimal CurrentBalance { get; set; }

        [Required(ErrorMessage = "اسم صاحب الحساب مطلوب")]
        [StringLength(100, ErrorMessage = "يجب ألا يتجاوز الاسم 100 حرف")]
        public string AccountHolderName { get; set; }

        [Required(ErrorMessage = "البنك المحول إليه مطلوب")]
        public string SelectedBank { get; set; }

        public List<BankInfo> AvailableBanks { get; set; }

        [Required(ErrorMessage = "رقم الحساب مطلوب")]
        [StringLength(24, MinimumLength = 10, ErrorMessage = "يجب أن يكون رقم الحساب بين 10 و24 رقمًا")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "يجب أن يحتوي رقم الحساب على أرقام فقط")]
        public string AccountNumber { get; set; }

        [StringLength(10, ErrorMessage = "يجب ألا يتجاوز رمز الفرع 10 أحرف")]
        public string BranchCode { get; set; }

    }
}
