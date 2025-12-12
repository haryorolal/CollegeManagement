using CollegeManagement.Data.Model;
using CollegeManagement.Models;

namespace CollegeManagement.Data.IRepository
{
    public interface ILibraryCardRepository : ICollegeRepository<LibraryCard>
    {
        Task<string> GenerateLibraryCard(string name);
    }
}
