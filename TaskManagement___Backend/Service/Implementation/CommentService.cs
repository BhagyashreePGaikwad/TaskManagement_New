using Microsoft.EntityFrameworkCore;
using TaskManagement_April_.Context;
using TaskManagement_April_.Model;

namespace TaskManagement_April_.Service.Implementation
{
    public class CommentService : ICommentService
    {
        #region Variable
        private TaskManagementContext _dbcontext;
        #endregion
        #region Construstor
        public CommentService(TaskManagementContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        #endregion
        public Task<bool> DelComment(int id)
        {
            try
            {
                var comment = _dbcontext.Comment.FirstOrDefault(x => x.Id == id);
                if (comment != null)
                {
                    _dbcontext.Comment.Remove(comment);
                    _dbcontext.SaveChanges();
                    return Task.FromResult(true);
                }
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                return Task.FromResult(false);
            }
        }

        public async Task<IQueryable> GetCommentsForTask(int taskId)
        {
            try
            {
                var comment = await _dbcontext.Comment
              .Where(x => x.TaskId == taskId)
              .Select(x => new Comment
              {
                  Id = x.Id,
                  CommentByUser = x.CommentByUser,
                  TaskId = x.TaskId,
                  TaggedUserIds = x.TaggedUserIds,
                  CreatedAt = x.CreatedAt,
                  ReplyToCommentId=x.ReplyToCommentId,
              })
              .ToListAsync();
                if (comment != null)
                {
                    return comment.AsQueryable();
                }
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<bool> SaveComment(Comment model)
        {
            try
            {
                var comment = new Comment
                {
                    //Id = model.Id,
                    CommentByUser = model.CommentByUser,
                    TaskId = model.TaskId,
                    UserId = model.UserId,
                    ReplyToCommentId = model.ReplyToCommentId,
                    CreatedAt = model.CreatedAt,
                    TaggedUserIds = model.TaggedUserIds
                };
                _dbcontext.Comment.Add(comment);
                _dbcontext.SaveChanges();
                //foreach (var reply in model.Replies)
                //{
                //    var replyComment = new Comment
                //    {
                //        Id = model.Id,
                //        CommentByUser = model.CommentByUser,
                //        TaskId = model.TaskId,
                //        UserId = model.UserId,
                //        ReplyToCommentId = model.ReplyToCommentId,
                //        CreatedAt = model.CreatedAt,
                //        TaggedUserIds = model.TaggedUserIds
                //    };
                //    _dbcontext.Add(replyComment);
                //}
                //_dbcontext.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Task<bool> UpdateComment(Comment model, int id)
        {
            try
            {
                var comment = _dbcontext.Comment.FirstOrDefault(c => c.Id == id);
                if (comment != null)
                {
                    comment.CommentByUser = model.CommentByUser;
                   // comment.TaskId = model.TaskId;
                    comment.CreatedAt = new DateTime();
                    comment.TaggedUserIds = model.TaggedUserIds.Cast<int>().ToList();
                    // comment.UserId = model.UserId;
                    _dbcontext.SaveChanges();
                    return Task.FromResult(true);
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
