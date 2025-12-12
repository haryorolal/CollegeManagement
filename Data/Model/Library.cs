using CollegeManagement.Data.Models;

namespace CollegeManagement.Data.Model
{
    public class Library
    {
        public int Id { get; set; }
        public string Name { get; set; }        
        public string Description { get; set; }
        public int? TotalRegisteredLibraryCards => LibraryCards?.Count ?? 0;
        public int? TotalRegisteredBooks => Books?.Count ?? 0;
        public int SchoolId { get; set; }

        public virtual School? School { get; set; }
        public virtual ICollection<LibraryCard> LibraryCards { get; set; } = new List<LibraryCard>();
        public virtual ICollection<Book> Books { get; set; } = new List<Book>();


    }
}
