using CollegeManagement.Data.Identity;
using CollegeManagement.Data.Models;

namespace CollegeManagement.Data.Model
{
    public class LibraryCard
    {
        public int Id { get; set; }
        public int LibraryId { get; set; }
        public int StudentId { get; set; }
        public string CardNumber { get; set; } = string.Empty; //should autogenerate

        public virtual Library? Library { get; set; }
        public virtual Student? Student { get; set; }
        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
        public virtual ICollection<Book> Books { get; set; } = new List<Book>();

    }
}
