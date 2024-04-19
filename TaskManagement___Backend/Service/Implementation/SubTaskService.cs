using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TaskManagement_April_.Context;
using TaskManagement_April_.Model;

namespace TaskManagement_April_.Service.Implementation
{
    public class SubTaskService : ISubTaskService
    {
        #region Variable
        private TaskManagementContext _dbcontext;
        #endregion

        #region Constructor
        public SubTaskService(TaskManagementContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        #endregion
        public Task<bool> DelSubTask(int id)
        {
            try
            {
                var result = _dbcontext.SubTask.FirstOrDefault(x => x.Id == id);
                if (result != null)
                {
                    _dbcontext.SubTask.Remove(result);
                    _dbcontext.SaveChanges();
                    return Task.FromResult(true);
                }
                else { return Task.FromResult(false); }

            }
            catch (Exception ex)
            {
                return Task.FromResult(false);
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

        public Task<bool> SaveSubTask(SubTask model)
        {
            try
            {
                _dbcontext.SubTask.Add(model);
                _dbcontext.SaveChanges();
                return Task.FromResult(true);
            }
            catch( Exception ex)
            {
                return Task.FromResult(false);
            }
        }
        public Task<bool> UpdateSubTask(SubTask model,int id)
        {
            try
            {

                var task = _dbcontext.SubTask.FirstOrDefault(t => t.Id == id);
                if (task != null)
                {
                    task.SubTaskName = model.SubTaskName;
                    task.projectId = model.projectId;
                    _dbcontext.SaveChanges();
                    return Task.FromResult(true);
                }
                else
                {
                    return Task.FromResult(false);

                }

                return Task.FromResult(false);

            }
            catch (Exception ex)
            {
                return Task.FromResult(false);
            }
        }
    }
}
