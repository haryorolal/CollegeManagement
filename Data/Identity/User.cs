using CollegeManagement.Data.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollegeManagement.Data.Identity
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public int UserTypeId { get; set; }
        [NotMapped]
        public int? schoolId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual UserType? UserType { get; set; }
        public virtual Parent? Parent { get; set; }

        public virtual ICollection<BookReview> BookReviews { get; set; } = new List<BookReview>();
        public virtual ICollection<SchoolUser> SchoolUsers { get; set; } = new List<SchoolUser>();
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
        public virtual ICollection<Staff> Staffs { get; set; } = new List<Staff>();
        public virtual ICollection<SchoolAdmin> SchoolAdmins { get; set; } = new List<SchoolAdmin>();
    }

    public class UserResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int UserTypeId { get; set; }
        [NotMapped]
        public int? schoolId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual UserType? UserType { get; set; }
        public virtual Parent? Parent { get; set; }

        public virtual ICollection<BookReview> BookReviews { get; set; } = new List<BookReview>();
        public virtual ICollection<SchoolUser> SchoolUsers { get; set; } = new List<SchoolUser>();
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
        public virtual ICollection<Staff> Staffs { get; set; } = new List<Staff>();
        public virtual ICollection<SchoolAdmin> SchoolAdmins { get; set; } = new List<SchoolAdmin>();
    }
}
