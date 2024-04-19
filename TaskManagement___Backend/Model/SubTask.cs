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

    public class SearchSubTask
    {
        public string? subTask { get; set; }
        public int? projId { get; set; }
        public string? sortBy { get; set; }
        public int pageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 5;
    }

}
