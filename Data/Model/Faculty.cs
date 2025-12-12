namespace CollegeManagement.Data.Model
{
    public class Faculty
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FacultyCode { get; set; }
        public int SchoolId { get; set; }

        public virtual School? School { get; set; }

        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
        public virtual ICollection<FacultyHead> FacultyHeads { get; set; }
        public virtual ICollection<Department> Departments { get; set; }
    }
}
