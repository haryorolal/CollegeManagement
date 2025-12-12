namespace CollegeManagement.Models
{
    public class StaffDTO
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public int? SchoolId { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Designation { get; set; } //lecturer, Secretary e.t.c
        public string StaffType { get; set; } // teaching or non teaching staff
        public string StaffNumber { get; set; }
        public DateTime DateOfEmployement { get; set; }
        public bool IsActive { get; set; }
    }
}
