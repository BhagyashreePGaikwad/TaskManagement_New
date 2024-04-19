using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement_April_.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [ForeignKey("Role")]
        public int RoleId { get; set; }
       
    }

    public class Login
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
