using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SERVE_ME_PROJECT.Models
{
    public class OrderModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "الكمية مطلوبة")]
        [Range(1, 1000, ErrorMessage = "يجب أن تكون الكمية بين 1 و1000")]
        public decimal Quantity { get; set; }

        [Required(ErrorMessage = "السعر مطلوب")]
        [Range(0.01, 100000, ErrorMessage = "يجب أن يكون السعر بين 0.01 و100000")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "حالة الطلب مطلوبة")]
        public int StatOrderId { get; set; }

        [Required(ErrorMessage = "الخدمة مطلوبة")]
        public int ServiceId { get; set; }

        [Required(ErrorMessage = "المستخدم مطلوب")]
        [StringLength(450)]
        public string UserId { get; set; }

        // 🔹 هذه الحقول جديدة لدعم التأجير
        public DateTime? RentalStartDate { get; set; } // تاريخ بدء التأجير

        public int? RentalDays { get; set; } // عدد أيام التأجير

        [NotMapped] // لا يتم تخزينه في قاعدة البيانات لأنه مشتق
        public DateTime? RentalEndDate
        {
            get
            {
                if (RentalStartDate.HasValue && RentalDays.HasValue)
                    return RentalStartDate.Value.AddDays(RentalDays.Value);
                return null;
            }
        }

        // علاقات التنقل
        [ForeignKey("StatOrderId")]
        public virtual OrderStatModel OrderStat { get; set; }

        [ForeignKey("ServiceId")]
        public virtual ServiceModel Service { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        public virtual WalletTransaction WalletTransaction { get; set; }
    }

}

