using ims.Application.Interfaces;

namespace ims.Application.Services;

    public class EmailService : IEmailService
    {
        public Task SendAsync(string to, string subject, string body)
        {
            return Task.CompletedTask;
        }
    }

