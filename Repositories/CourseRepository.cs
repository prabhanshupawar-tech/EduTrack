using EduTrack.Data;
using EduTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace EduTrack.Repositories
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
        IQueryable<Course> GetWithEnrollments();
    }

    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        public CourseRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IQueryable<Course> GetWithEnrollments()
        {
            return _dbSet.Include(c => c.Enrollments).ThenInclude(e => e.Student);
        }
    }
}
