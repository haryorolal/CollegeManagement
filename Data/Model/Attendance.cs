using CollegeManagement.Data.Identity;

namespace CollegeManagement.Data.Model
{
    public class Attendance
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }

        public string? AttendanceDateCreated { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Student? Student { get; set; }
        public virtual Course? Course { get; set; }
    }
}
