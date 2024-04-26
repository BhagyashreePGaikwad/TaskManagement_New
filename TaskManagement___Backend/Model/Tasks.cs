using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement_April_.Model
{
    public class Tasks
    {
        [Key]
        public int Id { get; set; }
        public string TaskCode { get; set; }
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
        public int? subTaskId {  get; set; }
        
        [ForeignKey("Project")]
        public int? ProjectId {  get; set; }
        public virtual IList<int>? Attachments { get; set; }
    }

    public class TaskL
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [ForeignKey("User")]
        public int AssignTo { get; set; }

        [ForeignKey("User")]
        public int AssignBy { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [ForeignKey("Status")]
        public int Status { get; set; }
        [ForeignKey("Priority")]
        public int priority { get; set; }
        [ForeignKey("SubTask")]
        public int? subTaskId { get; set; }

        [ForeignKey("Project")]
        public int? ProjectId { get; set; }
        public virtual IList<int>? Attachments { get; set; }
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

        public string? sortBy { get; set; }
        public int pageNumber { get; set; } = 5;
        public int pageSize { get; set; } = 1;

    }

    public class SearchSortTask
    {
        public int id { get; set; }
        public bool? prior { get; set; }
        public bool? endDate { get; set; }
        public DateTime? duedate { get; set; }
        public int? priority { get; set; }
    }
    public class SearchSortTask1
    {
        public bool? prior { get; set; }
        public bool? endDate { get; set; }
        public DateTime? duedate { get; set; }
        public int? priority { get; set; }
    }

    public class TaskView
    {
        public string AssignToName { get; set; }
        public string AssignByName { get; set; }
        public string AssignToEmail { get; set; }
        public string AssignByEmail { get; set; }
        public string PriorityName { get; set; }
        public string ProjectName { get; set; }
        public string SubTaskName { get; set; }
    }

    //public class TaskValidate
    //{
    //    public int valid {  get; set; }
    //    public string errormsg { get; set; }
}
