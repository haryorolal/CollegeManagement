namespace CollegeManagement.Models
{
    public class ExamQuestionDTO
    {
        public int Id { get; set; }
        public int ExamId { get; set; }
        public int QuestionId { get; set; }
        public int Order { get; set; }  // order of appearance
        public decimal Marks { get; set; } // optional, score weight
    }
}
