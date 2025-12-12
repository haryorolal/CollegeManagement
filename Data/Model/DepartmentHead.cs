using CollegeManagement.Data.Identity;
using CollegeManagement.Models;

namespace CollegeManagement.Data.Model
{
    public class DepartmentHead
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public int StaffId { get; set; }
        public DateTime StartYear { get; set; }
        public DateTime EndYear { get; set; }
        public bool IsCurrent { get; set; }

        public bool IsDeleted { get; set; }
        public string CreatedDateOnString { get; set; }
        public string ModifiedDateOnString { get; set; }
        public DateTime CreatedDate { get; set; } 
        public DateTime ModifiedDate { get; set; } 

        public virtual Staff? Staff { get; set; }
        public virtual Department? Department { get; set; }
    }

}
