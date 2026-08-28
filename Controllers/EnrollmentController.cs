using EduTrack.Models;
using EduTrack.Repositories;
using EduTrack.Services;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EduTrack.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly IStudentRepository _studentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ILogger<EnrollmentController> _logger;
        private const int PageSize = 5;

        public EnrollmentController(
            IEnrollmentService enrollmentService,
            IStudentRepository studentRepository,
            ICourseRepository courseRepository,
            ILogger<EnrollmentController> logger)
        {
            _enrollmentService = enrollmentService;
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;
            _logger = logger;
        }

        // GET: /Enrollment
        public async Task<IActionResult> Index(string? searchTerm, string sortOrder = "date_desc", int pageIndex = 1)
        {
            try
            {
                ViewData["CurrentFilter"] = searchTerm;
                ViewData["CurrentSort"] = sortOrder;

                var enrollments = await _enrollmentService.GetEnrollmentsAsync(searchTerm, sortOrder, pageIndex, PageSize);
                return View(enrollments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving enrollment list.");
                TempData["ErrorMessage"] = "Something went wrong while loading enrollments.";
                return View("Error");
            }
        }

        // GET: /Enrollment/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var enrollment = await _enrollmentService.GetWithDetailsAsync(id);
            if (enrollment == null) return NotFound();
            return View(enrollment);
        }

        // GET: /Enrollment/Create
        public async Task<IActionResult> Create()
        {
            var vm = new EnrollmentFormViewModel
            {
                Enrollment = new Enrollment { EnrollmentDate = DateTime.Now },
                Students = await GetStudentSelectListAsync(),
                Courses = await GetCourseSelectListAsync()
            };
            return View(vm);
        }

        // POST: /Enrollment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EnrollmentFormViewModel vm)
        {
            ModelState.Remove("Enrollment.Student");
            ModelState.Remove("Enrollment.Course");

            if (!ModelState.IsValid)
            {
                vm.Students = await GetStudentSelectListAsync();
                vm.Courses = await GetCourseSelectListAsync();
                return View(vm);
            }

            var (success, message) = await _enrollmentService.CreateAsync(vm.Enrollment);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, message);
                vm.Students = await GetStudentSelectListAsync();
                vm.Courses = await GetCourseSelectListAsync();
                return View(vm);
            }

            TempData["SuccessMessage"] = message;
            return RedirectToAction(nameof(Index));
        }

        // GET: /Enrollment/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var enrollment = await _enrollmentService.GetByIdAsync(id);
            if (enrollment == null) return NotFound();

            var vm = new EnrollmentFormViewModel
            {
                Enrollment = enrollment,
                Students = await GetStudentSelectListAsync(),
                Courses = await GetCourseSelectListAsync()
            };
            return View(vm);
        }

        // POST: /Enrollment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EnrollmentFormViewModel vm)
        {
            if (id != vm.Enrollment.Id) return BadRequest();

            ModelState.Remove("Enrollment.Student");
            ModelState.Remove("Enrollment.Course");

            if (!ModelState.IsValid)
            {
                vm.Students = await GetStudentSelectListAsync();
                vm.Courses = await GetCourseSelectListAsync();
                return View(vm);
            }

            var (success, message) = await _enrollmentService.UpdateAsync(vm.Enrollment);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, message);
                vm.Students = await GetStudentSelectListAsync();
                vm.Courses = await GetCourseSelectListAsync();
                return View(vm);
            }

            TempData["SuccessMessage"] = message;
            return RedirectToAction(nameof(Index));
        }

        // GET: /Enrollment/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var enrollment = await _enrollmentService.GetWithDetailsAsync(id);
            if (enrollment == null) return NotFound();
            return View(enrollment);
        }

        // POST: /Enrollment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var (success, message) = await _enrollmentService.DeleteAsync(id);
            TempData[success ? "SuccessMessage" : "ErrorMessage"] = message;
            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<SelectListItem>> GetStudentSelectListAsync()
        {
            return await _studentRepository.GetAll()
                .OrderBy(s => s.FullName)
                .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.FullName + " (" + s.Email + ")" })
                .ToListAsync();
        }

        private async Task<IEnumerable<SelectListItem>> GetCourseSelectListAsync()
        {
            return await _courseRepository.GetAll()
                .OrderBy(c => c.CourseName)
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.CourseName })
                .ToListAsync();
        }
    }
}
