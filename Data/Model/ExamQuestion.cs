namespace CollegeManagement.Data.Model
{
    public class ExamQuestion
    {
        public int Id { get; set; }
        public int ExamId { get; set; }
        public int QuestionId { get; set; }
        public int Order { get; set; }  // order of appearance
        public decimal Marks { get; set; } // optional, score weight

        public virtual Exam? Exam { get; set; }
        public virtual Question? Question { get; set; }
    }

}
