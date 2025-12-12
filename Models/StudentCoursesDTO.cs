
namespace CollegeManagement.Models
{
    public class StudentCoursesDTO
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public bool IsPrimaryInstructor { get; set; }
        public string? Grade { get; set; }
        public int Semester { get; set; }
    }
}
