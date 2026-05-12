using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SERVE_ME_PROJECT.Models
{
    public class RefundRequest
    {
        [Key]
        public int RefundRequestId { get; set; }

        [Required(ErrorMessage = "الطلب المرتبط مطلوب")]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "المستخدم مطلوب")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "المبلغ مطلوب")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, 100000, ErrorMessage = "يجب أن يكون المبلغ بين 0.01 و100,000")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "السبب مطلوب")]
        [StringLength(500, ErrorMessage = "يجب ألا يتجاوز السبب 500 حرف")]
        public string Reason { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected

        [Required]
        public DateTime RequestDate { get; set; } = DateTime.UtcNow;

        public DateTime? ProcessedDate { get; set; }

        // علاقات التنقل
        [ForeignKey("OrderId")]
        public virtual OrderModel Order { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        public virtual WalletTransaction WalletTransaction { get; set; }

    }
}
