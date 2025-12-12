using AutoMapper;
using CollegeManagement.Data.IRepository;
using CollegeManagement.Data.Model;
using CollegeManagement.Models;
using CollegeManagement.Data.Identity;
using System.Net;

namespace CollegeManagement.Data.Repository
{
    public class CourseRepository : CollegeRepository<Course>, ICourseRepository
    {
        public readonly LibraryDbContext _libraryDbContext;
        public IMapper _mapper;
        public APIResponse _apiResponse;
        public CourseRepository(LibraryDbContext libraryDbContext, IMapper mapper) : base(libraryDbContext, mapper)
        {
            _libraryDbContext = libraryDbContext;
            _mapper = mapper;
            _apiResponse = new APIResponse();
        }
        
    }
}
