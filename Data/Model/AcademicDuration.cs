using CollegeManagement.Data.Identity;

namespace CollegeManagement.Data.Model
{
    public class AcademicDuration
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public int TotalYears { get; set; } // e.g., 4, 5, 6        

        public virtual Department? Department { get; set; }
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();

        

    }
}
