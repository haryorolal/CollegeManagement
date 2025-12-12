using CollegeManagement.Data.Identity;
using CollegeManagement.Models;

namespace CollegeManagement.Data.IRepository
{
    public interface IStaffRepository : ICollegeRepository<Staff>
    {
        //Task<UserCapacity> AddUserCapacity(StaffDTO dto);
        Task<string> GenerateStaffNumber();
    }
}
