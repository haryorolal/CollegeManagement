using CollegeManagement.Data.Model;

namespace CollegeManagement.Data.Identity
{
    public class SchoolAdmin
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int UserId { get; set; }
        public int SchoolId { get; set; }        
        public bool IsActive { get; set; } = true;

        public DateTime AssignedDate { get; set; } = DateTime.UtcNow;
        
        public virtual User? User { get; set; }
        public virtual School? School { get; set; }

        //public virtual ICollection<Student> Students { get; set; } = new List<Student>();
        //public virtual ICollection<Staff> Staffs { get; set; } = new List<Staff>();
        //public virtual ICollection<BookReview> BookReviews { get; set; } = new List<BookReview>();
        //public virtual ICollection<SchoolUser> SchoolUsers { get; set; } = new List<SchoolUser>();
        //public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
        //public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
        //public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();
    }
}
