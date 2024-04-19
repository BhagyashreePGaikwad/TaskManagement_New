using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement_April_.Model
{
    public class Project
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [ForeignKey("User")]
        public int ReportingManger { get; set; }
       
    }

    public class SearchProject
    {
        public string? ProjectName { get; set; }
        public int? ReportingManger { get; set; }
        public string? sortBy { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }

    }
}
