using EduTrack.Models;
using EduTrack.Repositories;
using EduTrack.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EduTrack.Services
{
    public interface ICourseService
    {
        Task<PaginatedList<Course>> GetCoursesAsync(string? searchTerm, string sortOrder, int pageIndex, int pageSize);
        Task<Course?> GetByIdAsync(int id);
        Task<Course?> GetWithEnrollmentsAsync(int id);
        Task<(bool Success, string Message)> CreateAsync(Course course);
        Task<(bool Success, string Message)> UpdateAsync(Course course);
        Task<(bool Success, string Message)> DeleteAsync(int id);
        Task<int> GetTotalCountAsync();
        Task<List<Course>> GetLatestAsync(int count);
        Task<List<CoursePopularityViewModel>> GetTopCoursesAsync(int count);
    }

    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _repository;

        public CourseService(ICourseRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedList<Course>> GetCoursesAsync(string? searchTerm, string sortOrder, int pageIndex, int pageSize)
        {
            var query = _repository.GetAll();

            // ---- LINQ Search by Course Name or Instructor ----
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim();
                query = query.Where(c =>
                    c.CourseName.Contains(searchTerm) ||
                    c.InstructorName.Contains(searchTerm));
            }

            // ---- LINQ Sorting ----
            query = sortOrder switch
            {
                "name_desc" => query.OrderByDescending(c => c.CourseName),
                "name_asc" => query.OrderBy(c => c.CourseName),
                "fees_desc" => query.OrderByDescending(c => c.Fees),
                "fees_asc" => query.OrderBy(c => c.Fees),
                "duration_asc" => query.OrderBy(c => c.Duration),
                "duration_desc" => query.OrderByDescending(c => c.Duration),
                "date_asc" => query.OrderBy(c => c.CreatedDate),
                _ => query.OrderByDescending(c => c.CreatedDate)
            };

            return await PaginatedList<Course>.CreateAsync(query, pageIndex, pageSize);
        }

        public Task<Course?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task<Course?> GetWithEnrollmentsAsync(int id) =>
            _repository.GetWithEnrollments().FirstOrDefaultAsync(c => c.Id == id);

        public async Task<(bool Success, string Message)> CreateAsync(Course course)
        {
            course.CreatedDate = DateTime.Now;
            await _repository.AddAsync(course);
            await _repository.SaveChangesAsync();
            return (true, "Course created successfully.");
        }

        public async Task<(bool Success, string Message)> UpdateAsync(Course course)
        {
            _repository.Update(course);
            await _repository.SaveChangesAsync();
            return (true, "Course updated successfully.");
        }

        public async Task<(bool Success, string Message)> DeleteAsync(int id)
        {
            var course = await _repository.GetByIdAsync(id);
            if (course == null)
            {
                return (false, "Course not found.");
            }

            try
            {
                _repository.Remove(course);
                await _repository.SaveChangesAsync();
                return (true, "Course deleted successfully.");
            }
            catch (Exception)
            {
                return (false, "Unable to delete course. It may have active enrollments.");
            }
        }

        public Task<int> GetTotalCountAsync() => _repository.GetAll().CountAsync();

        public async Task<List<Course>> GetLatestAsync(int count) =>
            await _repository.GetAll().OrderByDescending(c => c.CreatedDate).Take(count).ToListAsync();

        // ---- LINQ: Courses with the most enrolled students ----
        public async Task<List<CoursePopularityViewModel>> GetTopCoursesAsync(int count)
        {
            return await _repository.GetWithEnrollments()
                .Select(c => new CoursePopularityViewModel
                {
                    CourseName = c.CourseName,
                    StudentCount = c.Enrollments.Count
                })
                .OrderByDescending(c => c.StudentCount)
                .Take(count)
                .ToListAsync();
        }
    }
}
