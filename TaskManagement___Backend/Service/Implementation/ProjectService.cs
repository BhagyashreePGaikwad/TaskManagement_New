using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TaskManagement_April_.Context;
using TaskManagement_April_.Model;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaskManagement_April_.Service.Implementation
{
    public class ProjectService : IProjectService
    {
        #region Variable
        private TaskManagementContext _dbcontext;
        private ITaskService _taskService;
        private ISubTaskService _subTaskService;
        private ICounterService _counterService;
        private readonly IConfiguration _configuration;
      //  private readonly string counterFilePath = "C:\\Bhagyashree Work\\TaskManagement___New\\TaskManagement_New\\TaskManagement___Backend\\Counter\\ProjectCounter.txt";
        #endregion

        #region Constructor
        public ProjectService(TaskManagementContext dbcontext,ISubTaskService subTaskService,ITaskService taskService,IConfiguration configuration,ICounterService counterService)
        {
            _dbcontext = dbcontext;
            _taskService = taskService;
            _subTaskService = subTaskService;
            _configuration = configuration;
            _counterService = counterService;
        }
        #endregion

        //private int? LoadCounterFromFile()
        //{
        //    if (File.Exists(counterFilePath))
        //    {
        //        string counterString = File.ReadAllText(counterFilePath);
        //        if (int.TryParse(counterString, out int counterValue))
        //        {
        //            return counterValue;
        //        }
        //    }
        //    return null;
        //}
        
        //private void SaveCounterToFile(int counter)
        //{
        //    File.WriteAllText(counterFilePath, counter.ToString());
        //}
        public async Task<bool> DelProject(int id)
        {
            try
            {
                var proj=_dbcontext.Project.FirstOrDefault(x => x.Id == id);
                if (proj != null)
                {
                    var projT = await _taskService.DelTaskwithProjId(id);
                    var subTaskT = await _subTaskService.DelSubTaskwithProject(id);
                    if(projT && subTaskT)
                    {
                        _dbcontext.Project.Remove(proj);
                        _dbcontext.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else { return false; }

            }catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IQueryable> GetAllProject()
        {
            try
            {
                var result = await (from i in _dbcontext.Project
                                    select new
                                    {
                                        i.Id,
                                        i.ProjectName
                                    }).ToListAsync();
                return result.AsQueryable();
            }catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IQueryable> GetProjectById(int id)
        {

            var result = await _dbcontext.Project.Where(e => e.Id == id).Select(x =>
               new
               {
                  x.Id,
                  x.ProjectName,
                  x.Description
               }).ToListAsync();
            return result.AsQueryable();
        }

        public async Task<(bool,string,int,string)> SaveProject(ProjectF model)
        {
            try
            {
                var proj=_dbcontext.Project.FirstOrDefault(p=>p.ProjectName == model.ProjectName);
                if (proj != null)
                {
                    return((false, "Project already exists", 400,""));
                }
                else
                {
                    string prefix = _configuration["ProjectCodeSettings:Prefix"];
                    string suffix = _configuration["ProjectCodeSettings:Suffix"];
                    int codeLength = Convert.ToInt32(_configuration["ProjectCodeSettings:Length"]);
                    var projCounter =await _counterService.ProjectCounter();

                    string generatedCode = GenerateRandomCode(prefix, suffix, codeLength, projCounter);


                    if (!_dbcontext.User.Any(u => u.Id == model.ReportingManger))
                    {
                        return ((false, "Invalid UserId", 400, ""));
                    }
                    var project = new Project()
                    {
                        Id = model.Id,
                        ProjectName = model.ProjectName,
                        Description = model.Description,
                        StartDate = model.StartDate,
                        EndDate = model.EndDate,
                        ReportingManger = model.ReportingManger,
                        ProjectCode = generatedCode,
                        CreatedOn = DateTime.Now,

                    };
                    _dbcontext.Project.Add(project);
                    _dbcontext.SaveChanges();
                    return ((true, "Project added successfully", 200, generatedCode));
                }
                return ((false, "Project cannot be added", 400, ""));
            }catch (Exception ex) {
                return ((false, ex.Message, 400, ""));
            }
        }

        private string GenerateRandomCode(string prefix, string suffix, int length,int counter)
        {
            string format = new string('0', length - prefix.Length - suffix.Length);
            string code = counter.ToString(format);
            return prefix + code + suffix;
        }
        public Task<(bool,string,int)> UpdateProject(Project model, int id)
        {
            try
            {
                var proj = _dbcontext.Project.FirstOrDefault(p => p.Id == id);
                if (proj == null)
                {
                    return Task.FromResult((false, "Project does not exists",404));
                }
                else
                {

                    if (!_dbcontext.User.Any(u => u.Id == model.ReportingManger))
                    {
                        return Task.FromResult((false, "Invalid UserId", 400));
                    }
                    proj.ProjectName = model.ProjectName;
                    proj.Description = model.Description;
                    proj.StartDate = model.StartDate;
                    proj.EndDate = model.EndDate;
                    proj.ReportingManger = model.ReportingManger;
                    _dbcontext.SaveChanges();
                    return Task.FromResult((true, "Project updated successfully", 200));
                }
                return Task.FromResult((false, "Project cannot be updated",400));
            }
            catch (Exception ex)
            {
                return Task.FromResult((false, ex.Message, 400));
            }
        }
        public async Task<IQueryable<Project>> SearchProject(SearchProject model)
        {
            try
            {
                var query = _dbcontext.Project.AsQueryable();
                if (!model.ProjectName.IsNullOrEmpty())
                {
                    query = query.Where(p => p.ProjectName.Contains(model.ProjectName));
                }
               
                if (model.ReportingManger != 0 && model.ReportingManger != null)
                {
                    query = query.Where(p => p.ReportingManger == model.ReportingManger);
                }


                if (!string.IsNullOrEmpty(model.sortBy))
                {
                    query = ApplySorting(query,model.sortBy);
                }
                else
                {
                    query = query.OrderBy(p => p.Id);
                }

                var paginatedProjects = await query.Skip((model.pageNumber - 1) * model.pageSize).Take(model.pageSize).ToListAsync();

                return paginatedProjects.AsQueryable();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private IQueryable<Project> ApplySorting(IQueryable<Project> query, string sortBy)
        {
            
            try
            {
                switch (sortBy)
                {
                    case "ProjectName":
                        query = query.OrderBy(p => p.ProjectName);
                        break;
                    case "StartDate":
                        query = query.OrderBy(p => p.StartDate);
                        break;
                    case "EndDate":
                        query = query.OrderBy(p => p.EndDate);
                        break;
                    case "ReportingManger":
                        query = query.OrderBy(p => p.ReportingManger);
                        break;
                    default:
                        query = query.OrderBy(p => p.Id);
                        break;
                }
                return query.AsQueryable();

            }
            catch (Exception ex)
            {
                // Handle invalid sorting parameters
                throw new ArgumentException("Invalid sortBy parameter");
            }
        }

    }
}
