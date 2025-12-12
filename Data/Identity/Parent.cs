using CollegeManagement.Data.Model;

namespace CollegeManagement.Data.Identity
{
    public class Parent
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SchoolId { get; set; }

        public string RelationshipToChild { get; set; } // father, mother, guardian
        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;

        // Navigation
        public virtual User? User { get; set; }
        public virtual School? School { get; set; }
        public virtual ICollection<StudentParent> StudentParents { get; set; } = new List<StudentParent>();
    }
}
