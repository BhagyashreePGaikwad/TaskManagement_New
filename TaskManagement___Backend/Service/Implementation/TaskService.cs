using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Net.Mail;
using System.Threading.Tasks;
using TaskManagement_April_.Context;
using TaskManagement_April_.Model;

namespace TaskManagement_April_.Service.Implementation
{
    public class TaskService : ITaskService
    {
        #region Variable
        private TaskManagementContext _dbcontext;
        private IEmailService _emailService;
        #endregion

        #region Constructor
        public TaskService(TaskManagementContext dbcontext, IEmailService emailService)
        {
            _dbcontext = dbcontext;
            _emailService = emailService;
        }
        #endregion
        public Task<bool> DelTask(int id)
        {
            try
            {
                var task=_dbcontext.Task.FirstOrDefault(x => x.Id == id);
                if (task != null)
                {
                    _dbcontext.Task.Remove(task);
                    _dbcontext.SaveChanges();
                    return Task.FromResult(true);
                }
                return Task.FromResult(false);
                
            }catch (Exception ex)
            {
                return Task.FromResult(false);
            }
        }

        public async Task<IQueryable> GetAllTaskofProject(int ProjectId)
        {
            try
            {
                var task = await (from i in _dbcontext.Task
                                 where i.ProjectId == ProjectId
                                 select new
                                 {
                                     id=i.Id,
                                     Name = i.Name,
                                     Description=i.Description,
                                     AssignTo=i.AssignTo,
                                     AssignBy=i.AssignBy,
                                     Status=i.Status,
                                     Priority=i.priority,
                                     ProjectId = i.ProjectId,
                                     SubTask=i.subTaskId
                                 }).ToListAsync();

                return task.AsQueryable();
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<IQueryable> GetAllTaskofSubTask(int subTaskId)
        {
            try
            {
                var task = await (from i in _dbcontext.Task
                                  where i.subTaskId == subTaskId
                                  select new
                                  {
                                      id = i.Id,
                                      Name = i.Name,
                                      Description = i.Description,
                                      AssignTo = i.AssignTo,
                                      AssignBy = i.AssignBy,
                                      Status = i.Status,
                                      Priority = i.priority,
                                      ProjectId = i.ProjectId,
                                      SubTask = i.subTaskId
                                  }).ToListAsync();

                return task.AsQueryable();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IQueryable> GetTaskByAssignTo(int assignTo)
        {
            try
            {
                var task = await (from i in _dbcontext.Task
                                  where i.AssignTo == assignTo
                                  select new
                                  {
                                      id = i.Id,
                                      Name = i.Name,
                                      Description = i.Description,
                                      AssignTo = i.AssignTo,
                                      AssignBy = i.AssignBy,
                                      Status = i.Status,
                                      Priority = i.priority,
                                      ProjectId = i.ProjectId,
                                      SubTask = i.subTaskId
                                  }).ToListAsync();

                return task.AsQueryable();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IQueryable> GetYourTaskSortByDueDateorPriority(int assignTo,string? filter)
        {
            try
            {

                
                var task = await (from i in _dbcontext.Task
                                  where i.AssignTo == assignTo
                                  select new
                                  {
                                      id = i.Id,
                                      Name = i.Name,
                                      Description = i.Description,
                                      AssignTo = i.AssignTo,
                                      AssignBy = i.AssignBy,
                                      Status = i.Status,
                                      Priority = i.priority,
                                      ProjectId = i.ProjectId,
                                      Duedate=i.EndDate,
                                      SubTask = i.subTaskId
                                  }).ToListAsync();

                if (filter == "priority")
                {
                    task = task.OrderBy(t => t.Priority).ToList();
                }
                else if (filter == "endDate")
                {
                    task = task.OrderBy(t => t.Duedate).ToList();
                }

                return task.AsQueryable();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<IQueryable> GetTaskById(int id)
        {
            try
            {
                var result = await (from x in _dbcontext.Task
                                    where x.Id== id
                                    select new {
                                        x.Id,
                                        x.Name,
                                        x.Description,
                                        x.AssignTo,
                                        x.AssignBy,
                                        x.Status,
                                        x.priority
                                    }).ToListAsync();
                return result.AsQueryable();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> SaveTask(Tasks model)
        {
            try
            {
                _dbcontext.Task.Add(model);
                _dbcontext.SaveChanges();

                if (model.AssignTo != 0 && model.AssignBy!=0)
                {
                    var assignToUser = await _dbcontext.User.FirstOrDefaultAsync(u => u.Id == model.AssignTo);
                    var assignByUser = await _dbcontext.User.FirstOrDefaultAsync(u => u.Id == model.AssignBy);
                    var priority = await _dbcontext.Priority.FirstOrDefaultAsync(u => u.id == model.priority);
                    var project = await _dbcontext.Project.FirstOrDefaultAsync(u => u.Id == model.ProjectId);
                    var subtask = await _dbcontext.SubTask.FirstOrDefaultAsync(u => u.Id == model.subTaskId);

                    if (assignToUser != null && assignByUser!=null ) 
                    {

                        //string htmlTemplatePath = System.IO.File.ReadAllText(@"./Template/AssignTaskTemplate.html"); 
                        string htmlTemplatePath= @"C:\Bhagyashree Work\Onboarding\C#\TaskManagement(April)\Template\AssignTaskTemplate.html";
                        string htmlTemplate = File.ReadAllText(htmlTemplatePath);

                       
                        htmlTemplate = htmlTemplate.Replace("{{AssignTo}}", assignToUser.Name);
                        htmlTemplate = htmlTemplate.Replace("{{AssignBy}}", assignByUser.Name);
                        htmlTemplate = htmlTemplate.Replace("{{Task}}", model.Name);
                        htmlTemplate = htmlTemplate.Replace("{{startDate}}", model.StartDate.ToString());
                        htmlTemplate = htmlTemplate.Replace("{{endDate}}", model.EndDate.ToString());
                        htmlTemplate = htmlTemplate.Replace("{{ProjectName}}", project.ProjectName);
                        htmlTemplate = htmlTemplate.Replace("{{SubTaskName}}", subtask.SubTaskName);
                        htmlTemplate = htmlTemplate.Replace("{{priority}}",priority.PriorityName);
                        Email emailMessage = new Email
                        {
                            From=assignByUser.Email,
                            To = assignToUser.Email,
                            CC="bhayashree.gaikwad@wonderbiz.in",
                            Subject = "New Task Assigned",
                            //Body = $"Hello {assignToUser.Name},<br/><br/>A task has been assigned to you: {model.Name}." +
                            //$"Regards," +
                            //$"{assignByUser.Name}"
                           Body = htmlTemplate
                    };
                        await _emailService.SendEmailAsync(emailMessage);
                    }
                }
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task CheckIncompleteTasksAndSendEmail()
        {
            var today = DateTime.Today;
            var incompleteTasks = await _dbcontext.Task
                .Where(t => t.Status !=3 && t.EndDate.Date == today)
                .ToListAsync();

            foreach (var task in incompleteTasks)
            {
                var assignToUser = await _dbcontext.User.FirstOrDefaultAsync(u => u.Id == task.AssignTo);
                var assignByUser = await _dbcontext.User.FirstOrDefaultAsync(u => u.Id == task.AssignBy);
                var priority = await _dbcontext.Priority.FirstOrDefaultAsync(u => u.id == task.priority);
                var project = await _dbcontext.Project.FirstOrDefaultAsync(u => u.Id == task.ProjectId);
                var subtask = await _dbcontext.SubTask.FirstOrDefaultAsync(u => u.Id == task.subTaskId);
                if (assignToUser != null && assignByUser != null)
                {
                    string htmlTemplatePath = @"C:\Bhagyashree Work\Onboarding\C#\TaskManagement(April)\Template\TaskNotCompleted.html";
                    string htmlTemplate = File.ReadAllText(htmlTemplatePath);


                    htmlTemplate = htmlTemplate.Replace("{{AssignTo}}", assignToUser.Name);
                    htmlTemplate = htmlTemplate.Replace("{{AssignBy}}", assignByUser.Name);
                    htmlTemplate = htmlTemplate.Replace("{{Task}}", task.Name);
                    htmlTemplate = htmlTemplate.Replace("{{startDate}}", task.StartDate.ToString());
                    htmlTemplate = htmlTemplate.Replace("{{endDate}}", task.EndDate.ToString());
                    htmlTemplate = htmlTemplate.Replace("{{ProjectName}}", project.ProjectName);
                    htmlTemplate = htmlTemplate.Replace("{{SubTaskName}}", subtask.SubTaskName);
                    htmlTemplate = htmlTemplate.Replace("{{priority}}", priority.PriorityName);
                    var emailMessage = new Email
                    {
                        From = assignByUser.Email,
                        To = assignToUser.Email,
                        CC = "bhayashree.gaikwad@wonderbiz.in",
                        Subject = "Incomplete Task Reminder",
                        //Body = $"Hello {assignToUser.Name},<br/><br/>This is a reminder that the task \"{task.Name}\" is incomplete and its end date is today."
                        Body = htmlTemplate
                    };

                    await _emailService.SendEmailAsync(emailMessage);
                }
            }
        }
        public async Task<bool> UpdateTask(Tasks model,int id)
        {
            try
            {

                var task=_dbcontext.Task.FirstOrDefault(t=>t.Id==id);
                if (task != null)
                {
                    task.Name=model.Name;
                    task.AssignBy = model.AssignBy;
                    task.Status=model.Status;
                    task.AssignTo = model.AssignTo;
                    task.priority = model.priority;
                    task.StartDate = model.StartDate;
                    task.EndDate=model.EndDate;
                    task.subTaskId = model.subTaskId;
                    task.ProjectId = model.ProjectId;
                    _dbcontext.SaveChanges();

                    if (model.AssignTo != 0 && model.AssignBy != 0)
                    {
                        var assignToUser = await _dbcontext.User.FirstOrDefaultAsync(u => u.Id == model.AssignTo);
                        var assignByUser = await _dbcontext.User.FirstOrDefaultAsync(u => u.Id == model.AssignBy);
                        var priority = await _dbcontext.Priority.FirstOrDefaultAsync(u => u.id == model.priority);
                        var project = await _dbcontext.Project.FirstOrDefaultAsync(u => u.Id == model.ProjectId);
                        var subtask = await _dbcontext.SubTask.FirstOrDefaultAsync(u => u.Id == model.subTaskId);

                        if (assignToUser != null && assignByUser != null)
                        {
                            string htmlTemplatePath = @"C:\Bhagyashree Work\Onboarding\C#\TaskManagement(April)\Template\UpdateTaskTemplate.html";
                            string htmlTemplate = File.ReadAllText(htmlTemplatePath);


                            htmlTemplate = htmlTemplate.Replace("{{AssignTo}}", assignToUser.Name);
                            htmlTemplate = htmlTemplate.Replace("{{AssignBy}}", assignByUser.Name);
                            htmlTemplate = htmlTemplate.Replace("{{Task}}", model.Name);
                            htmlTemplate = htmlTemplate.Replace("{{startDate}}", model.StartDate.ToString());
                            htmlTemplate = htmlTemplate.Replace("{{endDate}}", model.EndDate.ToString());
                            htmlTemplate = htmlTemplate.Replace("{{ProjectName}}", project.ProjectName);
                            htmlTemplate = htmlTemplate.Replace("{{SubTaskName}}", subtask.SubTaskName);
                            htmlTemplate = htmlTemplate.Replace("{{priority}}", priority.PriorityName);
                            Email emailMessage = new Email
                            {
                                From = assignByUser.Email,
                                To = assignToUser.Email,
                                CC = "bhayashree.gaikwad@wonderbiz.in",
                                Subject = "Task is Updated",
                                //Body = $"Hello {assignToUser.Name},<br/><br/>A task has been assigned to you: {model.Name}." +
                                //$"Regards," +
                                //$"{assignByUser.Name}"
                                Body = htmlTemplate
                            };
                            await _emailService.SendEmailAsync(emailMessage);
                        }
                    }
                    return true;
                }
                else
                {
                    return false;

                }

                return false;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IQueryable<Tasks>> SearchTask(SearchTasks model,string sortBy,int pageNumber,int pageSize)
        {
            try
            {
                var query = _dbcontext.Task.AsQueryable();
                if (!model.Name.IsNullOrEmpty())
                {
                    query = query.Where(p => p.Name.Contains(model.Name));
                }

                if (model.ProjectId != 0)
                {
                    query = query.Where(p => p.ProjectId == model.ProjectId);
                }
                if (model.subTaskId != 0)
                {
                    query = query.Where(p => p.subTaskId == model.subTaskId);
                }
                if (model.priority != 0)
                {
                    query = query.Where(p => p.priority == model.priority);
                }
                if (model.Status != 0)
                {
                    query = query.Where(p => p.Status == model.Status);
                }
                if (model.AssignBy != 0)
                {
                    query = query.Where(p => p.AssignBy == model.AssignBy);
                }
                if (model.AssignTo != 0)
                {
                    query = query.Where(p => p.AssignTo == model.AssignTo);
                }
                if (!string.IsNullOrEmpty(sortBy))
                {
                    query = ApplySorting(query,sortBy);
                }
                else
                {
                    query = query.OrderBy(p => p.Id);
                }

                var paginatedProjects = await query.Skip((pageNumber - 1) *pageSize).Take(pageSize).ToListAsync();

                return paginatedProjects.AsQueryable();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private IQueryable<Tasks> ApplySorting(IQueryable<Tasks> query, string sortBy)
        {

            try
            {
                switch (sortBy)
                {
                    case "Name":
                        query = query.OrderBy(p => p.Name);
                        break;
                    case "AssignTo":
                        query = query.OrderBy(p => p.AssignTo);
                        break;
                    case "AssignBy":
                        query = query.OrderBy(p => p.AssignBy);
                        break;
                    case "StartDate":
                        query = query.OrderBy(p => p.StartDate);
                        break;
                    case "EndDate":
                        query = query.OrderBy(p => p.EndDate);
                        break;
                    case "Status":
                        query = query.OrderBy(p => p.Status);
                        break;
                    case "priority":
                        query = query.OrderBy(p => p.priority);
                        break;
                    default:
                        query = query.OrderBy(p => p.Id);
                        break;
                }
                return query.AsQueryable();

            }
            catch (Exception ex)
            {
                throw new ArgumentException("Invalid sortBy parameter");
            }
        }



    }
}
