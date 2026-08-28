using EduTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace EduTrack.Data
{
    /// <summary>
    /// EF Core database context for EduTrack. Configures entities, relationships and indexes.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---- Student configuration ----
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasIndex(s => s.Email).IsUnique();
                entity.Property(s => s.FullName).IsRequired();
                entity.Property(s => s.Email).IsRequired();
            });

            // ---- Course configuration ----
            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasIndex(c => c.CourseName);
                entity.Property(c => c.Fees).HasColumnType("decimal(10,2)");
            });

            // ---- Enrollment configuration (Fluent API relationships) ----
            modelBuilder.Entity<Enrollment>(entity =>
            {
                // Many Enrollments -> One Student
                entity.HasOne(e => e.Student)
                      .WithMany(s => s.Enrollments)
                      .HasForeignKey(e => e.StudentId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Many Enrollments -> One Course
                entity.HasOne(e => e.Course)
                      .WithMany(c => c.Enrollments)
                      .HasForeignKey(e => e.CourseId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Prevent the exact same student from being enrolled twice in the same course
                entity.HasIndex(e => new { e.StudentId, e.CourseId }).IsUnique();
            });
        }
    }
}
