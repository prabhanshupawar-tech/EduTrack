using System.Linq.Expressions;

namespace EduTrack.Repositories
{
    /// <summary>
    /// Generic repository contract shared by all entity-specific repositories.
    /// Keeps basic CRUD + querying consistent across Student, Course and Enrollment.
    /// </summary>
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        IQueryable<T> GetAll();
        IQueryable<T> Find(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
        Task<bool> SaveChangesAsync();
    }
}
