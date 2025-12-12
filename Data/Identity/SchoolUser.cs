using CollegeManagement.Data.Model;

namespace CollegeManagement.Data.Identity
{
    public class SchoolUser
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SchoolId { get; set; }
        public string Role { get; set; } // SchoolAdmin, Teacher, Librarian, etc.

        public virtual User? User { get; set; }
        public virtual School? School { get; set; }
    }
}
