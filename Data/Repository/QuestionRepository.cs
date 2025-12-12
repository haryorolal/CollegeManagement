using AutoMapper;
using CollegeManagement.Data.IRepository;
using CollegeManagement.Data.Model;
using CollegeManagement.Data.Models;
using CollegeManagement.Models;
using CollegeManagement.Data.Identity;
using System.Net;

namespace CollegeManagement.Data.Repository
{
    public class QuestionRepository : CollegeRepository<Question>, IQuestionRespository
    {
        public readonly LibraryDbContext _libraryDbContext;
        public IMapper _mapper;
        public APIResponse _apiResponse;
        public QuestionRepository(LibraryDbContext libraryDbContext, IMapper mapper) : base(libraryDbContext, mapper)
        {
            _libraryDbContext = libraryDbContext;
            _mapper = mapper;
            _apiResponse = new APIResponse();
        }
        
       
    }
}
