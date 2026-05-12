using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SERVE_ME_PROJECT.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "الاسم الكامل مطلوب")]
        [StringLength(100, ErrorMessage = "الاسم الكامل يجب ألا يتجاوز 100 حرف")]
        public string FullName { get; set; }

        [StringLength(50, ErrorMessage = "المدينة يجب ألا تتجاوز 50 حرف")]
        public string City { get; set; }

        public string ProfileImagePath { get; set; }
        public bool IsActive { get; set; } = true;

        // العلاقات
        public virtual ICollection<OrderModel> Orders { get; set; } = new List<OrderModel>();
        public virtual ICollection<CommentModel> Comments { get; set; } = new List<CommentModel>();
        public virtual ICollection<ServiceModel> Services { get; set; } = new List<ServiceModel>();
        public virtual Wallet Wallet { get; set; }
        public virtual ICollection<DepositRequest> DepositRequests { get; set; } = new List<DepositRequest>();
        public virtual ICollection<WithdrawalRequest> WithdrawalRequests { get; set; } = new List<WithdrawalRequest>();

    }

}
