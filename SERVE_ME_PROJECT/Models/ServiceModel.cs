using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
namespace SERVE_ME_PROJECT.Models
{
    public class ServiceModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم الخدمة مطلوب")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "يجب أن يكون اسم الخدمة بين 3 و50 حرفاً")]
        [Display(Name = "اسم الخدمة")]
        public string Name { get; set; }

        [Required(ErrorMessage = "الوصف مطلوب")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "يجب أن يكون الوصف بين 10 و2000 حرف")]
        [Display(Name = "وصف الخدمة")]
        public string Description { get; set; }

        [Required(ErrorMessage = "تاريخ الإنشاء مطلوب")]
        [Display(Name = "تاريخ الإنشاء")]
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "السعر مطلوب")]
        [Range(100.00, 1000000, ErrorMessage = "يجب أن يكون السعر بين 100 و1,000,000")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "السعر")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "نوع الخدمة مطلوب:بيع/ تاجير")]
        [Display(Name = "نوع الخدمة")]
        public int TypeServiceId { get; set; }

        [Required(ErrorMessage = "التصنيف مطلوب")]
        [Display(Name = "التصنيف")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "المدينة مطلوب")]
        [Display(Name = "المدينة")]
        public int CityId { get; set; }

        public string? UserId { get; set; }

        // علاقات التنقل
        [ForeignKey("TypeServiceId")]
        public virtual TypeserviceModel? TypeService { get; set; }

        [ForeignKey("CategoryId")]
        public virtual CategoryModel? Category { get; set; }

        [ForeignKey("CityId")]
        public virtual CityModel? City { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }

        public bool IsApproved { get; set; } = false;
        public virtual ICollection<WalletTransaction> WalletTransactions { get; set; } = new List<WalletTransaction>();
        public virtual ICollection<ServiceImgeModel> Images { get; set; } = new List<ServiceImgeModel>();
        public virtual ICollection<CommentModel> Comments { get; set; } = new List<CommentModel>();
        public virtual ICollection<OrderModel> Orders { get; set; } = new List<OrderModel>();

    }
}
