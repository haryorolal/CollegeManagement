using CollegeManagement.Data.Identity;

namespace CollegeManagement.Data.Model
{
    public class School
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Founder { get; set; }
        public string SchoolCode { get; set; }

        public virtual ICollection<SchoolUser> SchoolUsers { get; set; } = new List<SchoolUser>();
        public virtual ICollection<SchoolAdmin> SchoolAdmins { get; set; } = new List<SchoolAdmin>();
        public virtual ICollection<Faculty> Faculties { get; set; } = new List<Faculty>();
        public virtual ICollection<Department> Departments { get; set; } = new List<Department>();
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
        public virtual ICollection<Parent> Parents { get; set; } = new List<Parent>();
        public virtual ICollection<Staff> Staffs { get; set; } = new List<Staff>();
        public virtual ICollection<Library> Libraries { get; set; } = new List<Library>();
        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
        public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();
    }
}
