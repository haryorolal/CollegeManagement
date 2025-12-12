namespace CollegeManagement.Data.Model
{
    public class Exam
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

        public string CreatedDateToString { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public virtual Course? Course { get; set; }
        public virtual Department? Department { get; set; }
        public virtual School? School { get; set; }

        // ✅ New bridge entity
        public ICollection<ExamQuestion> ExamQuestions { get; set; } = new List<ExamQuestion>();
        public ICollection<Assessment> Assessments { get; set; } = new List<Assessment>();

        public TimeSpan RemainingTime
        {
            get
            {
                if (!IsExamStarted || IsExamCompleted || ExamEndTime == null)
                    return TimeSpan.Zero;

                var remaining = ExamEndTime.Value - DateTime.UtcNow;
                return remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
            }
        }

    }
}
