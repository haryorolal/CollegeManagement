using System.ComponentModel.DataAnnotations;

namespace CollegeManagement.Data.Model
{
    public class AcademicSession
    {
        public int Id { get; set; }
        public int StartYear { get; set; } // e.g., 2024
        public int EndYear { get; set; }   // e.g., 2025
        public string Term { get; set; }   // e.g., "First Semester", "Second Semester"

        public bool IsCurrentYear { get; set; }
        public int Duration { get; set; } // Duration in 6 years      


        public virtual ICollection<AcademicRecord> AcademicRecords { get; set; } = new List<AcademicRecord>();
        public virtual ICollection<SessionTranscript> SessionTranscripts { get; set; } = new List<SessionTranscript>();

        public AcademicSession()
        {
            Duration = EndYear - StartYear;
        }
    }
}
