using TaskManagement_April_.Model;

namespace TaskManagement_April_.Service
{
    public interface ITaskService
    {
        public Task<IQueryable> GetAllTaskofProject(int ProjectId);
        public Task<IQueryable> GetTaskById(int id);
        public Task<(bool, string, int,string)> SaveTask(TaskL model);
        public Task<(bool, string, int)> UpdateTask(TaskL model, int id);
        public Task<bool> DelTask(int id);
        public Task<bool> DelTaskwithSubTaskId(int subTaskId);
        public Task<bool> DelTaskwithProjId(int projId);
        public Task CheckIncompleteTasksAndSendEmail();
        public Task<IQueryable<Tasks>> SearchTask(SearchTasks model);
        public Task<IQueryable> GetAllTaskofSubTask(int subTaskId);
        public Task<IQueryable> GetTaskByAssignTo(int assignTo);
        public Task<IQueryable> GetYourTaskSortByDueDateorPriority(SearchSortTask model);
        public Task<IQueryable> GetYourTaskAssignedSortByDueDateorPriority(SearchSortTask model);
        public Task<IQueryable> GetTaskSortByDueDateorPriority(SearchSortTask1 model);
        public Task<(bool, string)> UpdateTaskStatus(int id);
        public Task<(bool, string)> UpdateTaskPriority(int id);
    }
}
