using AutoMapper;
using CollegeManagement.Data.IRepository;
using CollegeManagement.Data.Model;
using CollegeManagement.Data.Models;
using CollegeManagement.Models;
using CollegeManagement.Data.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CollegeManagement.Data.Repository
{
    public class BookRepository : CollegeRepository<Book>, IBookRepository
    {
        public IMapper _mapper;
        public APIResponse _apiResponse;
        public BookRepository(LibraryDbContext libraryDbContext, IMapper mapper) : base(libraryDbContext, mapper)
        {
            _mapper = mapper;
            _apiResponse = new APIResponse();
        }
        public async Task<List<Book>> GetBookByReviews()
        {
            var result = await GetAllFilterAsync(x => x.BookReviews.Count >= 1, null, new List<string> { "BookReviews", "BookReviews.Student" }, true);            
            return result;
        }
    }
}
