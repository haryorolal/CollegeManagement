using CollegeManagement.Data.Model;
using CollegeManagement.Models;

namespace CollegeManagement.Data.IRepository
{
    public interface IMatricRepository : ICollegeRepository<MatricNumber>
    {
        Task<string> GenerateMatricNumber(string SchoolCode, string FacultyCode, string DepartmentCode);
        Task<string> GenerateMatricNumber();
    }
}
