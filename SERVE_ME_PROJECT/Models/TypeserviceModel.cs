using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SERVE_ME_PROJECT.Models
{
    public class TypeserviceModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم النوع مطلوب")]
        [StringLength(50, ErrorMessage = "يجب ألا يتجاوز الاسم 50 حرفاً")]
        [Display(Name = "نوع الخدمة")]
        public string? Name { get; set; }

        public virtual ICollection<ServiceModel>? Services { get; set; }
    }
}
