using TaskManagement_April_.Model;

namespace TaskManagement_April_.Service
{
    public interface ITaskService
    {
        public Task<IQueryable> GetAllTaskofProject(int ProjectId);
        public Task<IQueryable> GetTaskById(int id);
        public Task<bool> SaveTask(Tasks model);
        public Task<bool> UpdateTask(Tasks model, int id);
        public Task<bool> DelTask(int id);
        public Task CheckIncompleteTasksAndSendEmail();
        public Task<IQueryable<Tasks>> SearchTask(SearchTasks model, string sortBy, int pageNumber, int pageSize);
        public Task<IQueryable> GetAllTaskofSubTask(int subTaskId);
        public Task<IQueryable> GetTaskByAssignTo(int assignTo);
        public Task<IQueryable> GetYourTaskSortByDueDateorPriority(int assignTo, string? filter);


    }
}
