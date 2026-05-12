using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SERVE_ME_PROJECT.Models
{
    public class WalletTransaction
    {
        [Key]
        public int TransactionId { get; set; }

        [Required(ErrorMessage = "يجب تحديد المحفظة")]
        public int WalletId { get; set; }

        [Required(ErrorMessage = "نوع المعاملة مطلوب")]
        public int TransactionTypeId { get; set; }

        [Required(ErrorMessage = "المبلغ مطلوب")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, 1000000, ErrorMessage = "يجب أن يكون المبلغ بين 0.01 و1,000,000")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "الرصيد قبل العملية مطلوب")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal BalanceBefore { get; set; }

        [Required(ErrorMessage = "الرصيد بعد العملية مطلوب")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal BalanceAfter { get; set; }

        [StringLength(255, ErrorMessage = "يجب ألا يتجاوز الوصف 255 حرفًا")]
        public string Description { get; set; }

        [Required(ErrorMessage = "تاريخ المعاملة مطلوب")]
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "حالة المعاملة مطلوبة")]
        [StringLength(20)]
        public string Status { get; set; } // Completed, Pending, Failed, Refunded

        // مفاتيح أجنبية
        public int? OrderId { get; set; }
        public int? ServiceId { get; set; }
        public int? DepositRequestId { get; set; }
        public int? WithdrawalRequestId { get; set; }
        public int? TransferId { get; set; }
        public int? RefundRequestId { get; set; }

        // علاقات التنقل
        [ForeignKey("WalletId")]
        public virtual Wallet Wallet { get; set; }

        [ForeignKey("TransactionTypeId")]
        public virtual TransactionType TransactionType { get; set; }

        [ForeignKey("OrderId")]
        public virtual OrderModel Order { get; set; }

        [ForeignKey("ServiceId")]
        public virtual ServiceModel Service { get; set; }

        [ForeignKey("DepositRequestId")]
        public virtual DepositRequest DepositRequest { get; set; }

        [ForeignKey("WithdrawalRequestId")]
        public virtual WithdrawalRequest WithdrawalRequest { get; set; }

        [ForeignKey("TransferId")]
        public virtual WalletTransfer Transfer { get; set; }

        [ForeignKey("RefundRequestId")]
        public virtual RefundRequest RefundRequest { get; set; }
    }

}
