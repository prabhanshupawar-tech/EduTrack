using EduTrack.ViewModels;

namespace EduTrack.Services
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> GetDashboardDataAsync();
    }

    /// <summary>
    /// Aggregates data from Student, Course and Enrollment services for the Home dashboard.
    /// </summary>
    public class DashboardService : IDashboardService
    {
        private readonly IStudentService _studentService;
        private readonly ICourseService _courseService;
        private readonly IEnrollmentService _enrollmentService;

        public DashboardService(IStudentService studentService, ICourseService courseService, IEnrollmentService enrollmentService)
        {
            _studentService = studentService;
            _courseService = courseService;
            _enrollmentService = enrollmentService;
        }

        public async Task<DashboardViewModel> GetDashboardDataAsync()
        {
            var vm = new DashboardViewModel
            {
                TotalStudents = await _studentService.GetTotalCountAsync(),
                TotalCourses = await _courseService.GetTotalCountAsync(),
                TotalEnrollments = await _enrollmentService.GetTotalCountAsync(),
                LatestStudents = await _studentService.GetLatestAsync(5),
                LatestCourses = await _courseService.GetLatestAsync(5),
                TopCourses = await _courseService.GetTopCoursesAsync(5)
            };

            return vm;
        }
    }
}
