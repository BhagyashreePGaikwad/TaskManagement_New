using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement_April_.Model
{
    public class Comment
    {
        public int Id { get; set; }
        public string CommentByUser { get; set; }
        [ForeignKey("Tasks")]
        public int TaskId { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ForeignKey("Comment")]
        public int? ReplyToCommentId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public virtual IList<int> TaggedUserIds { get; set; }
        // public virtual ICollection<Comment> Replies { get; set; }
    }
}
