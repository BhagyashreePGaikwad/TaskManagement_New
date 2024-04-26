using System.Net.Mail;
using System.Net;
using TaskManagement_April_.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TaskManagement_April_.Context;
using System.Reflection;

namespace TaskManagement_April_.Service.Implementation
{
    public class EmailService:IEmailService
    {
        #region Variable
        private readonly IConfiguration _configuration;
        private TaskManagementContext _dbcontext;
        #endregion
        #region Constructor
        public EmailService(IConfiguration configuration, TaskManagementContext dbcontext)
        {
            _configuration = configuration;
            _dbcontext = dbcontext;
        }
        #endregion
        public async Task<bool> SendEmailAsync(Email message)
        {
            try
            {
                var server=_configuration["EmailCOnfiguration:smtpServer"];
                var port = Convert.ToInt32(_configuration["EmailCOnfiguration:port"]);
                var userEmail = _configuration["EmailCOnfiguration:username"];
                var password = _configuration["EmailCOnfiguration:password"];
                using (SmtpClient smtpClient = new SmtpClient(server,port))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(userEmail,password);
                    smtpClient.EnableSsl = true;

                    MailMessage mailMessage = new MailMessage(message.From, message.To, message.Subject, message.Body)
                    {
                        IsBodyHtml = true
                    };
                    if (!string.IsNullOrEmpty(message.CC))
                    {
                        mailMessage.CC.Add(message.CC);
                    }

                    await smtpClient.SendMailAsync(mailMessage);
                    return true;
                 
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
                // Handle exception (log or rethrow)
                throw;
            }
        }

        public async Task<bool> SendEmailTemplate(TaskL model,string templateName,int type)
        {
            try
            {
                var sendEmail = false;
                var parameters = new[]
                       {
                        new SqlParameter("@AssignTo", model.AssignTo),
                        new SqlParameter("@AssignBy", model.AssignBy),
                        new SqlParameter("@PriorityId", model.priority),
                        new SqlParameter("@ProjectId", model.ProjectId),
                        new SqlParameter("@SubTaskId", model.subTaskId)
                        };

                var data = await _dbcontext.TaskView.FromSqlRaw("EXECUTE GetTaskData @AssignTo, @AssignBy, @PriorityId, @ProjectId, @SubTaskId", parameters).ToListAsync();
                var result = data.FirstOrDefault();
                if (result != null)
                {
                    //string htmlTemplatePath = System.IO.File.ReadAllText(@"./Template/AssignTaskTemplate.html"); 
                    //string htmlTemplatePath= @"C:\Bhagyashree Work\Onboarding\C#\TaskManagement(April)\Template\AssignTaskTemplate.html";

                    string htmlTemplatePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Template",templateName);

                    string htmlTemplate = File.ReadAllText(htmlTemplatePath);


                    htmlTemplate = htmlTemplate.Replace("{{AssignTo}}", result.AssignToName);
                    htmlTemplate = htmlTemplate.Replace("{{AssignBy}}", result.AssignByName);
                    htmlTemplate = htmlTemplate.Replace("{{Task}}", model.Name);
                    htmlTemplate = htmlTemplate.Replace("{{startDate}}", model.StartDate.ToString());
                    htmlTemplate = htmlTemplate.Replace("{{endDate}}", model.EndDate.ToString());
                    htmlTemplate = htmlTemplate.Replace("{{ProjectName}}", result.ProjectName);
                    htmlTemplate = htmlTemplate.Replace("{{SubTaskName}}", result.SubTaskName);
                    htmlTemplate = htmlTemplate.Replace("{{priority}}", result.PriorityName);
                    string subject = "";
                    if (type == 1)
                    {
                        subject = "New Task Assigned";
                    }
                    else if (type == 2)
                    {
                        subject = "Task being Updated";
                    }
                    else
                    {
                        subject = "Task Not Completed";
                    }

                    Email emailMessage = new Email
                    {
                        From = result.AssignByEmail,
                        To = result.AssignToEmail,
                        CC = "bhayashree.gaikwad@wonderbiz.in",
                        Subject = subject,
                        Body = htmlTemplate
                    };
                
                    sendEmail = await SendEmailAsync(emailMessage);
                }
                else
                {
                    sendEmail = false;

                }
                return sendEmail;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

    }
}
