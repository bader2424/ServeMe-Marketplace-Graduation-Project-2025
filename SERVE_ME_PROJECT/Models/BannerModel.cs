using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SERVE_ME_PROJECT.Models
{
    public class BannerModel
    {
        [Key]
        public int Id { get; set; }

        [StringLength(500)]
        public string? Title { get; set; }

        public string? UrlImage { get; set; }
    }
}
