using AutoMapper;
using CollegeManagement.Data.IRepository;
using CollegeManagement.Data.Model;
using CollegeManagement.Models;
using CollegeManagement.Data.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CollegeManagement.Data.Repository
{
    public class MatricRepository : CollegeRepository<MatricNumber>, IMatricRepository
    {
        public readonly LibraryDbContext _libraryDbContext;
        public IMapper _mapper;
        public APIResponse _apiResponse;
        public MatricRepository(LibraryDbContext libraryDbContext, IMapper mapper) : base(libraryDbContext, mapper)
        {
            _libraryDbContext = libraryDbContext;
            _mapper = mapper;
            _apiResponse = new APIResponse();
        }
        public async Task<string> GenerateMatricNumber(string SchoolCode, string FacultyCode, string DepartmentCode)
        {
            //var currentYear = DateTime.Now.Year.ToString().Substring(2, 2);
            var currentYear = DateTime.Now.Year % 100;
            var prefix = $"{SchoolCode}{FacultyCode}{DepartmentCode}{currentYear}";
            var lastNumber = await _libraryDbContext.MatricNumbers
                .Where(m => m.Number.StartsWith(prefix))
                .OrderByDescending(m => m.Number)
                .Select(m => m.Number)
                .FirstOrDefaultAsync();

            int nextSequence = 1;

            if (lastNumber != null)
            {
                var sequencePart = lastNumber.Substring(prefix.Length);
                if (int.TryParse(sequencePart, out int lastSequence))
                {
                    nextSequence = lastSequence + 1;
                }
            }

            return $"{prefix}{nextSequence:D3}";            
        }

        public async Task<string> GenerateMatricNumber()
        {
            var year = DateTime.Now.Year % 100;
            var timestamp = DateTime.Now.ToString("HHmmss");
            var random = new Random().Next(100, 999);
            var matGen = $"MAT/{year}/{timestamp}/{random}";

            var existingMat = await _libraryDbContext.MatricNumbers
                .Where(m => m.Number == $"MAT{year}{timestamp}{random}")
                .FirstOrDefaultAsync();

            if (existingMat != null)
            {
                return await GenerateMatricNumber();
            }

            return matGen;
        }
        
    }
}
