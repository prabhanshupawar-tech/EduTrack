# EduTrack – Student and Course Management System

A production-quality, interview-ready **ASP.NET Core MVC (.NET 8)** web application for managing students, courses, and enrollments — built with Entity Framework Core, SQL Server, the Repository + Service pattern, and a clean Bootstrap 5 UI.

---

## 1. Project Overview

EduTrack is a web-based admin system where an administrator can:

- Manage a **Student** roster (CRUD)
- Manage a **Course** catalog (CRUD)
- **Enroll** students into courses and track enrollment status
- View a **Dashboard** with live counts, recent activity, and top courses

The project is intentionally structured to demonstrate real-world ASP.NET Core MVC architecture: layered Controllers → Services → Repositories → EF Core, with LINQ-driven search, sorting, and pagination throughout.

---

## 2. Features

- **Dashboard**: total students / courses / enrollments, latest students, latest courses, courses with the most enrolled students
- **Student module**: Add / Edit / Delete / Details / List, with unique-email validation
- **Course module**: Add / Edit / Delete / Details / List
- **Enrollment module**: Enroll / Edit / Delete / List, prevents duplicate enrollment of the same student in the same course
- **Search**: students by name/email/phone, courses by name/instructor, enrollments by student/course/status
- **Sorting**: by name, date, fees, duration (ascending/descending)
- **Pagination**: on all three list pages
- **Validation**: Data Annotations + client-side unobtrusive validation, with server-side re-validation
- **Bonus UI**: SweetAlert2 delete-confirmation dialogs, toast notifications for success/error, Bootstrap Icons, a global loading spinner, and fully responsive tables

---

## 3. Technology Stack

| Layer      | Technology |
|------------|------------|
| Frontend   | ASP.NET Core MVC, Razor Views, Bootstrap 5, HTML5, CSS3, JavaScript, jQuery |
| Backend    | ASP.NET Core MVC (.NET 8), C#, Entity Framework Core, LINQ, Dependency Injection, Repository + Service pattern |
| Database   | SQL Server, EF Core Migrations |
| Bonus      | SweetAlert2, Bootstrap Icons |
| IDE        | Visual Studio 2022 (v17.8+) |

---

## 4. Folder Structure

```
EduTrack/
├── Controllers/          # HomeController, StudentController, CourseController, EnrollmentController
├── Models/                # Student, Course, Enrollment (entities)
├── ViewModels/             # DashboardViewModel, PaginatedList<T>, EnrollmentFormViewModel
├── Data/                   # ApplicationDbContext, SeedData
├── Repositories/           # Generic + entity-specific repositories (IStudentRepository, etc.)
├── Services/                # Business logic layer (IStudentService, ICourseService, IEnrollmentService, IDashboardService)
├── Views/
│   ├── Home/                # Dashboard
│   ├── Student/              # Index, Create, Edit, Details, Delete
│   ├── Course/                # Index, Create, Edit, Details, Delete
│   ├── Enrollment/             # Index, Create, Edit, Details, Delete
│   └── Shared/                  # _Layout, _Pagination, _Notifications, Error
├── Migrations/                    # EF Core InitialCreate migration + model snapshot
├── Database/                       # Standalone EduTrack_Schema.sql (manual DB setup / reference)
├── wwwroot/                         # site.css, site.js
├── Program.cs                       # DI container, middleware pipeline
├── appsettings.json                 # Connection string, logging
└── EduTrack.csproj
```

---

## 5. Prerequisites

- Visual Studio 2022 (17.8 or later) with the **ASP.NET and web development** workload
- .NET 8 SDK
- SQL Server (LocalDB, which ships with Visual Studio, works out of the box) or a full SQL Server instance

---

## 6. Installation & Setup

1. **Extract** the project and open `EduTrack.csproj` (or the folder) in Visual Studio 2022.
2. Visual Studio will restore NuGet packages automatically. If not, run:
   ```
   dotnet restore
   ```
3. **Configure the connection string** in `appsettings.json` if you're not using LocalDB:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=EduTrackDb;Trusted_Connection=True;TrustServerCertificate=True"
   }
   ```

---

## 7. Database Setup & Migration Commands

The app automatically applies pending migrations and seeds sample data on startup (see `SeedData.Initialize()` in `Program.cs`), so in most cases **you can just press F5** and the database will be created for you.

If you'd rather manage it manually, use the **Package Manager Console** in Visual Studio (Tools → NuGet Package Manager → Package Manager Console):

```powershell
# Apply the existing migration and create the database
Update-Database

# If you change the models and want to add a new migration
Add-Migration <MigrationName>
Update-Database
```

Or via the .NET CLI (from the project folder):

```bash
dotnet tool install --global dotnet-ef   # first time only
dotnet ef database update
```

Alternatively, run the plain SQL script directly against SQL Server:

```
Database/EduTrack_Schema.sql
```

This script creates the database, tables, foreign keys, indexes, and seed data without needing EF tooling — useful for quick manual setup or DBA review.

---

## 8. Running the Project

- Press **F5** (or **Ctrl+F5**) in Visual Studio to build and launch with IIS Express / Kestrel.
- The app opens at `https://localhost:7050` and redirects to the **Dashboard**.
- Sample data (6 students, 4 courses, 7 enrollments) is seeded automatically on first run.

---

## 9. Screenshots

> _Add screenshots here after running the app locally:_
- `docs/screenshots/dashboard.png`
- `docs/screenshots/students-list.png`
- `docs/screenshots/course-form.png`
- `docs/screenshots/enrollment-list.png`

---

## 10. Key Architectural Notes

- **Repository Pattern**: `IGenericRepository<T>` + entity-specific repositories (`IStudentRepository`, `ICourseRepository`, `IEnrollmentRepository`) isolate EF Core from the rest of the app.
- **Service Layer**: business rules (duplicate-email checks, duplicate-enrollment checks, search/sort/pagination composition) live in `Services/`, keeping controllers thin.
- **Dependency Injection**: `DbContext`, repositories, and services are all registered as `Scoped` in `Program.cs` and injected via constructors.
- **LINQ usage**: search filters, multi-column sorting, "students enrolled in a course," and "courses with the most students" are all implemented with LINQ over `IQueryable<T>` so filtering/sorting/paging is translated to SQL and executed server-side.
- **Validation**: Data Annotations (`[Required]`, `[StringLength]`, `[EmailAddress]`, `[Phone]`, `[Range]`) drive both client-side (jQuery Validation) and server-side (`ModelState.IsValid`) checks.
- **Error Handling**: controllers wrap data access in `try/catch`, log via `ILogger<T>`, and surface friendly messages through `TempData` + a shared `Error.cshtml` view; the pipeline also registers `UseExceptionHandler("/Home/Error")` for unhandled exceptions in production.

---

## 11. Future Improvements

- Add ASP.NET Core Identity for authentication/authorization (admin login, role-based access)
- Add AutoMapper for DTO/entity mapping
- Add automated unit tests (xUnit) for the Service layer using an in-memory EF Core provider
- Add file upload support for student photos / course thumbnails
- Add export-to-Excel/PDF for reports
- Add API endpoints (Web API) alongside MVC for a future SPA/mobile client
- Add Docker support for containerized deployment

---

## 12. License

This project was generated as a learning/interview-preparation reference. Use and modify freely.
