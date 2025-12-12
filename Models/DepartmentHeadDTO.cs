namespace CollegeManagement.Models
{
    public class DepartmentHeadDTO
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public int StaffId { get; set; }
        public DateTime StartYear { get; set; }
        public DateTime EndYear { get; set; }
        public bool IsCurrent { get; set; }
        public bool IsDeleted { get; set; }
    }
}
