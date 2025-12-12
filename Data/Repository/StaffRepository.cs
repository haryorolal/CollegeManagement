using AutoMapper;
using CollegeManagement.Data.IRepository;
using CollegeManagement.Models;
using CollegeManagement.Data.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CollegeManagement.Data.Repository
{
    public class StaffRepository : CollegeRepository<Staff>, IStaffRepository
    {
        private readonly LibraryDbContext _libraryDbContext;
        private readonly IMapper _mapper;
        private APIResponse _apiResponse;

        public StaffRepository(LibraryDbContext libraryDbContext, IMapper mapper) : base(libraryDbContext, mapper)
        {
            _libraryDbContext = libraryDbContext;
            _mapper = mapper;
            _apiResponse = new APIResponse();
        }

        //public async Task<UserCapacity> AddUserCapacity(StaffDTO dto)
        //{
        //    List<Capacity> capacities = await _libraryDbContext.Capacities.ToListAsync();
        //    var existingStaffCapacity = await _libraryDbContext.UserCapacities.Select(x => x.StaffId).FirstOrDefaultAsync();
        //    if (existingStaffCapacity == null)
        //        return null;

        //    var staffCapacity = new UserCapacity();

        //    foreach (Capacity cap in capacities)
        //    {
        //        if (cap.Code == "EMP")
        //             staffCapacity.CreateUserCapacity(dto.Id, null, null, cap.Id);
        //    }
        //    return staffCapacity;
        //}

        public async Task<string> GenerateStaffNumber()
        {
            var year = DateTime.Now.Year % 100;
            var random = new Random().Next(100, 999);
            var matGen = $"Staff/{year}{random}";

            return matGen;
        }
    }
}
