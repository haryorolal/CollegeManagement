using CollegeManagement.Data.Enums;
using CollegeManagement.Data.Identity;

namespace CollegeManagement.Data.Model
{
    public class AcademicRecord
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

        public DateTime CalculatedAt { get; set; } = DateTime.UtcNow;

        public virtual Course? Course { get; set; }
        public virtual AcademicSession? AcademicSession { get; set; }
        public virtual Student? Student { get; set; }
    }
}
