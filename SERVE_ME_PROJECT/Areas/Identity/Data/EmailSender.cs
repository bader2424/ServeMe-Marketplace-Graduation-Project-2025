using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace SERVE_ME_PROJECT.Areas.Identity.Data
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // هذا مثال فقط، ستحتاج لاحقاً إلى استخدام SMTP أو خدمة خارجية لإرسال الإيميل
            Console.WriteLine($"To: {email}, Subject: {subject}, Message: {htmlMessage}");
            return Task.CompletedTask;
        }
    }
}
