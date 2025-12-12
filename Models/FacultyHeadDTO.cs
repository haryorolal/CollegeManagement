namespace CollegeManagement.Models
{
    public class FacultyHeadDTO
    {
        public int Id { get; set; }
        public int StaffId { get; set; }
        public int FacultyId { get; set; }
        public bool IsCurrent { get; set; }
        public DateTime StartYear { get; set; }
        public DateTime EndYear { get; set; }
    }
}
