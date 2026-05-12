using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SERVE_ME_PROJECT.Models
{
    public class BlogModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string? Title { get; set; }

        [Required]
        [StringLength(maximumLength:3000)]
        public string? Content { get; set; }
        public string? ImgUrl { get; set; }
        //public string? CityName { get; set; } = "الشحر"; // إضافة خاصية اسم المدينة

        public DateTime SentDate { get; set; } = DateTime.UtcNow;


    }
}
