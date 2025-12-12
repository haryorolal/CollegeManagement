namespace CollegeManagement.Data.Model
{
    public class CourseLevel
    {
        public int Id { get; set; }
        public int LevelName { get; set; }  // e.g. 100, 200, 300 Level
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
