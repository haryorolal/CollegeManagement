using CollegeManagement.Data.Model;
using CollegeManagement.Models;

namespace CollegeManagement.Data.IRepository
{
    public interface IDepartmentRepository : ICollegeRepository<Department>
    {
        Task<APIResponse> GetDepartmentalHeadById(int Id);
        Task<APIResponse> CreateDepartmentalHead(DepartmentHeadDTO departmentHead);
        //Task<UserCapacity> UpdateUserCapacity(DepartmentHeadDTO dto);
    }
}
