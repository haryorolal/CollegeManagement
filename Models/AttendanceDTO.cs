namespace CollegeManagement.Models
{
    public class AttendanceDTO
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
    }
}
