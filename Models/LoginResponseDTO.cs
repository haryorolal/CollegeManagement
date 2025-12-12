using CollegeManagement.Data.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollegeManagement.Models
{
    public class LoginResponseDTO
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
        //public int RoleId { get; set; }
        //public int SchoolId { get; set; }
        public int UserTypeId { get; set; }
        public bool IsActive { get; set; }
        public SchoolUser SchoolUser { get; set; }

        //public int getOtherRoleBySchoolId(int schoolID)
        //{
        //    if (Role == "SuperAdmin")
        //        return SchoolId;

        //    SchoolId = schoolID;
        //    return ;
        //}
    }
}
