

namespace CollegeManagement.Models
{
    public class UserTypeDTO
    {
        public int Id { get; set; }
        public string Code { get; set; } // SUPERADMIN, SCHOOLADMIN, STAFF, STUDENT
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
