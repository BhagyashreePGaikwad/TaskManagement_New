using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement_April_.Model
{
    public class Documents
    {
        [Key]
        public int DocId {  get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        [ForeignKey("User")]
        public int userId {  get; set; }
    }
}
