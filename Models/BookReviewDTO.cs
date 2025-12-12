using CollegeManagement.Data.Identity;
using CollegeManagement.Data.Model;
using CollegeManagement.Data.Models;

namespace CollegeManagement.Models
{
    public class BookReviewDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; } //comment
        public int Rating { get; set; } //e.g 1-5 stars
        public string? CreatedDateTimeOnString { get; set; }
        public int BookId { get; set; }
        public int StudentId { get; set; }
        public bool IsApproved { get; set; } = false; // default: needs moderation
        public int? ApprovedByUserId { get; set; }


    }
}
