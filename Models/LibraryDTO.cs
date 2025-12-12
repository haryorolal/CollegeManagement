using CollegeManagement.Data.Model;

namespace CollegeManagement.Models
{
    public class LibraryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TotalRegisteredLibraryCards { get; set; }
        public int TotalRegisteredStudents { get; set; }
        public int TotalRegisteredBooks { get; set; }
        public int SchoolId { get; set; }
    }
}
