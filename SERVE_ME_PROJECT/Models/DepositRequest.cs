using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SERVE_ME_PROJECT.Models
{
    public class DepositRequest
    {
        [Key]
        public int DepositRequestId { get; set; }

        [Required(ErrorMessage = "يجب تحديد المستخدم")]
        [StringLength(450, ErrorMessage = "معرف المستخدم غير صالح")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "المبلغ المطلوب إيداعه مطلوب")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(10, 100000, ErrorMessage = "يجب أن يكون المبلغ بين 10 و100,000")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "صورة إثبات الإيداع مطلوبة")]
        [StringLength(255, ErrorMessage = "يجب ألا يتجاوز مسار الصورة 255 حرفًا")]
        public string DepositProofImage { get; set; }

        [Required(ErrorMessage = "حالة الطلب مطلوبة")]
        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected

        [StringLength(500, ErrorMessage = "يجب ألا تتجاوز ملاحظات المشرف 500 حرف")]
        public string? AdminNotes { get; set; }

        [Required(ErrorMessage = "تاريخ الطلب مطلوب")]
        public DateTime RequestDate { get; set; } = DateTime.UtcNow;

        public DateTime? ProcessedDate { get; set; }

        [StringLength(50, ErrorMessage = "يجب ألا يتجاوز رقم المرجع 50 حرفًا")]
        public string TransactionReference { get; set; }

        // علاقات التنقل
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        public virtual WalletTransaction WalletTransaction { get; set; }

    }
}
