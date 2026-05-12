using System.ComponentModel.DataAnnotations;

namespace SERVE_ME_PROJECT.Models
{
    public class ContactUsModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime SentDate { get; set; } = DateTime.UtcNow;
        public bool IsReplied { get; set; }

    }
}
