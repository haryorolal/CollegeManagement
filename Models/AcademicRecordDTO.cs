using CollegeManagement.Data.Enums;

namespace CollegeManagement.Models
{
    public class AcademicRecordDTO
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int AcademicSessionId { get; set; }

        public decimal TotalScore { get; set; } // aggregate of all exam types
        public GradeLetter Grade { get; set; }
        public decimal GradePoint { get; set; } // e.g. 5.0, 4.0, 3.0, 2.0, 0.0 // derived from GradingPolicy
        public int CreditUnit { get; set; }  // pulled from Course
        public decimal WeightedPoint => GradePoint * CreditUnit;
        public bool IsPassed { get; set; }
        public bool IsCarriedOver { get; set; }
    }
}
