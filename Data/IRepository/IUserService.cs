using CollegeManagement.Data.Identity;
using CollegeManagement.Models;

namespace CollegeManagement.Data.IRepository
{
    public interface IUserService : ICollegeRepository<User>
    {
        Task<APIResponse> AuthenticateUser(LoginDTO dto, IConfiguration _configuration);
        Task<List<UserResponse>> GetAllUsers();
        Task<UserResponse> GetUserById(int Id);
        Task<UserResponse> GetUserByUsername(string Username);
        Task<bool> CreateUserAsync(UserDTO dto);
        Task<bool> UpdateUser(UserDTO dto);
        Task<bool> DeleteUser(int Id);
    }
}
