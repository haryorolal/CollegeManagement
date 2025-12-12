using CollegeManagement.Data.Identity;

namespace CollegeManagement.Data.Model
{
    // Additional entity for the join table
    public class StaffCourses
    {
        public int Id { get; set; }
        public int StaffId { get; set; }
        public int CourseId { get; set; }

        // Navigation properties
        public virtual Staff? Staff { get; set; }
        public virtual Course? Course { get; set; }
    }
}
