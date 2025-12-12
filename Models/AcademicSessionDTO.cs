namespace CollegeManagement.Models
{
    public class AcademicSessionDTO
    {
        public int Id { get; set; }
        public int StartYear { get; set; } // e.g., 2024
        public int EndYear { get; set; }   // e.g., 2025
        public string Term { get; set; }   // e.g., "First Semester", "Second Semester"

        public bool IsCurrentYear { get; set; }   
    }
}
