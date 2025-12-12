namespace CollegeManagement.Models
{
    public class AssessmentDTO
    {
        public int Id { get; set; }
        public string Type { get; set; } // Test, Exam, Quiz
        public decimal Score { get; set; }
        public DateTime TakenAt { get; set; } = DateTime.UtcNow;

        public int ExamId { get; set; }
        public int StudentId { get; set; }
    }
}
