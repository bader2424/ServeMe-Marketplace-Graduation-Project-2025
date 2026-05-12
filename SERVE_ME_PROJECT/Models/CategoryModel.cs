using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SERVE_ME_PROJECT.Models
{
    public class CategoryModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم التصنيف مطلوب")]
        [StringLength(50, ErrorMessage = "يجب ألا يتجاوز الاسم 50 حرفاً")]
        [Display(Name = "التصنيف")]
        public string? Name { get; set; }

        public virtual ICollection<ServiceModel>? Services { get; set; }

    }
}
