using EduTrack.Models;
using EduTrack.Services;
using Microsoft.AspNetCore.Mvc;

namespace EduTrack.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly ILogger<CourseController> _logger;
        private const int PageSize = 5;

        public CourseController(ICourseService courseService, ILogger<CourseController> logger)
        {
            _courseService = courseService;
            _logger = logger;
        }

        // GET: /Course
        public async Task<IActionResult> Index(string? searchTerm, string sortOrder = "date_desc", int pageIndex = 1)
        {
            try
            {
                ViewData["CurrentFilter"] = searchTerm;
                ViewData["CurrentSort"] = sortOrder;
                ViewData["NameSort"] = sortOrder == "name_asc" ? "name_desc" : "name_asc";
                ViewData["FeesSort"] = sortOrder == "fees_asc" ? "fees_desc" : "fees_asc";
                ViewData["DurationSort"] = sortOrder == "duration_asc" ? "duration_desc" : "duration_asc";

                var courses = await _courseService.GetCoursesAsync(searchTerm, sortOrder, pageIndex, PageSize);
                return View(courses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving course list.");
                TempData["ErrorMessage"] = "Something went wrong while loading courses.";
                return View("Error");
            }
        }

        // GET: /Course/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var course = await _courseService.GetWithEnrollmentsAsync(id);
            if (course == null) return NotFound();
            return View(course);
        }

        // GET: /Course/Create
        public IActionResult Create() => View(new Course());

        // POST: /Course/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course course)
        {
            if (!ModelState.IsValid)
            {
                return View(course);
            }

            try
            {
                var (success, message) = await _courseService.CreateAsync(course);
                TempData["SuccessMessage"] = message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating course.");
                ModelState.AddModelError(string.Empty, "An unexpected error occurred while saving the course.");
                return View(course);
            }
        }

        // GET: /Course/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var course = await _courseService.GetByIdAsync(id);
            if (course == null) return NotFound();
            return View(course);
        }

        // POST: /Course/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Course course)
        {
            if (id != course.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                return View(course);
            }

            try
            {
                var (success, message) = await _courseService.UpdateAsync(course);
                TempData["SuccessMessage"] = message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating course {CourseId}.", id);
                ModelState.AddModelError(string.Empty, "An unexpected error occurred while updating the course.");
                return View(course);
            }
        }

        // GET: /Course/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _courseService.GetByIdAsync(id);
            if (course == null) return NotFound();
            return View(course);
        }

        // POST: /Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var (success, message) = await _courseService.DeleteAsync(id);
            TempData[success ? "SuccessMessage" : "ErrorMessage"] = message;
            return RedirectToAction(nameof(Index));
        }
    }
}
