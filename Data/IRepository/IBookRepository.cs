using CollegeManagement.Data.Models;
using CollegeManagement.Models;

namespace CollegeManagement.Data.IRepository
{
    public interface IBookRepository : ICollegeRepository<Book>
    {
        Task<List<Book>> GetBookByReviews();
    }
}
