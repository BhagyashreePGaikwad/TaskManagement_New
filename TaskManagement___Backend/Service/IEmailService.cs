using TaskManagement_April_.Model;

namespace TaskManagement_April_.Service
{
    public interface IEmailService
    {
        public Task<bool> SendEmailAsync(Email message);

        public Task<bool> SendEmailTemplate(TaskL model, string templateName,int type);
    }
}
