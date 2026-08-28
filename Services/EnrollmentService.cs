using EduTrack.Models;
using EduTrack.Repositories;
using EduTrack.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EduTrack.Services
{
    public interface IEnrollmentService
    {
        Task<PaginatedList<Enrollment>> GetEnrollmentsAsync(string? searchTerm, string sortOrder, int pageIndex, int pageSize);
        Task<Enrollment?> GetByIdAsync(int id);
        Task<Enrollment?> GetWithDetailsAsync(int id);
        Task<(bool Success, string Message)> CreateAsync(Enrollment enrollment);
        Task<(bool Success, string Message)> UpdateAsync(Enrollment enrollment);
        Task<(bool Success, string Message)> DeleteAsync(int id);
        Task<int> GetTotalCountAsync();
        Task<List<Enrollment>> GetByStudentAsync(int studentId);
        Task<List<Enrollment>> GetByCourseAsync(int courseId);
    }

    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _repository;

        public EnrollmentService(IEnrollmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedList<Enrollment>> GetEnrollmentsAsync(string? searchTerm, string sortOrder, int pageIndex, int pageSize)
        {
            var query = _repository.GetWithDetails();

            // ---- LINQ Search by Student name or Course name ----
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim();
                query = query.Where(e =>
                    e.Student!.FullName.Contains(searchTerm) ||
                    e.Course!.CourseName.Contains(searchTerm) ||
                    e.Status.Contains(searchTerm));
            }

            query = sortOrder switch
            {
                "date_asc" => query.OrderBy(e => e.EnrollmentDate),
                "student_asc" => query.OrderBy(e => e.Student!.FullName),
                "course_asc" => query.OrderBy(e => e.Course!.CourseName),
                _ => query.OrderByDescending(e => e.EnrollmentDate)
            };

            return await PaginatedList<Enrollment>.CreateAsync(query, pageIndex, pageSize);
        }

        public Task<Enrollment?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task<Enrollment?> GetWithDetailsAsync(int id) =>
            _repository.GetWithDetails().FirstOrDefaultAsync(e => e.Id == id);

        public async Task<(bool Success, string Message)> CreateAsync(Enrollment enrollment)
        {
            if (await _repository.IsAlreadyEnrolledAsync(enrollment.StudentId, enrollment.CourseId))
            {
                return (false, "This student is already enrolled in the selected course.");
            }

            await _repository.AddAsync(enrollment);
            await _repository.SaveChangesAsync();
            return (true, "Student enrolled successfully.");
        }

        public async Task<(bool Success, string Message)> UpdateAsync(Enrollment enrollment)
        {
            if (await _repository.IsAlreadyEnrolledAsync(enrollment.StudentId, enrollment.CourseId, enrollment.Id))
            {
                return (false, "This student is already enrolled in the selected course.");
            }

            _repository.Update(enrollment);
            await _repository.SaveChangesAsync();
            return (true, "Enrollment updated successfully.");
        }

        public async Task<(bool Success, string Message)> DeleteAsync(int id)
        {
            var enrollment = await _repository.GetByIdAsync(id);
            if (enrollment == null)
            {
                return (false, "Enrollment not found.");
            }

            _repository.Remove(enrollment);
            await _repository.SaveChangesAsync();
            return (true, "Enrollment removed successfully.");
        }

        public Task<int> GetTotalCountAsync() => _repository.GetAll().CountAsync();

        // ---- LINQ: Students enrolled in a specific course ----
        public async Task<List<Enrollment>> GetByCourseAsync(int courseId) =>
            await _repository.GetWithDetails().Where(e => e.CourseId == courseId).ToListAsync();

        public async Task<List<Enrollment>> GetByStudentAsync(int studentId) =>
            await _repository.GetWithDetails().Where(e => e.StudentId == studentId).ToListAsync();
    }
}
