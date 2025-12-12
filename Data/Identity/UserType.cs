using CollegeManagement.Data.Model;

namespace CollegeManagement.Data.Identity
{
    public class UserType
    {
        public int Id { get; set; }
        public string Code { get; set; } // SUPERADMIN, SCHOOLADMIN, STAFF, STUDENT
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<User> Users { get; set; } = new List<User>();

        //public virtual ICollection<Student> Students { get; set; } = new List<Student>();
        //public virtual ICollection<Staff> Staffs { get; set; } = new List<Staff>();
        //public virtual ICollection<FacultyHead> FacultyHeads { get; set; } = new List<FacultyHead>();
        //public virtual ICollection<DepartmentHead> DepartmentHeads { get; set; } = new List<DepartmentHead>();
    }
}
