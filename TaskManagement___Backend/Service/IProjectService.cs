using TaskManagement_April_.Model;

namespace TaskManagement_April_.Service
{
    public interface IProjectService
    {
        public Task<IQueryable> GetAllProject();
        public Task<IQueryable> GetProjectById(int id);
        public Task<bool> SaveProject(Project model);
        public Task<bool> UpdateProject(Project model, int id);
        public Task<IQueryable<Project>> SearchProject(SearchProject model);
       // IQueryable<Project> ApplySorting(IQueryable<Project> query, string sortBy);
        public Task<bool> DelProject(int id);
    }
}
