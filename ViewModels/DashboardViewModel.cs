using EduTrack.Models;

namespace EduTrack.ViewModels
{
    /// <summary>
    /// Aggregated data shown on the Home Dashboard.
    /// </summary>
    public class DashboardViewModel
    {
        public int TotalStudents { get; set; }
        public int TotalCourses { get; set; }
        public int TotalEnrollments { get; set; }
        public List<Student> LatestStudents { get; set; } = new();
        public List<Course> LatestCourses { get; set; } = new();
        public List<CoursePopularityViewModel> TopCourses { get; set; } = new();
    }

    /// <summary>
    /// Used for the "Courses with most students" LINQ query result.
    /// </summary>
    public class CoursePopularityViewModel
    {
        public string CourseName { get; set; } = string.Empty;
        public int StudentCount { get; set; }
    }
}
