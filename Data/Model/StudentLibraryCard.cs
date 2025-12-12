using CollegeManagement.Data.Identity;
using CollegeManagement.Data.Models;

namespace CollegeManagement.Data.Model
{
    public class StudentLibraryCard
    {
        public int StudentId { get; set; }
        public int LibraryCardId { get; set; }
        public int BookId { get; set; }
        public bool HasBorrowedBook { get; set; }
        public bool HasReturnedBook { get; set; }
        public bool IsBookAvailable { get; set; }

        public virtual Student? Student { get; set; }
        public virtual LibraryCard? LibraryCard { get; set; }
        public virtual Book? Book { get; set; }
    }
}
