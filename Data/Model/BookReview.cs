using CollegeManagement.Data.Identity;
using CollegeManagement.Data.Models;

namespace CollegeManagement.Data.Model
{
    public class BookReview
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; } //comment
        public int Rating { get; set; } //e.g 1-5 stars
        public int BookId { get; set; }
        public int StudentId {  get; set; }
        public bool IsApproved { get; set; } = false; // default: needs moderation
        public int? ApprovedByUserId { get; set; } //schoolAdmin

        public string? CreatedDateTimeOnString { get; set; }
        public DateTime CreatedDate { get; set; }
        
        public virtual Book? Book { get; set; }
        public virtual Student? Student { get; set; }
        public virtual User? ApprovedByUser { get; set; }
    }
}
