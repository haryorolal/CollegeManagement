using CollegeManagement.Data.Identity;
using CollegeManagement.Models;

namespace CollegeManagement.Data.IRepository
{
    public interface IStudentRepository : ICollegeRepository<Student>
    {
        Task<Student> GetStudentByMatric(int  matricId);
        //Task<UserCapacity> AddUserCapacity(StudentDTO dto);
    }
}
