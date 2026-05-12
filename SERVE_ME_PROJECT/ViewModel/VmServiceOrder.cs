using System.ComponentModel.DataAnnotations;
using SERVE_ME_PROJECT.Models;

namespace SERVE_ME_PROJECT.ViewModel
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class VmServiceOrder
    {
        public int ServiceId { get; set; }

        public string ServiceName { get; set; }

        public string ProviderName { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "السعر مطلوب")]
        [Range(0.01, 100000, ErrorMessage = "السعر غير صالح")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "الكمية مطلوبة")]
        [Range(1, 1000, ErrorMessage = "الكمية يجب أن تكون بين 1 و1000")]
        public int Quantity { get; set; }=1;

        // 🟡 فقط عند التأجير
        [Display(Name = "تاريخ بدء التأجير")]
        [DataType(DataType.Date)]
        public DateTime? RentalStartDate { get; set; }

        [Display(Name = "عدد أيام التأجير")]
        [Range(1, 365, ErrorMessage = "يجب أن تكون عدد الأيام بين 1 و365")]
        public int? RentalDays { get; set; }
    }

}
