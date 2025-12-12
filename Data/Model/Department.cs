using CollegeManagement.Data.Identity;

namespace CollegeManagement.Data.Model
{
    public class Department
    {
        public int Id { get; set; }
        public int FacultyId { get; set; }
        public int SchoolId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DepartmentCode { get; set; }

        public virtual Faculty? Faculty { get; set; }
        public virtual School? School { get; set; }

        public virtual ICollection<AcademicDuration> AcademicDurations { get; set; } = new List<AcademicDuration>();
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
        public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();
        public virtual ICollection<DepartmentHead> DepartmentHeads { get; set; } = new List<DepartmentHead>();
        public virtual ICollection<Staff> Staffs { get; set; } = new List<Staff>();
        

    }
}
