using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using TaskManagement_April_.Context;
using TaskManagement_April_.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaskManagement_April_.Service.Implementation
{
    public class SubTaskService : ISubTaskService
    {
        #region Variable
        private TaskManagementContext _dbcontext;
        private ITaskService _taskService;
        #endregion

        #region Constructor
        public SubTaskService(TaskManagementContext dbcontext,ITaskService taskService)
        {
            _dbcontext = dbcontext;
            _taskService = taskService;
        }
        #endregion
        public async Task<bool> DelSubTask(int id)
        {
            try
            {
                var result = _dbcontext.SubTask.FirstOrDefault(x => x.Id == id);
                if (result != null)
                {
                    var delTask= await _taskService.DelTaskwithSubTaskId(id);
                    if (delTask)
                    {
                        _dbcontext.SubTask.Remove(result);
                        _dbcontext.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                  
                }
                else { return false; }

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DelSubTaskwithProject(int projid)
        {
            try
            {
                var result = _dbcontext.SubTask.Where(x => x.Id == projid).ToList();
                var delTask = await _taskService.DelTaskwithProjId(projid);
                if (delTask)
                {
                    if (result != null && result.Any())
                    {

                        _dbcontext.SubTask.RemoveRange(result);
                        _dbcontext.SaveChanges();
                        return true;
                    }
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

        public async Task<IQueryable> GetAllSubTaskOfProject(int projectId)
        {
            try
            {
                var subtask = await (from i in _dbcontext.SubTask
                                 where i.projectId == projectId
                                 select new
                                 {
                                   id= i.Id,
                                   SubTaskName=i.SubTaskName,
                                   projectId=i.projectId
                                 }).ToListAsync();

                return subtask.AsQueryable();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IQueryable> GetSubTaskbyId(int id)
        {
            try
            {
               var result = await _dbcontext.SubTask.Where(e => e.Id == id).Select(x =>
               new
               {
                   x.Id,
                   x.projectId,
                   x.SubTaskName
               }).ToListAsync();
                return result.AsQueryable();

            }
            catch(Exception ex) {
                throw;
            }

        }
        public async Task<IQueryable> SearchSubtask(SearchSubTask model)
        {
           try
            {
                var query = _dbcontext.SubTask.AsQueryable();
                if (!model.subTask.IsNullOrEmpty())
                {
                    query = query.Where(p => p.SubTaskName.Contains(model.subTask));
                }

                if (model.projId!= 0 && model.projId!=null)
                {
                    query = query.Where(p => p.projectId ==model.projId);
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

        private IQueryable<SubTask> ApplySorting(IQueryable<SubTask> query, string sortBy)
        {

            try
            {
                switch (sortBy)
                {
                    case "projectId":
                        query = query.OrderBy(p => p.projectId);
                        break;
                    case "SubTaskName":
                        query = query.OrderBy(p => p.SubTaskName);
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

        public Task<(bool,string,int)> SaveSubTask(SubTask model)
        {
            try
            {
                if (!_dbcontext.Project.Any(u => u.Id == model.projectId))
                {
                    return Task.FromResult((false, "Invalid ProjectId", 400));
                }
            
                _dbcontext.SubTask.Add(model);
                _dbcontext.SaveChanges();
                return Task.FromResult((true, "Subtask Added successfully", 200));
            }
            catch( Exception ex)
            {
                 return Task.FromResult((false, ex.Message, 400));
            }
        }
        public Task<(bool,string,int)> UpdateSubTask(SubTask model,int id)
        {
            try
            {

                var task = _dbcontext.SubTask.FirstOrDefault(t => t.Id == id);
                if (task != null)
                {
                    if (!_dbcontext.Project.Any(u => u.Id == model.projectId))
                    {
                        return Task.FromResult((false, "Invalid ProjectId", 400));
                    }

                    task.SubTaskName = model.SubTaskName;
                    task.projectId = model.projectId;
                    _dbcontext.SaveChanges();
                    return Task.FromResult((true,"Subtask updated successsfully",200));
                }
                else
                {
                    return Task.FromResult((false, "subtask does not exist", 404));

                }

                return Task.FromResult((false, "subtask cannot be updated", 400));

            }
            catch (Exception ex)
            {
                return Task.FromResult((false, ex.Message, 400));
            }
        }
    }
}
