using TaskManagement_April_.Model;

namespace TaskManagement_April_.Service
{
    public interface ISubTaskService
    {
        public Task<IQueryable> GetAllSubTaskOfProject(int projectId);
        public Task<IQueryable> GetSubTaskbyId(int id);
        public Task<bool> SaveSubTask(SubTask model);
        public Task<bool> UpdateSubTask(SubTask model,int id);
        public Task<IQueryable> SearchSubtask(string subTask, int projId, string sortBy, int pageNumber, int pageSize);
        public Task<bool> DelSubTask(int id);
    }
}
