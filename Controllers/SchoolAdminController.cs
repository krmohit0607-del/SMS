using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMS.API.Data;
using SMS.API.DTOs;
using SMS.API.DTOs.SchoolAdmin;
using SMS.API.Models;
using SMS.API.Models.Enums;

namespace SMS.API.Controllers
{
    [Authorize(Roles = "SchoolAdmin")]
    [ApiController]
    [Route("api/schooladmin")]
    public class SchoolAdminController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        // 🔹 Get My School Details
        [HttpGet("school")]
        public async Task<IActionResult> GetMySchool()
        {
            var schoolId = int.Parse(User.FindFirst("SchoolId")!.Value);

            var school = await _context.Schools
                .Include(x => x.Subscription)
                .FirstOrDefaultAsync(x => x.Id == schoolId && x.IsActive);

            if (school == null)
                return NotFound("School not found");

            return Ok(school);
        }
        // 🔹 Get My School Details
        [HttpGet("classes")]
        public async Task<IActionResult> GetClasses()
        {
            var schoolId = int.Parse(User.FindFirst("SchoolId")!.Value);

            int totalClasses = await _context.Classes.CountAsync(x => x.SchoolId == schoolId && x.IsActive);

            return Ok(totalClasses);
        }

        // 🔹 Get Teachers
        [HttpGet("teachers")]
        public async Task<IActionResult> GetTeachers()
        {
            var schoolId = int.Parse(User.FindFirst("SchoolId")!.Value);

            var teachers = await _context.Users
                .Where(x => x.Role == UserRole.Teacher && x.SchoolId == schoolId)
                .Select(x => new
                {
                    x.Id,
                    x.FullName,
                    x.Email,
                    x.IsActive
                })
                .ToListAsync();

            return Ok(teachers);
        }

        // 🔹 Create Teacher
        [HttpPost("teachers")]
        public async Task<IActionResult> CreateTeacher(CreateTeacherDto dto)
        {
            var schoolId = int.Parse(User.FindFirst("SchoolId")!.Value);

            if (await _context.Users.AnyAsync(x => x.Email == dto.Email))
                return BadRequest("Email already exists");

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = UserRole.Teacher,
                SchoolId = schoolId,
                IsActive = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var teacher = new Teacher
            {
                UserId = user.Id,
                Subject = dto.Subject,
                SchoolId = schoolId,
                IsActive = true
            };

            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();

            return Ok("Teacher created successfully");
        }
        // 🔹 Update Teacher
        [HttpPut("teachers")]
        public async Task<IActionResult> UpdateTeacher(CreateTeacherDto dto)
        {
            var schoolId = int.Parse(User.FindFirst("SchoolId")!.Value);

            if (await _context.Users.AnyAsync(x => x.Email == dto.Email))
                return BadRequest("Email already exists");

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = UserRole.Teacher,
                SchoolId = schoolId
            };

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            var teacher = new Teacher
            {
                UserId = user.Id,
                Subject = dto.Subject
            };

            _context.Teachers.Update(teacher);
            await _context.SaveChangesAsync();

            return Ok("Teacher created successfully");
        }
        // 🔹 Delete Teacher
        [HttpDelete("teachers/{id}")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            var schoolId = int.Parse(User.FindFirst("SchoolId")!.Value);
            var teacher = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (teacher == null)
                return BadRequest("User not exists");

                 

            _context.Users.Remove(teacher);
            await _context.SaveChangesAsync();

            return Ok("Teacher created successfully");
        }

        // 🔹 Get Students
        [HttpGet("students")]
        public async Task<IActionResult> GetStudents()
        {
            var schoolId = int.Parse(User.FindFirst("SchoolId")!.Value);

            var students = await _context.Students
                .Where(x => x.SchoolId == schoolId)
                .Select(x => new
                {
                    x.Id,
                    x.FullName,
                    x.Email
                })
                .ToListAsync();

            return Ok(students);
        }

        // 🔹 Create Student
        //[HttpPost("students")]
        //public async Task<IActionResult> CreateStudent(CreateStudentDto dto)
        //{
        //    var schoolId = int.Parse(User.FindFirst("SchoolId")!.Value);

        //    if (await _context.Users.AnyAsync(x => x.Email == dto.Email))
        //        return BadRequest("Email already exists");

        //    var student = new User
        //    {
        //        FullName = dto.FullName,
        //        Email = dto.Email,
        //        PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
        //        Role = UserRole.Student,
        //        SchoolId = schoolId
        //    };

        //    _context.Users.Add(student);
        //    await _context.SaveChangesAsync();

        //    return Ok("Student created successfully");
        //}

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var schoolId = int.Parse(User.FindFirst("SchoolId")!.Value);

            var totalTeachers = await _context.Users
                .CountAsync(x => x.Role == UserRole.Teacher && x.SchoolId == schoolId);

            var totalStudents = await _context.Students
                .CountAsync(x => x.SchoolId == schoolId);

            var totalClasses = await _context.Classes
                .CountAsync(x => x.SchoolId == schoolId && x.IsActive);

            var feesPaid = await _context.Fees
                .Where(x => x.SchoolId == schoolId && x.IsPaid)
                .SumAsync(x => (int?)x.Amount) ?? 0;

            var feesPending = await _context.Fees
                .Where(x => x.SchoolId == schoolId && !x.IsPaid)
                .SumAsync(x => (int?)x.Amount) ?? 0;

            // Sample weekly attendance (replace with real attendance later)
            var weeklyAttendance = new List<int> { 82, 88, 91, 86, 90 };

            return Ok(new DashboardDto
            {
                TotalTeachers = totalTeachers,
                TotalStudents = totalStudents,
                TotalClasses = totalClasses,
                FeesPaid = feesPaid,
                FeesPending = feesPending,
                WeeklyAttendance = weeklyAttendance
            });
        }
    }
}