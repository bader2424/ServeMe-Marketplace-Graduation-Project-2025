using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SERVE_ME_PROJECT.Models
{
    public class CityModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم المدينة مطلوب")]
        [StringLength(50, ErrorMessage = "يجب ألا يتجاوز الاسم 50 حرفاً")]
        [Display(Name = "المدينة")]
        public string? Name { get; set; }
        public virtual ICollection<ServiceModel>? Services { get; set; }

    }
}
