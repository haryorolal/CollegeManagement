using CollegeManagement.Data.Model;
using CollegeManagement.Models;

namespace CollegeManagement.Data.Identity
{
    public class Staff
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public int? SchoolId { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Designation { get; set; } //lecturer, Secretary e.t.c
        public string StaffType { get; set; } // teaching or non teaching staff
        public string StaffNumber { get; set; }
        public DateTime DateOfEmployement { get; set; }
        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }
        public string CreatedDateOnString { get; set; }
        public string ModifiedDateOnString { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual User? User { get; set; }
        public virtual Department? Department { get; set; }
        public virtual School? School { get; set; }

        public ICollection<StaffCourses> StaffCourses { get; set; } = new List<StaffCourses>();
        public ICollection<DepartmentHead> DepartmentHeadEntries { get; set; } = new List<DepartmentHead>();
        public ICollection<FacultyHead> FacultyHeadEntries { get; set; } = new List<FacultyHead>();
        public ICollection<Question> QuestionsAuthored { get; set; } = new List<Question>();
    }
    
}
