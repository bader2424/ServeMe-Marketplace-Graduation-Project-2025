using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SERVE_ME_PROJECT.Models
{
    public class ServiceProviderModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string? NameShop { get; set; }

        [Required]
        [StringLength(2000, MinimumLength = 10)]
        public string? Description { get; set; }

        [StringLength(200)]
        public string? Logo { get; set; }

        [StringLength(200)]
        public string? PhotoRecord { get; set; }

        public DateTime JoinDate { get; set; } = DateTime.UtcNow;

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public string Status { get; set; } = "قيد المراجعة"; // أو "مقبول", "مرفوض"


        [NotMapped]
        [Required(ErrorMessage = "يرجى رفع صورة الشعار")]
        public IFormFile Imagf1 { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "يرجى رفع صورة السجل التجاري")]
        public IFormFile Imagf2 { get; set; }

        public ICollection<ServiceModel> Services { get; set; } = new List<ServiceModel>();

        public ICollection<OrderModel> Orders { get; set; } = new List<OrderModel>();

    }
}
