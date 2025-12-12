using AutoMapper;
using CollegeManagement.Data.IRepository;
using CollegeManagement.Data.Model;
using CollegeManagement.Models;
using CollegeManagement.Data.Identity;
using System.Net;

namespace CollegeManagement.Data.Repository
{
    public class ExamRepository : CollegeRepository<Exam>, IExamRepository
    {
        public readonly LibraryDbContext _libraryDbContext;
        public IMapper _mapper;
        public APIResponse _apiResponse;
        public ExamRepository(LibraryDbContext libraryDbContext, IMapper mapper) : base(libraryDbContext, mapper)
        {
            _libraryDbContext = libraryDbContext;
            _mapper = mapper;
        }
        
    }
}
