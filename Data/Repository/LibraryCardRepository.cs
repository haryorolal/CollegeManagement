using AutoMapper;
using CollegeManagement.Data.IRepository;
using CollegeManagement.Data.Model;
using CollegeManagement.Models;
using CollegeManagement.Data.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CollegeManagement.Data.Repository
{
    public class LibraryCardRepository : CollegeRepository<LibraryCard>, ILibraryCardRepository
    {
        public readonly LibraryDbContext _libraryDbContext;
        public IMapper _mapper;
        public LibraryCardRepository(LibraryDbContext libraryDbContext, IMapper mapper) : base(libraryDbContext, mapper)
        {
            _libraryDbContext = libraryDbContext;
            _mapper = mapper;
        }

        public async Task<string> GenerateLibraryCard(string name)
        {
            var random = new Random();
            var randomNumber = random.Next(1000, 9999);
            var cardNumber = $"{name.Substring(0, 3).ToUpper()} - {randomNumber}";
            
            var existingCard = await _libraryDbContext.LibraryCards.FirstOrDefaultAsync(c => c.CardNumber == cardNumber);
            if (existingCard != null)
            {
                return await GenerateLibraryCard(name);
            }
            return cardNumber;
        }
    }
}
