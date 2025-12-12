namespace CollegeManagement.Models
{
    public class TranscriptDTO
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int AcademicSessionId { get; set; }

        public decimal GPA { get; set; } // Grade Point Average for the session
        public decimal CGPA { get; set; } // Cumulative Grade Point Average
        public int TotalCreditsEarned { get; set; }
        public DateTime CalculatedAt { get; set; } = DateTime.UtcNow;
    }
}
