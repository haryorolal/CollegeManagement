namespace CollegeManagement.Models
{
    public class SchoolAdminDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int UserId { get; set; }
        public int SchoolId { get; set; }
        public bool IsActive { get; set; }
    }
}
