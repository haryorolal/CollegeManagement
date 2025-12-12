using CollegeManagement.Data.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollegeManagement.Data.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ISBN { get; set; }
        public int? TotalBookReviews => BookReviews?.Count ?? 0;
        public int AuthorId { get; set; }
        public int LibraryId { get; set; }
        public virtual Author? Author { get; set; }
        public virtual Library? Library { get; set; }
        public virtual ICollection<LibraryCard> LibraryCards { get; set; } = new List<LibraryCard>();
        public virtual ICollection<BookReview> BookReviews { get; set; } = new List<BookReview>();

    }
    
}
