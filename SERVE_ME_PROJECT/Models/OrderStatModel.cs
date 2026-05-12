using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SERVE_ME_PROJECT.Models
{
    public class OrderStatModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public ICollection<OrderModel> Orders { get; set; } = new List<OrderModel>();
    }
}
