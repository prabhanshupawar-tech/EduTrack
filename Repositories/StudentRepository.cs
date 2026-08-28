using EduTrack.Data;
using EduTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace EduTrack.Repositories
{
    public interface IStudentRepository : IGenericRepository<Student>
    {
        Task<bool> EmailExistsAsync(string email, int? excludeId = null);
        IQueryable<Student> GetWithEnrollments();
    }

    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        public StudentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeId = null)
        {
            return await _dbSet.AnyAsync(s => s.Email == email && (excludeId == null || s.Id != excludeId));
        }

        public IQueryable<Student> GetWithEnrollments()
        {
            return _dbSet.Include(s => s.Enrollments).ThenInclude(e => e.Course);
        }
    }
}
