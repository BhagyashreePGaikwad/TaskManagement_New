using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Reflection;
using TaskManagement_April_.Context;
using TaskManagement_April_.Model;

namespace TaskManagement_April_.Service.Implementation
{
    public class TaskService : ITaskService
    {
        #region Variable
        private TaskManagementContext _dbcontext;
        private IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private ICounterService _counterService;
        
        #endregion
        #region Constructor
        public TaskService(TaskManagementContext dbcontext, IEmailService emailService, IConfiguration configuration, ICounterService counterService)
        {
            _dbcontext = dbcontext;
            _emailService = emailService;
            _configuration = configuration;
            _counterService = counterService;
        }
        #endregion
      
        public async Task<bool> DelTask(int id)
        {
            try
            {
                var task = _dbcontext.Tasks.FirstOrDefault(x => x.Id == id);
                if (task != null)
                {
                    _dbcontext.Tasks.Remove(task);
                    await _dbcontext.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public Task<bool> DelTaskwithProjId(int projId)
        {
            try
            {
                var task = _dbcontext.Tasks.Where(t => t.ProjectId == projId).ToList();
                if (task != null && task.Any()) 
                {
                    _dbcontext.Tasks.RemoveRange(task); 
                    _dbcontext.SaveChanges();
                    return Task.FromResult(true);
                }
                else
                {
                    return Task.FromResult(true) ;
                }
                return Task.FromResult(false);
            }
            catch (Exception ex)
            {
                return Task.FromResult(false);
            }
        }

        public Task<bool> DelTaskwithSubTaskId(int subTaskId)
        {
            try
            {
                var tasks = _dbcontext.Tasks.Where(t => t.subTaskId == subTaskId).ToList();
                if (tasks != null && tasks.Any()) 
                {
                    _dbcontext.Tasks.RemoveRange(tasks); 
                    _dbcontext.SaveChanges();
                    return Task.FromResult(true);
                }
                else
                {
                    return Task.FromResult(true); 
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                return Task.FromResult(false);
            }
        }


        public async Task<IQueryable> GetAllTaskofProject(int ProjectId)
        {
            try
            {
                var task = await (from i in _dbcontext.Tasks
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
                var task = await (from i in _dbcontext.Tasks
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
                var task = await (from i in _dbcontext.Tasks
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

        public async Task<IQueryable> GetYourTaskSortByDueDateorPriority(SearchSortTask model)
        {
            try
            {    
                var task = await (from i in _dbcontext.Tasks
                                  where i.AssignTo == model.id
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

                if (model.duedate != default(DateTime) && model.duedate != null)
                {
                    task = task.Where(t => t.Duedate.Date == model.duedate.Value.Date).ToList();
                }
                if (model.priority!= null && model.priority !=0)
                {
                    task = task.Where(t => t.Priority ==model.priority).ToList();
                }
                if (model.prior!=null && model.prior !=false)
                {
                    task = task.OrderBy(t => t.Priority).ToList();                    
                }
                if (model.endDate != null && model.endDate != false)
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

        public async Task<IQueryable> GetYourTaskAssignedSortByDueDateorPriority(SearchSortTask model)
        {
            try
            {
                var task = await (from i in _dbcontext.Tasks
                                  where i.AssignBy == model.id
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
                                      Duedate = i.EndDate,
                                      SubTask = i.subTaskId
                                  }).ToListAsync();

                if (model.duedate != default(DateTime) && model.duedate != null)
                {
                    task = task.Where(t => t.Duedate.Date == model.duedate.Value.Date).ToList();
                }
                if (model.priority != null && model.priority != 0)
                {
                    task = task.Where(t => t.Priority == model.priority).ToList();
                }
                if (model.prior != null && model.prior != false)
                {
                    task = task.OrderBy(t => t.Priority).ToList();
                }
                if (model.endDate != null && model.endDate != false)
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

        public async Task<IQueryable> GetTaskSortByDueDateorPriority(SearchSortTask1 model)
        {
            try
            {
                var task = await (from i in _dbcontext.Tasks
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
                                      Duedate = i.EndDate,
                                      SubTask = i.subTaskId
                                  }).ToListAsync();

                if (model.duedate != default(DateTime) && model.duedate != null)
                {
                    task = task.Where(t => t.Duedate.Date == model.duedate.Value.Date).ToList();
                }
                if (model.priority != null && model.priority != 0)
                {
                    task = task.Where(t => t.Priority == model.priority).ToList();
                }
                if (model.prior != null && model.prior != false)
                {
                    task = task.OrderBy(t => t.Priority).ToList();
                }
                if (model.endDate != null && model.endDate != false)
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
                var result = await (from x in _dbcontext.Tasks
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

        public async Task<(bool, string)> UpdateTaskStatus(int id)
        {
            try
            {
                var task = _dbcontext.Tasks.FirstOrDefault(t => t.Id == id);
                if (task != null)
                {
                    task.Status = (task.Status % 3) + 1;
                    _dbcontext.SaveChanges();
                    return (true, "Task status updated successfully.");
                }
                else
                {
                    return (false, "Task not found.");
                }
            }catch  (Exception ex)
            {
                return (false,ex.Message);
            }
           
        }
         public async Task<(bool, string)> UpdateTaskPriority(int id)
        {
            try
            {
                var task = _dbcontext.Tasks.FirstOrDefault(t => t.Id == id);
                if (task != null)
                {
                    task.priority = (task.priority % 3) + 1;
                    _dbcontext.SaveChanges();
                    return (true, "Task priority updated successfully.");
                }
                else
                {
                    return (false, "Task not found.");
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }

        }

        public async Task<(bool,string,int,string)> SaveTask(TaskL model)
        {
            try
            {
                string generatedCode;
                var errors = new List<string>();
                //if (!_dbcontext.User.Any(u=>u.Id==model.AssignTo))
                //{
                //    errors.Add("Cannot assign to userId");
                //   // return (false,"Cannot assign to userId");
                //}
                //if (!_dbcontext.User.Any(u => u.Id == model.AssignBy))
                //{
                //    errors.Add("Cannot assign by userId");
                //    // (false, "Cannot assign by userId");
                //}
                //if (!_dbcontext.Status.Any(u => u.Id == model.Status))
                //{
                //    errors.Add("Invalid Status");
                //   // return (false, "Invalid Status");
                //}
                //if (!_dbcontext.Priority.Any(u => u.id == model.priority))
                //{
                //    errors.Add("Invalid Priority");
                //    //return (false, "Invalid Priority");
                //}
                //if (!_dbcontext.SubTask.Any(u => u.Id == model.subTaskId))
                //{
                //    errors.Add("Invalid Subtask");
                //    //return (false, "Invalid Subtask");
                //}
                //if (!_dbcontext.Project.Any(u => u.Id == model.ProjectId))
                //{
                //    errors.Add("Invalid Project");
                //   // return (false, "Invalid Project");
                //}
                //if (!model.Attachments.All(attach => _dbcontext.Documents.Select(doc => doc.DocId).Contains(attach)))
                //{
                //    errors.Add("Invalid Documents");
                //    // return ((false, "Invalid UserId"));
                //}
                //if (errors.Any())
                //{
                //    return (false, string.Join("; ", errors),400,"");
                //}
                string errorMessage = "";
                var parameters = new[]
                     {
                        new SqlParameter("@AssignTo", model.AssignTo),
                        new SqlParameter("@AssignBy", model.AssignBy),
                        new SqlParameter("@PriorityId", model.priority),
                        new SqlParameter("@ProjectId", model.ProjectId),
                        new SqlParameter("@SubTaskId", model.subTaskId),
                        new SqlParameter("@StatusId", model.Status),
                        new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, -1) { Direction = ParameterDirection.Output }
                        };

                var result = await _dbcontext.Database.ExecuteSqlRawAsync("EXECUTE TaskValidate @AssignTo, @AssignBy, @PriorityId, @ProjectId, @StatusId, @SubTaskId, @ErrorMessage OUTPUT", parameters);
                errorMessage = parameters[6].Value.ToString();
                if (result == 0 || errorMessage!="")
                {
                    return (false,errorMessage,400,"");
                }
                string prefix = _configuration["TaskCodeSettings:Prefix"];
                string suffix = _configuration["TaskCodeSettings:Suffix"];
                int codeLength = Convert.ToInt32(_configuration["TaskCodeSettings:Length"]);
                int counter = await _counterService.TaskCounter();

                generatedCode = GenerateRandomCode(prefix, suffix, codeLength, counter);
                var Model = new Tasks
                {
                    Id = model.Id,
                    TaskCode = generatedCode,
                    Name = model.Name,
                    StartDate = model.StartDate,
                    Description = model.Description,
                    EndDate = model.EndDate,
                    ProjectId = model.ProjectId,
                    Attachments = model.Attachments,
                    priority = model.priority,
                    subTaskId = model.subTaskId,
                    AssignTo = model.AssignTo,
                    AssignBy = model.AssignBy,
                    Status = model.Status,
                };
                _dbcontext.Tasks.Add(Model);
                _dbcontext.SaveChanges();
                var sendEmail = false;
                if (model.AssignTo != 0 && model.AssignBy!=0)
                {


                    //var parameters = new[]
                    //{
                    //    new SqlParameter("@AssignTo", model.AssignTo),
                    //    new SqlParameter("@AssignBy", model.AssignBy),
                    //    new SqlParameter("@PriorityId", model.priority),
                    //    new SqlParameter("@ProjectId", model.ProjectId),
                    //    new SqlParameter("@SubTaskId", model.subTaskId)
                    //};

                    //var data =await _dbcontext.TaskView.FromSqlRaw("EXECUTE GetTaskData @AssignTo, @AssignBy, @PriorityId, @ProjectId, @SubTaskId", parameters).ToListAsync();
                    //var result = data.FirstOrDefault();
                    ////var assignToUser = await _dbcontext.User.FirstOrDefaultAsync(u => u.Id == model.AssignTo);
                    ////var assignByUser = await _dbcontext.User.FirstOrDefaultAsync(u => u.Id == model.AssignBy);
                    ////var priority = await _dbcontext.Priority.FirstOrDefaultAsync(u => u.id == model.priority);
                    ////var project = await _dbcontext.Project.FirstOrDefaultAsync(u => u.Id == model.ProjectId);
                    ////var subtask = await _dbcontext.SubTask.FirstOrDefaultAsync(u => u.Id == model.subTaskId);

                    ////if (assignToUser != null && assignByUser!=null ) 
                    //if (result!=null)
                    //{
                    ////string htmlTemplatePath = System.IO.File.ReadAllText(@"./Template/AssignTaskTemplate.html"); 
                    ////string htmlTemplatePath= @"C:\Bhagyashree Work\Onboarding\C#\TaskManagement(April)\Template\AssignTaskTemplate.html";

                    //    string htmlTemplatePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Template", "AssignTaskTemplate.html");

                    //    string htmlTemplate = File.ReadAllText(htmlTemplatePath);


                    //    htmlTemplate = htmlTemplate.Replace("{{AssignTo}}", result.AssignToName);
                    //    htmlTemplate = htmlTemplate.Replace("{{AssignBy}}", result.AssignByName);
                    //    htmlTemplate = htmlTemplate.Replace("{{Task}}", model.Name);
                    //    htmlTemplate = htmlTemplate.Replace("{{startDate}}", model.StartDate.ToString());
                    //    htmlTemplate = htmlTemplate.Replace("{{endDate}}", model.EndDate.ToString());
                    //    htmlTemplate = htmlTemplate.Replace("{{ProjectName}}", result.ProjectName);
                    //    htmlTemplate = htmlTemplate.Replace("{{SubTaskName}}", result.SubTaskName);
                    //    htmlTemplate = htmlTemplate.Replace("{{priority}}",result.PriorityName);
                    //    Email emailMessage = new Email
                    //    {
                    //        From=result.AssignByEmail,
                    //        To = result.AssignToEmail,
                    //        CC="bhayashree.gaikwad@wonderbiz.in",
                    //        Subject = "New Task Assigned",
                    //        //Body = $"Hello {assignToUser.Name},<br/><br/>A task has been assigned to you: {model.Name}." +
                    //        //$"Regards," +
                    //        //$"{assignByUser.Name}"
                    //       Body = htmlTemplate
                    //};
                    //    sendEmail=await _emailService.SendEmailAsync(emailMessage);
                    //}

                    sendEmail = await _emailService.SendEmailTemplate(model, "AssignTaskTemplate.html",1);
                }
                if (!sendEmail)
                {
                    return (false, "Task Created successfully,But email not send",200,generatedCode);
                }
                return (true, "Task Created successfully",200,generatedCode) ;

            }
            catch (Exception ex)
            {
                return (false,ex.Message,400,"");
            }
        }


        private string GenerateRandomCode(string prefix, string suffix, int length, int counter)
        {
            string format = new string('0', length - prefix.Length - suffix.Length);
            string code = counter.ToString(format);
            return prefix + code + suffix;
        }



        public async Task CheckIncompleteTasksAndSendEmail()
        {
            var today = DateTime.Today;
            var incompleteTasks = await _dbcontext.Tasks
                .Where(t => t.Status !=3 && (t.EndDate.Date == today 
                //|| t.EndDate.Date<=today
                ))
                .ToListAsync();

            foreach (var task in incompleteTasks)
            {
                var model = new TaskL
                {
                    Id = task.Id,
                    Attachments = task.Attachments,
                    Description = task.Description,
                    Status = task.Status,
                    AssignTo = task.AssignTo,
                    AssignBy = task.AssignBy,
                    priority = task.priority,
                    ProjectId = task.ProjectId,
                    subTaskId = task.subTaskId,
                    Name = task.Name,
                    StartDate = task.StartDate,
                    EndDate = task.EndDate

                   
                };
                await _emailService.SendEmailTemplate(model, "TaskNotCompleted.html",3);
            }
          
        
        }
        public async Task<(bool,string,int)> UpdateTask(TaskL model,int id)
        {
            try
            {

                var task=_dbcontext.Tasks.FirstOrDefault(t=>t.Id==id);
                if (task != null)
                {
                    //var errors = new List<string>();
                    //if (!_dbcontext.User.Any(u => u.Id == model.AssignTo))
                    //{
                    //    errors.Add("Cannot assign to userId");
                    //    // return (false,"Cannot assign to userId");
                    //}
                    //if (!_dbcontext.User.Any(u => u.Id == model.AssignBy))
                    //{
                    //    errors.Add("Cannot assign by userId");
                    //    // (false, "Cannot assign by userId");
                    //}
                    //if (!_dbcontext.Status.Any(u => u.Id == model.Status))
                    //{
                    //    errors.Add("Invalid Status");
                    //    // return (false, "Invalid Status");
                    //}
                    //if (!_dbcontext.Priority.Any(u => u.id == model.priority))
                    //{
                    //    errors.Add("Invalid Priority");
                    //    //return (false, "Invalid Priority");
                    //}
                    //if (!_dbcontext.SubTask.Any(u => u.Id == model.subTaskId))
                    //{
                    //    errors.Add("Invalid Subtask");
                    //    //return (false, "Invalid Subtask");
                    //}
                    //if (!_dbcontext.Project.Any(u => u.Id == model.ProjectId))
                    //{
                    //    errors.Add("Invalid Project");
                    //    // return (false, "Invalid Project");
                    //}
                    //if (!model.Attachments.All(attach => _dbcontext.Documents.Select(doc => doc.DocId).Contains(attach)))
                    //{
                    //    errors.Add("Invalid Documents");
                    //    // return ((false, "Invalid UserId"));
                    //}
                    //if (errors.Any())
                    //{
                    //    return (false, string.Join("; ", errors), 400);
                    //}
                    string errorMessage = "";
                    var parameters = new[]
                         {
                        new SqlParameter("@AssignTo", model.AssignTo),
                        new SqlParameter("@AssignBy", model.AssignBy),
                        new SqlParameter("@PriorityId", model.priority),
                        new SqlParameter("@ProjectId", model.ProjectId),
                        new SqlParameter("@SubTaskId", model.subTaskId),
                        new SqlParameter("@StatusId", model.Status),
                        new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, -1) { Direction = ParameterDirection.Output }
                        };

                    var result = await _dbcontext.Database.ExecuteSqlRawAsync("EXECUTE TaskValidate @AssignTo, @AssignBy, @PriorityId, @ProjectId, @StatusId, @SubTaskId, @ErrorMessage OUTPUT", parameters);
                    errorMessage = parameters[6].Value.ToString();
                    if (result == 0 || errorMessage != "")
                    {
                        return (false, errorMessage, 400);
                    }
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
                    var sendEmail = false;
                    if (model.AssignTo != 0 && model.AssignBy != 0)
                    {
                       
                        sendEmail = await _emailService.SendEmailTemplate(model, "UpdateTaskTemplate.html",2);
                       
                        if (!sendEmail)
                        {
                            return (false, "Task Created successfully,But email not send", 200);
                        }
                    }
                    return (true, "Task Updated successfully", 200);
                }
                else
                {
                    return (false,"Task does not exist",404);

                }

                return (false, "Task cannot be updated",400);

            }
            catch (Exception ex)
            {
                return (false, ex.Message,400);
            }
        }

        public async Task<IQueryable<Tasks>> SearchTask(SearchTasks model)
        {
            try
            {
                var query = _dbcontext.Tasks.AsQueryable();
                if (!model.Name.IsNullOrEmpty())
                {
                    query = query.Where(p => p.Name.Contains(model.Name));
                }

                if (model.ProjectId != 0 && model.ProjectId!=null)
                {
                    query = query.Where(p => p.ProjectId == model.ProjectId);
                }
                if (model.subTaskId != 0 && model.subTaskId!=null)
                {
                    query = query.Where(p => p.subTaskId == model.subTaskId);
                }
                if (model.priority != 0 && model.priority!=null)
                {
                    query = query.Where(p => p.priority == model.priority);
                }
                if (model.Status != 0 && model.Status != null)
                {
                    query = query.Where(p => p.Status == model.Status);
                }
                if (model.AssignBy != 0 && model.AssignBy!=null)
                {
                    query = query.Where(p => p.AssignBy == model.AssignBy);
                }
                if (model.AssignTo != 0 && model.AssignTo != null)
                {
                    query = query.Where(p => p.AssignTo == model.AssignTo);
                }
                if (!string.IsNullOrEmpty(model.sortBy))
                {
                    query = ApplySorting(query,model.sortBy);
                }
                else
                {
                    query = query.OrderBy(p => p.Id);
                }

                var paginatedProjects = await query.Skip((model.pageNumber - 1) *model.pageSize).Take(model.pageSize).ToListAsync();

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
