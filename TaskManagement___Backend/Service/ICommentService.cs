using TaskManagement_April_.Model;

namespace TaskManagement_April_.Service
{
    public interface ICommentService
    {
        public Task<IQueryable> GetCommentsForTask(int taskId);
        public Task<bool> SaveComment(Comment model);
        public Task<bool> UpdateComment(Comment model, int id);
        public Task<bool> DelComment(int id);
    }
}
