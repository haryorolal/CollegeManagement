namespace CollegeManagement.Models
{
    public class ParentDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SchoolId { get; set; }
        public string RelationshipToChild { get; set; } // father, mother, guardian
        public bool IActive { get; set; }
    }
}
