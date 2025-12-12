using System.ComponentModel.DataAnnotations.Schema;

namespace CollegeManagement.Models
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string? Password { get; set; }
        public int UserTypeId { get; set; }
        [NotMapped]
        public int? schoolId { get; set; }
        public bool IsActive { get; set; }
    }

    
}
