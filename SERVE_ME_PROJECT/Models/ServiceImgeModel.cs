using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SERVE_ME_PROJECT.Models
{
    public class ServiceImgeModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "مسار الصورة مطلوب")]
        [Display(Name = "مسار الصورة")]
        public string ImagePath { get; set; }

        //[Required(ErrorMessage = "تاريخ الرفع مطلوب")]
        //[Display(Name = "تاريخ الرفع")]
        //public DateTime UploadDate { get; set; } = DateTime.UtcNow;

        //[Required(ErrorMessage = "حالة الصورة مطلوبة")]
        //[Display(Name = "صورة رئيسية")]
        //public bool IsMainImage { get; set; }

        //[Display(Name = "ترتيب العرض")]
        //public int DisplayOrder { get; set; }

        //[Required(ErrorMessage = "الخدمة مطلوبة")]
        //[Display(Name = "الخدمة")]
        public int ServiceId { get; set; }

        [ForeignKey("ServiceId")]
        public virtual ServiceModel Service { get; set; }

    }
}
