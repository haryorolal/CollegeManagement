namespace CollegeManagement.Data.Model
{
    public class Course
    {
        public int Id { get; set; }
        public string Code { get; set; } // CSC101
        public string Name { get; set; } //Introduction to Computer Science
        public int CreditHours { get; set; } //2, 3, 4

        public int DepartmentId { get; set; }
        public int? FacultyId { get; set; }
        public int? SchoolId { get; set; }
        public int CourseLevelId { get; set; }


        public virtual CourseLevel? CourseLevel { get; set; }
        public virtual Department? Department { get; set; }
        public virtual Faculty? Faculty { get; set; }
        public virtual School? School { get; set; }

        public virtual ICollection<AcademicRecord> AcademicRecords { get; set; } = new List<AcademicRecord>();
        public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
        public virtual ICollection<StaffCourses> StaffCourses { get; set; } = new List<StaffCourses>();
        public virtual ICollection<StudentCourses> StudentCourses { get; set; } = new List<StudentCourses>();
        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}
