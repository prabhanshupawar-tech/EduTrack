using EduTrack.Models;
using EduTrack.Services;
using Microsoft.AspNetCore.Mvc;

namespace EduTrack.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly ILogger<StudentController> _logger;
        private const int PageSize = 5;

        public StudentController(IStudentService studentService, ILogger<StudentController> logger)
        {
            _studentService = studentService;
            _logger = logger;
        }

        // GET: /Student
        public async Task<IActionResult> Index(string? searchTerm, string sortOrder = "date_desc", int pageIndex = 1)
        {
            try
            {
                ViewData["CurrentFilter"] = searchTerm;
                ViewData["CurrentSort"] = sortOrder;
                ViewData["NameSort"] = sortOrder == "name_asc" ? "name_desc" : "name_asc";
                ViewData["DateSort"] = sortOrder == "date_asc" ? "date_desc" : "date_asc";

                var students = await _studentService.GetStudentsAsync(searchTerm, sortOrder, pageIndex, PageSize);
                return View(students);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving student list.");
                TempData["ErrorMessage"] = "Something went wrong while loading students.";
                return View("Error");
            }
        }

        // GET: /Student/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var student = await _studentService.GetWithEnrollmentsAsync(id);
            if (student == null) return NotFound();
            return View(student);
        }

        // GET: /Student/Create
        public IActionResult Create() => View(new Student());

        // POST: /Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            if (!ModelState.IsValid)
            {
                return View(student);
            }

            try
            {
                var (success, message) = await _studentService.CreateAsync(student);
                if (!success)
                {
                    ModelState.AddModelError(string.Empty, message);
                    return View(student);
                }

                TempData["SuccessMessage"] = message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating student.");
                ModelState.AddModelError(string.Empty, "An unexpected error occurred while saving the student.");
                return View(student);
            }
        }

        // GET: /Student/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var student = await _studentService.GetByIdAsync(id);
            if (student == null) return NotFound();
            return View(student);
        }

        // POST: /Student/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student student)
        {
            if (id != student.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                return View(student);
            }

            try
            {
                var (success, message) = await _studentService.UpdateAsync(student);
                if (!success)
                {
                    ModelState.AddModelError(string.Empty, message);
                    return View(student);
                }

                TempData["SuccessMessage"] = message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating student {StudentId}.", id);
                ModelState.AddModelError(string.Empty, "An unexpected error occurred while updating the student.");
                return View(student);
            }
        }

        // GET: /Student/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _studentService.GetByIdAsync(id);
            if (student == null) return NotFound();
            return View(student);
        }

        // POST: /Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var (success, message) = await _studentService.DeleteAsync(id);
            TempData[success ? "SuccessMessage" : "ErrorMessage"] = message;
            return RedirectToAction(nameof(Index));
        }
    }
}
