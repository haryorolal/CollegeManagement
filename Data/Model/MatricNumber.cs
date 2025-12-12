using CollegeManagement.Data.Identity;

namespace CollegeManagement.Data.Model
{
    public class MatricNumber
    {
        public int Id { get; set; }
        public string? Number { get; set; }
        public int StudentId { get; set; }
        public virtual Student? Student { get; set; }


        //public int DepartmentId { get; set; }
        //public int FacultyId { get; set; }
        //public int SchoolId { get; set; }

        //public virtual Department? Department { get; set; }
        //public virtual Faculty? Faculty { get; set; }
        //public virtual School? School { get; set; }
    }
}
