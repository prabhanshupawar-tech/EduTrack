using EduTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace EduTrack.Data
{
    /// <summary>
    /// Seeds the database with sample data on first run so the app is demo-ready immediately.
    /// Called from Program.cs during startup.
    /// </summary>
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.Migrate();

            if (context.Students.Any() || context.Courses.Any())
            {
                return; // Already seeded
            }

            var students = new List<Student>
            {
                new Student { FullName = "Aarav Sharma", Email = "aarav.sharma@example.com", Phone = "9876543210", DateOfBirth = new DateTime(2001, 5, 12), Gender = "Male", Address = "Raipur, Chhattisgarh", CreatedDate = DateTime.Now.AddDays(-20) },
                new Student { FullName = "Priya Verma", Email = "priya.verma@example.com", Phone = "9876543211", DateOfBirth = new DateTime(2002, 8, 23), Gender = "Female", Address = "Bhilai, Chhattisgarh", CreatedDate = DateTime.Now.AddDays(-18) },
                new Student { FullName = "Rohan Mehta", Email = "rohan.mehta@example.com", Phone = "9876543212", DateOfBirth = new DateTime(2000, 1, 5), Gender = "Male", Address = "Nagpur, Maharashtra", CreatedDate = DateTime.Now.AddDays(-15) },
                new Student { FullName = "Sneha Iyer", Email = "sneha.iyer@example.com", Phone = "9876543213", DateOfBirth = new DateTime(2001, 11, 30), Gender = "Female", Address = "Chennai, Tamil Nadu", CreatedDate = DateTime.Now.AddDays(-10) },
                new Student { FullName = "Karan Patel", Email = "karan.patel@example.com", Phone = "9876543214", DateOfBirth = new DateTime(1999, 3, 17), Gender = "Male", Address = "Ahmedabad, Gujarat", CreatedDate = DateTime.Now.AddDays(-5) },
                new Student { FullName = "Ananya Singh", Email = "ananya.singh@example.com", Phone = "9876543215", DateOfBirth = new DateTime(2002, 6, 9), Gender = "Female", Address = "Lucknow, Uttar Pradesh", CreatedDate = DateTime.Now.AddDays(-2) },
            };

            var courses = new List<Course>
            {
                new Course { CourseName = "Full Stack .NET Development", Description = "End-to-end web development using ASP.NET Core MVC, EF Core and SQL Server.", Duration = "6 Months", Fees = 25000, InstructorName = "Dr. Vikram Rao", CreatedDate = DateTime.Now.AddDays(-30) },
                new Course { CourseName = "Data Structures & Algorithms", Description = "Core CS fundamentals for technical interviews and problem solving.", Duration = "3 Months", Fees = 15000, InstructorName = "Prof. Meena Nair", CreatedDate = DateTime.Now.AddDays(-25) },
                new Course { CourseName = "Cloud Computing with Azure", Description = "Deploying and scaling applications on Microsoft Azure.", Duration = "4 Months", Fees = 20000, InstructorName = "Er. Suresh Kumar", CreatedDate = DateTime.Now.AddDays(-12) },
                new Course { CourseName = "React & Modern JavaScript", Description = "Building interactive front-end applications with React.", Duration = "3 Months", Fees = 18000, InstructorName = "Ms. Divya Kapoor", CreatedDate = DateTime.Now.AddDays(-6) },
            };

            context.Students.AddRange(students);
            context.Courses.AddRange(courses);
            context.SaveChanges();

            var enrollments = new List<Enrollment>
            {
                new Enrollment { StudentId = students[0].Id, CourseId = courses[0].Id, EnrollmentDate = DateTime.Now.AddDays(-19), Status = "Active" },
                new Enrollment { StudentId = students[1].Id, CourseId = courses[0].Id, EnrollmentDate = DateTime.Now.AddDays(-17), Status = "Active" },
                new Enrollment { StudentId = students[2].Id, CourseId = courses[1].Id, EnrollmentDate = DateTime.Now.AddDays(-14), Status = "Completed" },
                new Enrollment { StudentId = students[3].Id, CourseId = courses[2].Id, EnrollmentDate = DateTime.Now.AddDays(-9), Status = "Active" },
                new Enrollment { StudentId = students[4].Id, CourseId = courses[0].Id, EnrollmentDate = DateTime.Now.AddDays(-4), Status = "Active" },
                new Enrollment { StudentId = students[5].Id, CourseId = courses[3].Id, EnrollmentDate = DateTime.Now.AddDays(-1), Status = "Active" },
                new Enrollment { StudentId = students[0].Id, CourseId = courses[3].Id, EnrollmentDate = DateTime.Now, Status = "Active" },
            };

            context.Enrollments.AddRange(enrollments);
            context.SaveChanges();
        }
    }
}
