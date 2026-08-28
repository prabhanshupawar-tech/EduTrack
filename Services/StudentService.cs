using EduTrack.Models;
using EduTrack.Repositories;
using EduTrack.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EduTrack.Services
{
    public interface IStudentService
    {
        Task<PaginatedList<Student>> GetStudentsAsync(string? searchTerm, string sortOrder, int pageIndex, int pageSize);
        Task<Student?> GetByIdAsync(int id);
        Task<Student?> GetWithEnrollmentsAsync(int id);
        Task<(bool Success, string Message)> CreateAsync(Student student);
        Task<(bool Success, string Message)> UpdateAsync(Student student);
        Task<(bool Success, string Message)> DeleteAsync(int id);
        Task<int> GetTotalCountAsync();
        Task<List<Student>> GetLatestAsync(int count);
    }

    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repository;

        public StudentService(IStudentRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedList<Student>> GetStudentsAsync(string? searchTerm, string sortOrder, int pageIndex, int pageSize)
        {
            var query = _repository.GetAll();

            // ---- LINQ Search by Name, Email or Phone ----
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim();
                query = query.Where(s =>
                    s.FullName.Contains(searchTerm) ||
                    s.Email.Contains(searchTerm) ||
                    s.Phone.Contains(searchTerm));
            }

            // ---- LINQ Sorting ----
            query = sortOrder switch
            {
                "name_desc" => query.OrderByDescending(s => s.FullName),
                "name_asc" => query.OrderBy(s => s.FullName),
                "date_desc" => query.OrderByDescending(s => s.CreatedDate),
                "date_asc" => query.OrderBy(s => s.CreatedDate),
                _ => query.OrderByDescending(s => s.CreatedDate)
            };

            return await PaginatedList<Student>.CreateAsync(query, pageIndex, pageSize);
        }

        public Task<Student?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task<Student?> GetWithEnrollmentsAsync(int id) =>
            _repository.GetWithEnrollments().FirstOrDefaultAsync(s => s.Id == id);

        public async Task<(bool Success, string Message)> CreateAsync(Student student)
        {
            if (await _repository.EmailExistsAsync(student.Email))
            {
                return (false, "A student with this email already exists.");
            }

            student.CreatedDate = DateTime.Now;
            await _repository.AddAsync(student);
            await _repository.SaveChangesAsync();
            return (true, "Student added successfully.");
        }

        public async Task<(bool Success, string Message)> UpdateAsync(Student student)
        {
            if (await _repository.EmailExistsAsync(student.Email, student.Id))
            {
                return (false, "Another student is already using this email.");
            }

            _repository.Update(student);
            await _repository.SaveChangesAsync();
            return (true, "Student updated successfully.");
        }

        public async Task<(bool Success, string Message)> DeleteAsync(int id)
        {
            var student = await _repository.GetByIdAsync(id);
            if (student == null)
            {
                return (false, "Student not found.");
            }

            try
            {
                _repository.Remove(student);
                await _repository.SaveChangesAsync();
                return (true, "Student deleted successfully.");
            }
            catch (Exception)
            {
                return (false, "Unable to delete student. They may have active enrollments.");
            }
        }

        public Task<int> GetTotalCountAsync() => _repository.GetAll().CountAsync();

        public async Task<List<Student>> GetLatestAsync(int count) =>
            await _repository.GetAll().OrderByDescending(s => s.CreatedDate).Take(count).ToListAsync();
    }
}
