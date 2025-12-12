using System.Linq.Expressions;

namespace CollegeManagement.Data.IRepository
{
    public interface ICollegeRepository<T>
    {
        Task<List<T>> GetAllAsync(List<string> includes = null);
        Task<List<T>> GetAllFilterAsync(Expression<Func<T, bool>> exp, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<string> includes = null, bool useNoTracking = false);
        Task<T> GetAsync(Expression<Func<T, bool>> exp, List<string> includes = null, bool useNoTracking = false);
        Task<T> CreateAsync(T dbRecord);
        Task<List<T>> CreateRangeAsync(List<T> dbRecords);
        Task<T> UpdateAsync(T dbRecord);
        Task<bool> DeleteAsync(T dbRecord);
    }
}
