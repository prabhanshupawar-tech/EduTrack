using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduTrack.Models
{
    /// <summary>
    /// Represents an Enrollment entity - the junction between Student and Course.
    /// Many Enrollments -> One Student
    /// Many Enrollments -> One Course
    /// </summary>
    public class Enrollment
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please select a student.")]
        [Display(Name = "Student")]
        public int StudentId { get; set; }

        [ForeignKey(nameof(StudentId))]
        public Student? Student { get; set; }

        [Required(ErrorMessage = "Please select a course.")]
        [Display(Name = "Course")]
        public int CourseId { get; set; }

        [ForeignKey(nameof(CourseId))]
        public Course? Course { get; set; }

        [Required(ErrorMessage = "Enrollment date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Status is required.")]
        [StringLength(20)]
        public string Status { get; set; } = "Active"; // Active, Completed, Cancelled
    }
}
