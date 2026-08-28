using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduTrack.Models
{
    /// <summary>
    /// Represents a Course entity in the EduTrack system.
    /// A Course can have many Enrollments (1 -> Many relationship).
    /// </summary>
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Course name is required.")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Course name must be between 3 and 150 characters.")]
        [Display(Name = "Course Name")]
        public string CourseName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Duration is required.")]
        [StringLength(50)]
        [Display(Name = "Duration")]
        public string Duration { get; set; } = string.Empty;

        [Required(ErrorMessage = "Fees are required.")]
        [Range(0, 1000000, ErrorMessage = "Fees must be a positive value.")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Fees { get; set; }

        [Required(ErrorMessage = "Instructor name is required.")]
        [StringLength(100)]
        [Display(Name = "Instructor Name")]
        public string InstructorName { get; set; } = string.Empty;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation property: one course -> many enrollments
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
