namespace CollegeManagement.Data.Identity
{
    public class StudentParent
    {
        public int StudentId { get; set; }
        public int ParentId { get; set; }

        public virtual Student? Student { get; set; }
        public virtual Parent? Parent { get; set; }
    }

}
