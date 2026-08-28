using EduTrack.Data;
using EduTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace EduTrack.Repositories
{
    public interface IEnrollmentRepository : IGenericRepository<Enrollment>
    {
        IQueryable<Enrollment> GetWithDetails();
        Task<bool> IsAlreadyEnrolledAsync(int studentId, int courseId, int? excludeId = null);
    }

    public class EnrollmentRepository : GenericRepository<Enrollment>, IEnrollmentRepository
    {
        public EnrollmentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IQueryable<Enrollment> GetWithDetails()
        {
            return _dbSet.Include(e => e.Student).Include(e => e.Course);
        }

        public async Task<bool> IsAlreadyEnrolledAsync(int studentId, int courseId, int? excludeId = null)
        {
            return await _dbSet.AnyAsync(e =>
                e.StudentId == studentId &&
                e.CourseId == courseId &&
                (excludeId == null || e.Id != excludeId));
        }
    }
}
