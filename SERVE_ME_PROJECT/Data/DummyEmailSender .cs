using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace SERVE_ME_PROJECT.Data
{
    public class DummyEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // يمكنك هنا طباعة المعلومات للعرض فقط أو تجاهلها
            Console.WriteLine($"Email to: {email}, Subject: {subject}");
            return Task.CompletedTask;
        }
    }
}
