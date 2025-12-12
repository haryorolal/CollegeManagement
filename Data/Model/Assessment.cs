using CollegeManagement.Data.Identity;

namespace CollegeManagement.Data.Model
{
    public class Assessment
    {
        public int Id { get; set; }
        public string Type { get; set; } // Test, Exam, Quiz
        public decimal Score { get; set; }
        public DateTime TakenAt { get; set; } = DateTime.UtcNow;

        public int ExamId { get; set; }
        public int StudentId { get; set; }

        public virtual Exam? Exam { get; set; }
        public virtual Student? Student { get; set; }
    }
}
