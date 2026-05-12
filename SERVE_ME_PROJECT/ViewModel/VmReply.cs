using System.ComponentModel.DataAnnotations;

namespace SERVE_ME_PROJECT.ViewModel
{
    public class VmReply
    {
        public int MessageId { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }
    }
}
