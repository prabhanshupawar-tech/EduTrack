using System.ComponentModel.DataAnnotations;

namespace EduTrack.Models
{
    /// <summary>
    /// Represents a Student entity in the EduTrack system.
    /// A Student can have many Enrollments (1 -> Many relationship).
    /// </summary>
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Full name must be between 3 and 100 characters.")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [StringLength(20)]
        public string Gender { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(250)]
        public string Address { get; set; } = string.Empty;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation property: one student -> many enrollments
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
