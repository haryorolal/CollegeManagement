namespace CollegeManagement.Models
{
    public class CourseDTO
    {
        public int Id { get; set; }
        public string Code { get; set; } // CSC101
        public string Name { get; set; } //Introduction to Computer Science
        public int CreditHours { get; set; } //2, 3, 4

        public int DepartmentId { get; set; }
        public int FacultyId { get; set; }
        public int SchoolId { get; set; }
        public int CourseLevelId { get; set; }
    }
}
