using System.ComponentModel.DataAnnotations;

namespace SERVE_ME_PROJECT.Models
{
    public class TransactionType
    {
        [Key]
        public int TransactionTypeId { get; set; }

        [Required(ErrorMessage = "اسم نوع المعاملة مطلوب")]
        [StringLength(50, ErrorMessage = "يجب ألا يتجاوز الاسم 50 حرفًا")]
        public string Name { get; set; }
        [Required(ErrorMessage = "رمز النوع مطلوب")]
        [StringLength(20, ErrorMessage = "يجب ألا يتجاوز الرمز 20 حرفًا")]
        public string Code { get; set; } // مثل: DEPOSIT, WITHDRAWAL, PAYMENT, etc.
        public bool IsCredit { get; set; }  // ← هل هي عملية إضافة (true) أم خصم (false)
        // علاقات التنقل
        public virtual ICollection<WalletTransaction> WalletTransactions { get; set; } = new List<WalletTransaction>();
    }
}
