using CollegeManagement.Data.Model;

namespace CollegeManagement.Models
{
    public class ExamDTO
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public int SchoolId { get; set; }
        public int CourseId { get; set; }
        public string ExamType { get; set; } // Midterm, Final, Quiz, Test

        public DateTime ExamDate { get; set; }
        public TimeSpan ExamDuration { get; set; }
        public bool IsExamStarted { get; set; } = false;
        public bool IsExamCompleted { get; set; } = false;
        public DateTime? ExamStartTime { get; set; }
        public DateTime? ExamEndTime { get; set; }
    }
}
