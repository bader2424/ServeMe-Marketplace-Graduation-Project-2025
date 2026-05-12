using System.ComponentModel.DataAnnotations;

namespace SERVE_ME_PROJECT.Models
{
    public class GeneralSettingsModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string SiteName { get; set; }

   //     [Required]
       /// [StringLength(255)]
        public string? SiteLogo { get; set; }

        [Required]
        [StringLength(500)]
        public string SiteDescription { get; set; }

        [Required]
        [StringLength(100)]
        public string ContactEmail { get; set; }

        [Required]
        [StringLength(20)]
        public string ContactPhone { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        [StringLength(255)]
        public string FacebookLink { get; set; }

        [StringLength(255)]
        public string TwitterLink { get; set; }

        [StringLength(255)]
        public string InstagramLink { get; set; }

        [StringLength(255)]
        public string LinkedInLink { get; set; }
    }
}
