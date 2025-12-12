using AutoMapper;
using CollegeManagement.Data.IRepository;
using CollegeManagement.Data.Model;
using CollegeManagement.Models;
using CollegeManagement.Data.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CollegeManagement.Data.Repository
{
    public class DepartmentRepository : CollegeRepository<Department>, IDepartmentRepository
    {
        public readonly LibraryDbContext _libraryDbContext;
        public IMapper _mapper;
        public APIResponse _apiResponse;
        public DepartmentRepository(LibraryDbContext libraryDbContext, IMapper mapper) : base(libraryDbContext, mapper)
        {
            _libraryDbContext = libraryDbContext;
            _mapper = mapper;
            _apiResponse = new APIResponse();
        }

        public async Task<APIResponse> GetDepartmentalHeadById(int Id)
        {
            var departmentHeads = await GetAsync(x => x.Id == Id, new List<string> { "Departments", "Departments.DepartmentHeads", "Departments.DepartmentHeads.Staff" }, false);
            if (departmentHeads == null)
                return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "No record found", null, "Invalid Id");

            var result = departmentHeads.DepartmentHeads.Select(d => d.Staff).Where(dh => dh != null);

            return _apiResponse.ResponseToClient(true, HttpStatusCode.OK, "Successful", result, string.Empty);
        }

        public async Task<APIResponse> CreateDepartmentalHead(DepartmentHeadDTO departmentHead)
        {
            var existingStaff = await _libraryDbContext.Staffs.FindAsync(departmentHead.StaffId);
            if (existingStaff == null)
                return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "Staff does not exist", null, "Invalid StaffId");

            DepartmentHead newDepartmenalHead = _mapper.Map<DepartmentHead>(departmentHead);
            newDepartmenalHead.CreatedDate = DateTime.Now;
            newDepartmenalHead.ModifiedDate = DateTime.Now;
            newDepartmenalHead.CreatedDateOnString = newDepartmenalHead.CreatedDate.ToString("dd/MM/yyyy");
            newDepartmenalHead.ModifiedDateOnString = newDepartmenalHead.ModifiedDate.ToString("dd/MM/yyyy");
            departmentHead.Id = newDepartmenalHead.Id;

            var department = await GetAsync(d => d.Id == departmentHead.DepartmentId);
            if (department == null)
                return _apiResponse.ResponseToClient(false, HttpStatusCode.NotFound, "Department does not exist", null, "Invalid DepartmentId");

            department.DepartmentHeads.Add(newDepartmenalHead);
            await UpdateAsync(department);

            return _apiResponse.ResponseToClient(true, HttpStatusCode.Created, "Departmental head created successfully", null, string.Empty);

        }

        //public async Task<UserCapacity> UpdateUserCapacity(DepartmentHeadDTO dto)
        //{
        //    List<Capacity> capacities = await _libraryDbContext.Capacities.ToListAsync();
        //    var existingStaffCapacity = await _libraryDbContext.UserCapacities.Select(x => x.StaffId).FirstOrDefaultAsync();
        //    if (existingStaffCapacity == null)
        //        return null;

        //    var staffCapacity = new UserCapacity();

        //    foreach (Capacity cap in capacities)
        //    {
        //        if (cap.Code == "DH")
        //            staffCapacity.CreateUserCapacity(null, dto.Id, null, cap.Id);
        //    }
        //    return staffCapacity;
        //}

    }
}
