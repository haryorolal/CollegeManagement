using AutoMapper;
using CollegeManagement.Data.IRepository;
using CollegeManagement.Models;
using CollegeManagement.Data.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;

namespace CollegeManagement.Data.Repository
{
    public class CollegeRepository<T> : ICollegeRepository<T> where T : class
    {
        public readonly LibraryDbContext _libraryDbContext;
        public readonly IMapper? _mapper;
        private DbSet<T> _dbSet;
        public CollegeRepository(LibraryDbContext libraryDbContext, IMapper? mapper)
        {
            _libraryDbContext = libraryDbContext;
            _mapper = mapper;
            _dbSet = _libraryDbContext.Set<T>();
        }

        public async Task<List<T>> GetAllAsync(List<string> includes = null)
        {
            IQueryable<T> query = _dbSet;
            if (query.IsNullOrEmpty())
                return null;

            if (includes != null)
            {
                foreach (var item in includes)
                    query = query.Include(item);
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> exp, List<string> includes = null, bool useNoTractking = false)
        {
            IQueryable<T> query = _dbSet;
            if (query.IsNullOrEmpty())
                return null;

            if (exp != null)
                query = query.Where(exp);

            if (includes != null)
            {
                foreach(var item in includes)
                    query = query.Include(item);
            }

            if (useNoTractking)
            {
                Console.WriteLine("Using AsNoTracking"); // Debug line
                return await query.AsNoTracking().FirstOrDefaultAsync();
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAllFilterAsync(Expression<Func<T, bool>> exp = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<string> includes = null, bool useNoTracking = false)
        {
            IQueryable<T> query = _dbSet;
            if (query.IsNullOrEmpty())
                return new List<T>();

            if (exp != null)
                query = query.Where(exp);

            if (includes != null)
            {
                foreach (var item in includes)
                    query = query.Include(item);
            }

            if (orderBy != null)
                query = orderBy(query);

            if (useNoTracking)
                return await query.AsNoTracking().ToListAsync();            

            return await query.ToListAsync();
        }

        public async Task<T> CreateAsync(T dbRecord)
        {
            _dbSet.Add(dbRecord);
            await _libraryDbContext.SaveChangesAsync();
            return dbRecord;
        }

        public async Task<List<T>> CreateRangeAsync(List<T> dbRecord)
        {
            _dbSet.AddRange(dbRecord);
            await _libraryDbContext.SaveChangesAsync();
            return dbRecord;
        }

        public async Task<T> UpdateAsync(T dbRecord)
        {
            //_dbSet.Update(dbRecord);

            var entry = _libraryDbContext.Entry(dbRecord);

            if (entry.State == EntityState.Detached)
            {
                // Attach the entity and mark as modified
                _dbSet.Attach(dbRecord);
                entry.State = EntityState.Modified;
            }
            // If it's already tracked, EF Core will detect changes automatically
            await _libraryDbContext.SaveChangesAsync();
            return dbRecord;
        }

        public async Task<bool> DeleteAsync(T dbRecord)
        {
            _dbSet.Remove(dbRecord);
            await _libraryDbContext.SaveChangesAsync();
            return true;
        }
    }
}
