using CollegeManagement.Data.Model;
using CollegeManagement.Data.Models;
using CollegeManagement.Models;

namespace CollegeManagement.Data.Identity
{
    public class Student
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int DurationOfStudyId { get; set; }
        public int DepartmentId { get; set; }     
        public int? SchoolId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OtherName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int? Age { get; set; }
        public string Email { get; set; }
        public string PhoneNUmber { get; set; }
        public bool CanBorrowBook { get; set; } = false;
        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; } = false;
        public string? CreatedDateOnString { get; set; }
        public string? ModifiedDateOnString { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual User? User { get; set; }
        public virtual Department? Department { get; set; }
        public virtual MatricNumber? MatricNumber { get; set; }
        public virtual AcademicDuration? AcademicDuration { get; set; }
        public virtual School? School { get; set; }

        public virtual Transcript? Transcript { get; set; }
        public virtual ICollection<SessionTranscript> SessionTranscripts { get; set; } = new List<SessionTranscript>();
        public virtual ICollection<AcademicRecord> AcademicRecords { get; set; } = new List<AcademicRecord>();
        public virtual ICollection<StudentCourses> StudentCourses { get; set; } = new List<StudentCourses>();
        public virtual ICollection<Assessment> Assessments { get; set; } = new List<Assessment>();
        public virtual ICollection<LibraryCard> LibraryCards { get; set; } = new List<LibraryCard>();
        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
        public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
        public virtual ICollection<BookReview> BookReviews { get; set; } = new List<BookReview>();
        public virtual ICollection<StudentParent> StudentParents { get; set; } = new List<StudentParent>();
    }
}
