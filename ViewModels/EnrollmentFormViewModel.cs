using EduTrack.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EduTrack.ViewModels
{
    /// <summary>
    /// Backs the Enrollment Create/Edit views, supplying dropdown lists for
    /// Student and Course alongside the underlying Enrollment entity.
    /// </summary>
    public class EnrollmentFormViewModel
    {
        public Enrollment Enrollment { get; set; } = new();
        public IEnumerable<SelectListItem> Students { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Courses { get; set; } = new List<SelectListItem>();
        public List<string> StatusOptions { get; set; } = new() { "Active", "Completed", "Cancelled" };
    }
}
