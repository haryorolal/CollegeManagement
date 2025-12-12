using CollegeManagement.Data.Model;
using CollegeManagement.Models;

namespace CollegeManagement.Data.IRepository
{
    public interface IFacultyRepository : ICollegeRepository<Faculty>
    {
        Task<APIResponse> GetFacultyHeadById(int Id);
        Task<APIResponse> CreateFacultyHead(FacultyHeadDTO facultyHead);
        //Task<UserCapacity> UpdateUserCapacity(FacultyHeadDTO dto);
    }
}
