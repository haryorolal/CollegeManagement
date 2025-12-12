using CollegeManagement.Data.Model;
using CollegeManagement.Data.Models;

namespace CollegeManagement.Models
{
    public class StudentLibraryCardDTO
    {
        public int StudentId { get; set; }
        public int LibraryCardId { get; set; }
        public int BookId { get; set; }
        public bool HasBorrowedBook { get; set; }
        public bool HasReturnedBook { get; set; }
        public bool IsBookAvailable { get; set; }
    }
}
