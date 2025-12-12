namespace CollegeManagement.Models
{
    public class DepartmentDTO
    {
        public int Id { get; set; }
        public int FacultyId { get; set; }
        public int SchoolId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DepartmentCode { get; set; }
    }
}
