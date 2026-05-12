using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SERVE_ME_PROJECT.Models
{
    public class WalletTransfer
    {
        [Key]
        public int TransferId { get; set; }

        [Required(ErrorMessage = "المستخدم المرسل مطلوب")]
        public string SenderUserId { get; set; }

        [Required(ErrorMessage = "المستخدم المستقبل مطلوب")]
        public string ReceiverUserId { get; set; }

        [Required(ErrorMessage = "المبلغ مطلوب")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, 100000, ErrorMessage = "يجب أن يكون المبلغ بين 0.01 و100,000")]
        public decimal Amount { get; set; }
        [Required]
        public DateTime TransferDate { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Completed"; // Completed, Failed, Pending

        // علاقات التنقل
        [ForeignKey("SenderUserId")]
        public virtual ApplicationUser SenderUser { get; set; }

        [ForeignKey("ReceiverUserId")]
        public virtual ApplicationUser ReceiverUser { get; set; }

        public virtual ICollection<WalletTransaction> WalletTransactions { get; set; } = new List<WalletTransaction>();
    }
}
