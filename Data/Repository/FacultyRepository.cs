using AutoMapper;
using CollegeManagement.Data.IRepository;
using CollegeManagement.Data.Model;
using CollegeManagement.Models;
using CollegeManagement.Data.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CollegeManagement.Data.Repository
{
    public class FacultyRepository : CollegeRepository<Faculty>, IFacultyRepository
    {
        public readonly LibraryDbContext _libraryDbContext;
        public IMapper _mapper;
        public APIResponse _apiResponse;
        public FacultyRepository(LibraryDbContext libraryDbContext, IMapper mapper) : base(libraryDbContext, mapper)
        {
            _libraryDbContext = libraryDbContext;
            _mapper = mapper;
            _apiResponse = new APIResponse();
        }

        public async Task<APIResponse> GetFacultyHeadById(int Id)
        {
            var departmentHeads = await GetAsync(x => x.Id == Id, new List<string> { "Departments", "Departments.DepartmentHeads", "Departments.DepartmentHeads.Staff" }, false);
            if (departmentHeads == null)
                return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "No record found", null, "Invalid Id");

            var result = departmentHeads.FacultyHeads.Select(d => d.Staff).Where(dh => dh != null);

            return _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successful", result, string.Empty);
        }

        public async Task<APIResponse> CreateFacultyHead(FacultyHeadDTO facultyHead)
        {
            var existingStaff = await _libraryDbContext.Staffs.FindAsync(facultyHead.StaffId);
            if (existingStaff == null)
                return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "Staff does not exist", null, "Invalid StaffId");

            FacultyHead newFacultyHead = _mapper.Map<FacultyHead>(facultyHead);
            newFacultyHead.CreatedDate = DateTime.Now;
            newFacultyHead.ModifiedDate = DateTime.Now;
            newFacultyHead.CreatedDateOnString = newFacultyHead.CreatedDate.ToString("dd/MM/yyyy");
            newFacultyHead.ModifiedDateOnString = newFacultyHead.ModifiedDate.ToString("dd/MM/yyyy");
            facultyHead.Id = newFacultyHead.Id;
            //var facultHeadCapacity = await UpdateUserCapacity(facultyHead);
            //newFacultyHead.UserCapacities.Add(facultHeadCapacity);

            var department = await GetAsync(d => d.Id == facultyHead.FacultyId);
            if (department == null)
                return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "Department does not exist", null, "Invalid DepartmentId");

            department.FacultyHeads.Add(newFacultyHead);
            await UpdateAsync(department);

            return _apiResponse.ResponseToClient(true, HttpStatusCode.Created, "Departmental head created successfully", null, string.Empty);
        }

        //public async Task<UserCapacity> UpdateUserCapacity(FacultyHeadDTO dto)
        //{
        //    List<Capacity> capacities = await _libraryDbContext.Capacities.ToListAsync();
        //    var existingStaffCapacity = await _libraryDbContext.UserCapacities.Select(x => x.StaffId).FirstOrDefaultAsync();
        //    if (existingStaffCapacity == null)
        //        return null;

        //    UserCapacity staffCapacity = new UserCapacity();

        //    foreach (Capacity cap in capacities)
        //    {
        //        if (cap.Code == "DH")
        //            staffCapacity.CreateUserCapacity(null, dto.Id, null, cap.Id);
        //    }
        //    return staffCapacity;
        //}


    }
}
