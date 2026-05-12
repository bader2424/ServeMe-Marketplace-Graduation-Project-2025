using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SERVE_ME_PROJECT.Models
{
    public class Wallet
    {
        [Key]
        public int WalletId { get; set; }

        [Required(ErrorMessage = "يجب ربط المحفظة بمستخدم")]
        [StringLength(450, ErrorMessage = "معرف المستخدم غير صالح")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "الرصيد الحالي مطلوب")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 10000000, ErrorMessage = "يجب أن يكون الرصيد بين 0 و10,000,000")]
        public decimal CurrentBalance { get; set; } = 0;

        [Required(ErrorMessage = "تاريخ الإنشاء مطلوب")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "تاريخ آخر تحديث مطلوب")]
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "حالة المحفظة مطلوبة")]
        [StringLength(20)]
        public string Status { get; set; } = "Active"; // Active, Suspended, Closed

    [StringLength(255, ErrorMessage = "يجب ألا تتجاوز الملاحظات 255 حرفًا")]
    public string? Notes { get; set; }

        // علاقات التنقل (Navigation Properties)
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<WalletTransaction> Transactions { get; set; } = new List<WalletTransaction>();
        public virtual ICollection<DepositRequest> DepositRequests { get; set; } = new List<DepositRequest>();
        public virtual ICollection<WithdrawalRequest> WithdrawalRequests { get; set; } = new List<WithdrawalRequest>();
    }
}
