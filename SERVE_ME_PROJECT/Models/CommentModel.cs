using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SERVE_ME_PROJECT.Models
{
    public class CommentModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(2000, MinimumLength = 5)]
        public string Content { get; set; }

        [Required]
        [Range(1, 5)]
        public int RatingValue { get; set; }// 1-5 

        public DateTime CommentDate { get; set; } = DateTime.UtcNow;

        public int ServiceId { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [ForeignKey("ServiceId")]
        public ServiceModel Service { get; set; }


    }
}
