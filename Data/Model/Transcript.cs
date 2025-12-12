using CollegeManagement.Data.Identity;

namespace CollegeManagement.Data.Model
{
    public class Transcript
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        //public int AcademicSessionId { get; set; }

        public decimal CGPA { get; set; } // Cumulative Grade Point Average
        public int TotalCreditsEarned { get; set; }
        public int TotalCreditsAttempted { get; set; } // Optional, but useful
        public DateTime GeneratedAt { get; set; }

        public virtual Student? Student { get; set; }
        //public virtual AcademicSession? AcademicSession { get; set; }
        //public ICollection<AcademicRecord> AcademicRecords { get; set; } = new List<AcademicRecord>();

        

    }
}
