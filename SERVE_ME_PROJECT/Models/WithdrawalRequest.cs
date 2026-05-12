using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SERVE_ME_PROJECT.Models
{
    public class WithdrawalRequest
    {
        [Key]
        public int WithdrawalRequestId { get; set; }

        [Required(ErrorMessage = "يجب تحديد مقدم الخدمة")]
        [StringLength(450, ErrorMessage = "معرف المستخدم غير صالح")]
        public string ProviderId { get; set; }

        [Required(ErrorMessage = "المبلغ المطلوب سحبه مطلوب")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(100, 50000, ErrorMessage = "يجب أن يكون المبلغ بين 100 و50,000")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "تفاصيل الحساب البنكي مطلوبة")]
        [StringLength(200, ErrorMessage = "يجب ألا تتجاوز تفاصيل الحساب 200 حرف")]
        public string BankAccountDetails { get; set; }

        [Required(ErrorMessage = "حالة الطلب مطلوبة")]
        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected

        [Required(ErrorMessage = "تاريخ الطلب مطلوب")]
        public DateTime RequestDate { get; set; } = DateTime.UtcNow;

        public DateTime? ProcessedDate { get; set; }

        [Required(ErrorMessage = "اسم صاحب الحساب مطلوب")]
        [StringLength(100, ErrorMessage = "يجب ألا يتجاوز اسم صاحب الحساب 100 حرف")]
        public string AccountHolderName { get; set; }

        [Required(ErrorMessage = "رقم الحساب البنكي مطلوب")]
        [StringLength(50, ErrorMessage = "يجب ألا يتجاوز رقم الحساب 50 حرفًا")]
        public string AccountNumber { get; set; }

        [Required(ErrorMessage = "اسم البنك مطلوب")]
        [StringLength(50, ErrorMessage = "يجب ألا يتجاوز اسم البنك 50 حرفًا")]
        public string BankName { get; set; }

        [StringLength(50, ErrorMessage = "يجب ألا يتجاوز رمز الفرع 50 حرفًا")]
        public string BranchCode { get; set; }

        // علاقات التنقل
        [ForeignKey("ProviderId")]
        public virtual ApplicationUser Provider { get; set; }

        public virtual WalletTransaction WalletTransaction { get; set; }

    }
}
