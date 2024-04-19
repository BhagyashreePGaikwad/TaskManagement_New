using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement_April_.Model
{
    public class Tasks
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [ForeignKey("User")]
        public int AssignTo{ get; set; }
       
        [ForeignKey("User")]
        public int AssignBy { get; set; }
         public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [ForeignKey("Status")]
        public int Status {  get; set; }
        [ForeignKey("Priority")]
        public int priority {  get; set; }
        [ForeignKey("SubTask")]
        public int subTaskId {  get; set; }
        
        [ForeignKey("Project")]
        public int ProjectId {  get; set; }
        
    }

    public class SearchTasks
    {
        public string? Name { get; set; }
        public int? AssignTo { get; set; }
        public int? AssignBy { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long? Status { get; set; }
        public int? priority { get; set; }
        public int? subTaskId { get; set; }
        public int? ProjectId { get; set; }

    }
}
