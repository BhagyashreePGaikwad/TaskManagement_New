using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TaskManagement_April_.Context;
using TaskManagement_April_.Model;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TaskManagement_April_.Service.Implementation
{
    public class ProjectService : IProjectService
    {
        #region Variable
        private TaskManagementContext _dbcontext;
        #endregion

        #region Constructor
        public ProjectService(TaskManagementContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        #endregion
        public Task<bool> DelProject(int id)
        {
            try
            {
                var proj=_dbcontext.Project.FirstOrDefault(x => x.Id == id);
                if (proj != null)
                {
                    _dbcontext.Project.Remove(proj);
                    _dbcontext.SaveChanges();
                    return Task.FromResult(true);
                }
                else { return Task.FromResult(false); }

            }catch (Exception ex)
            {
                return Task.FromResult(false);
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

        public Task<bool> SaveProject(Project model)
        {
            try
            {
                var proj=_dbcontext.Project.FirstOrDefault(p=>p.ProjectName == model.ProjectName);
                if (proj != null)
                {
                    return Task.FromResult(false);
                }
                else
                {
                    var project = new Project()
                    {
                        Id = model.Id,
                        ProjectName = model.ProjectName,
                        Description = model.Description,
                        StartDate = model.StartDate,
                        EndDate = model.EndDate,
                        ReportingManger=model.ReportingManger
                       
                    };
                    _dbcontext.Project.Add(project);
                    _dbcontext.SaveChanges();
                    return Task.FromResult(true);
                }
                return Task.FromResult(false);
            }catch (Exception ex) {
                throw;
            }
        }
        public Task<bool> UpdateProject(Project model, int id)
        {
            try
            {
                var proj = _dbcontext.Project.FirstOrDefault(p => p.Id == id);
                if (proj == null)
                {
                    return Task.FromResult(false);
                }
                else
                {

                   
                    proj.ProjectName = model.ProjectName;
                    proj.Description = model.Description;
                    proj.StartDate = model.StartDate;
                    proj.EndDate = model.EndDate;
                    proj.ReportingManger = model.ReportingManger;
                    _dbcontext.SaveChanges();
                    return Task.FromResult(true);
                }
                return Task.FromResult(false);
            }
            catch (Exception ex)
            {
                throw;
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
