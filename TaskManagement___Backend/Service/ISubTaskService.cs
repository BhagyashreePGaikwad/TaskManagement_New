using System.Threading.Tasks;
using TaskManagement_April_.Model;

namespace TaskManagement_April_.Service
{
    public interface ISubTaskService
    {
        public Task<IQueryable> GetAllSubTaskOfProject(int projectId);
        public Task<IQueryable> GetSubTaskbyId(int id);
        public Task<(bool, string, int)> SaveSubTask(SubTask model);
        public Task<(bool, string,int)> UpdateSubTask(SubTask model,int id);
        public Task<IQueryable> SearchSubtask(SearchSubTask model);
        public Task<bool> DelSubTask(int id);
        public Task<bool> DelSubTaskwithProject(int projid);
    }
}
