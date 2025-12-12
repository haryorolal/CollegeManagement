using CollegeManagement.Data.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollegeManagement.Models
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ISBN { get; set; }
        public int? TotalBookReviews { get; set; }
        public int AuthorId { get; set; }
        public int LibraryId { get; set; }
    }
}
