namespace CollegeManagement.Data.Identity
{
    public class RolePrivilege
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string RolePrivilegeName {  get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public virtual Role? Role { get; set; }
    }
}
