using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement_April_.Model
{
    public class SubTask
    {
        public int Id { get; set; }
        public string SubTaskName { get; set; }
        [ForeignKey("Project")]
        public int projectId { get; set;}
    }
}
