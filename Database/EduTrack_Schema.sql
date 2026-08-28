/* =========================================================================
   EduTrack - Student and Course Management System
   Database Creation Script (SQL Server)
   -------------------------------------------------------------------------
   NOTE: If you run the application with EF Core Migrations, this script is
   NOT required - `dotnet ef database update` will create the same schema.
   This script is provided for manual setup or reference purposes.
   ========================================================================= */

IF DB_ID('EduTrackDb') IS NULL
BEGIN
    CREATE DATABASE EduTrackDb;
END
GO

USE EduTrackDb;
GO

/* ---------------------------------------------------------------------
   Table: Students
   --------------------------------------------------------------------- */
IF OBJECT_ID('dbo.Students', 'U') IS NOT NULL DROP TABLE dbo.Students;
GO
CREATE TABLE dbo.Students
(
    Id            INT IDENTITY(1,1) PRIMARY KEY,
    FullName      NVARCHAR(100) NOT NULL,
    Email         NVARCHAR(150) NOT NULL,
    Phone         NVARCHAR(20)  NOT NULL,
    DateOfBirth   DATETIME2     NOT NULL,
    Gender        NVARCHAR(20)  NOT NULL,
    Address       NVARCHAR(250) NOT NULL,
    CreatedDate   DATETIME2     NOT NULL DEFAULT (GETDATE())
);
GO

CREATE UNIQUE INDEX IX_Students_Email ON dbo.Students(Email);
GO

/* ---------------------------------------------------------------------
   Table: Courses
   --------------------------------------------------------------------- */
IF OBJECT_ID('dbo.Courses', 'U') IS NOT NULL DROP TABLE dbo.Courses;
GO
CREATE TABLE dbo.Courses
(
    Id             INT IDENTITY(1,1) PRIMARY KEY,
    CourseName     NVARCHAR(150)  NOT NULL,
    Description    NVARCHAR(1000) NOT NULL,
    Duration       NVARCHAR(50)   NOT NULL,
    Fees           DECIMAL(10,2)  NOT NULL,
    InstructorName NVARCHAR(100)  NOT NULL,
    CreatedDate    DATETIME2      NOT NULL DEFAULT (GETDATE())
);
GO

CREATE INDEX IX_Courses_CourseName ON dbo.Courses(CourseName);
GO

/* ---------------------------------------------------------------------
   Table: Enrollments (junction table: Many Students <-> Many Courses)
   --------------------------------------------------------------------- */
IF OBJECT_ID('dbo.Enrollments', 'U') IS NOT NULL DROP TABLE dbo.Enrollments;
GO
CREATE TABLE dbo.Enrollments
(
    Id             INT IDENTITY(1,1) PRIMARY KEY,
    StudentId      INT NOT NULL,
    CourseId       INT NOT NULL,
    EnrollmentDate DATETIME2    NOT NULL DEFAULT (GETDATE()),
    Status         NVARCHAR(20) NOT NULL DEFAULT ('Active'),

    CONSTRAINT FK_Enrollments_Students FOREIGN KEY (StudentId)
        REFERENCES dbo.Students(Id) ON DELETE CASCADE,

    CONSTRAINT FK_Enrollments_Courses FOREIGN KEY (CourseId)
        REFERENCES dbo.Courses(Id) ON DELETE CASCADE
);
GO

-- Prevent the same student from being enrolled twice in the same course
CREATE UNIQUE INDEX IX_Enrollments_Student_Course ON dbo.Enrollments(StudentId, CourseId);
GO

/* ---------------------------------------------------------------------
   Seed Data
   --------------------------------------------------------------------- */
INSERT INTO dbo.Students (FullName, Email, Phone, DateOfBirth, Gender, Address, CreatedDate) VALUES
('Aarav Sharma',  'aarav.sharma@example.com',  '9876543210', '2001-05-12', 'Male',   'Raipur, Chhattisgarh',   DATEADD(DAY, -20, GETDATE())),
('Priya Verma',   'priya.verma@example.com',   '9876543211', '2002-08-23', 'Female', 'Bhilai, Chhattisgarh',   DATEADD(DAY, -18, GETDATE())),
('Rohan Mehta',   'rohan.mehta@example.com',   '9876543212', '2000-01-05', 'Male',   'Nagpur, Maharashtra',    DATEADD(DAY, -15, GETDATE())),
('Sneha Iyer',    'sneha.iyer@example.com',    '9876543213', '2001-11-30', 'Female', 'Chennai, Tamil Nadu',    DATEADD(DAY, -10, GETDATE())),
('Karan Patel',   'karan.patel@example.com',   '9876543214', '1999-03-17', 'Male',   'Ahmedabad, Gujarat',     DATEADD(DAY, -5,  GETDATE())),
('Ananya Singh',  'ananya.singh@example.com',  '9876543215', '2002-06-09', 'Female', 'Lucknow, Uttar Pradesh', DATEADD(DAY, -2,  GETDATE()));
GO

INSERT INTO dbo.Courses (CourseName, Description, Duration, Fees, InstructorName, CreatedDate) VALUES
('Full Stack .NET Development', 'End-to-end web development using ASP.NET Core MVC, EF Core and SQL Server.', '6 Months', 25000.00, 'Dr. Vikram Rao',   DATEADD(DAY, -30, GETDATE())),
('Data Structures & Algorithms', 'Core CS fundamentals for technical interviews and problem solving.',        '3 Months', 15000.00, 'Prof. Meena Nair', DATEADD(DAY, -25, GETDATE())),
('Cloud Computing with Azure',   'Deploying and scaling applications on Microsoft Azure.',                    '4 Months', 20000.00, 'Er. Suresh Kumar', DATEADD(DAY, -12, GETDATE())),
('React & Modern JavaScript',    'Building interactive front-end applications with React.',                   '3 Months', 18000.00, 'Ms. Divya Kapoor',  DATEADD(DAY, -6,  GETDATE()));
GO

INSERT INTO dbo.Enrollments (StudentId, CourseId, EnrollmentDate, Status) VALUES
(1, 1, DATEADD(DAY, -19, GETDATE()), 'Active'),
(2, 1, DATEADD(DAY, -17, GETDATE()), 'Active'),
(3, 2, DATEADD(DAY, -14, GETDATE()), 'Completed'),
(4, 3, DATEADD(DAY, -9,  GETDATE()), 'Active'),
(5, 1, DATEADD(DAY, -4,  GETDATE()), 'Active'),
(6, 4, DATEADD(DAY, -1,  GETDATE()), 'Active'),
(1, 4, GETDATE(), 'Active');
GO

PRINT 'EduTrackDb schema created and seeded successfully.';
