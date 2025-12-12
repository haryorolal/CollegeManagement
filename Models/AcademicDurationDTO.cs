namespace CollegeManagement.Models
{
    public class AcademicDurationDTO
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public int TotalYears { get; set; } // e.g., 4, 5, 6
    }
}
