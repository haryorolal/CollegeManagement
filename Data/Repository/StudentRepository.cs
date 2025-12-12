using AutoMapper;
using CollegeManagement.Data.IRepository;
using CollegeManagement.Models;
using CollegeManagement.Data.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CollegeManagement.Data.Repository
{
    public class StudentRepository : CollegeRepository<Student>, IStudentRepository
    {
        private readonly LibraryDbContext _libraryDbContext;
        private readonly IMapper _mapper;
        private APIResponse _apiResponse;

        public StudentRepository(LibraryDbContext libraryDbContext, IMapper mapper) : base(libraryDbContext, mapper)
        {
            _libraryDbContext = libraryDbContext;
            _mapper = mapper;
            _apiResponse = new APIResponse();
        }


        public async Task<Student> GetStudentByMatric(int matricId)
        {
            return null;
        }

        //public async Task<UserCapacity> AddUserCapacity(StudentDTO dto)
        //{
        //    List<Capacity> capacities = await _libraryDbContext.Capacities.ToListAsync();
        //    var existingStaffCapacity = await _libraryDbContext.UserCapacities.Select(x => x.StaffId).FirstOrDefaultAsync();
        //    if (existingStaffCapacity == null)
        //        return null;

        //    var staffCapacity = new UserCapacity();

        //    foreach (Capacity cap in capacities)
        //    {
        //        if (cap.Code == "STU")
        //            staffCapacity.CreateUserCapacity(null, dto.Id, null, cap.Id);
        //    }
        //    return staffCapacity;
        //}
    }
}
