using CollegeManagement.Data.Model;
using CollegeManagement.Data.Models;

namespace CollegeManagement.Models
{
    public class StudentDTO
    {   
        public int Id { get; set; }
        //public PersonName Name { get; set; }
        public int UserId { get; set; }
        public int DurationOfStudyId { get; set; }
        public int DepartmentId { get; set; }
        public int? SchoolId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OtherName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int? Age { get; set; }
        public string Email { get; set; }
        public string PhoneNUmber { get; set; }
        public bool CanBorrowBook { get; set; } = false;
        public bool IsActive { get; set; }

    }
}
