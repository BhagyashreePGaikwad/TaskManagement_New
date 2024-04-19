using TaskManagement_April_.Model;

namespace TaskManagement_April_.Service
{
    public interface IEmailService
    {
        public Task SendEmailAsync(Email message);
    }
}
