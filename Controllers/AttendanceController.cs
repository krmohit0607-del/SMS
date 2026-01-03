//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using SMS.API.Data;
//using SMS.API.DTOs;
//using SMS.API.Models;
//using System.Security.Claims;

//namespace SMS.API.Controllers
//{
//    [ApiController]
//    [Route("api/schooladmin/attendance")]
//    [Authorize(Roles = "SchoolAdmin")]
//    public class AttendanceController : ControllerBase
//    {
//        private readonly ApplicationDbContext _context;

//        public AttendanceController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        private int SchoolId =>
//            int.Parse(User.FindFirst("SchoolId")!.Value);

//        // ✅ Mark attendance
//        [HttpPost]
//        public async Task<IActionResult> MarkAttendance(MarkAttendanceDto dto)
//        {
//            var exists = await _context.Attendances.AnyAsync(x =>
//                x.StudentId == dto.StudentId &&
//                x.Date.Date == dto.Date.Date);

//            if (exists)
//                return BadRequest("Attendance already marked");

//            var attendance = new Attendance
//            {
//                SchoolId = SchoolId,
//                StudentId = dto.StudentId,
//                ClassId = dto.ClassId,
//                Date = dto.Date,
//                IsPresent = dto.IsPresent
//            };

//            _context.Attendances.Add(attendance);
//            await _context.SaveChangesAsync();

//            return Ok("Attendance marked");
//        }

//        // ✅ Daily summary (for charts)
//        [HttpGet("summary")]
//        public async Task<ActionResult<AttendanceSummaryDto>> GetSummary()
//        {
//            var today = DateTime.UtcNow.Date;

//            var present = await _context.Attendances
//                .CountAsync(x => x.SchoolId == SchoolId && x.Date == today && x.IsPresent);

//            var absent = await _context.Attendances
//                .CountAsync(x => x.SchoolId == SchoolId && x.Date == today && !x.IsPresent);

//            return new AttendanceSummaryDto
//            {
//                Present = present,
//                Absent = absent
//            };
//        }

//        // ✅ Class-wise attendance
//        [HttpGet("class/{classId}")]
//        public async Task<IActionResult> GetClassAttendance(int classId)
//        {
//            var data = await _context.Attendances
//                .Where(x => x.ClassId == classId && x.SchoolId == SchoolId)
//                .OrderByDescending(x => x.Date)
//                .Select(x => new
//                {
//                    x.StudentId,
//                    x.Date,
//                    x.IsPresent
//                })
//                .ToListAsync();

//            return Ok(data);
//        }
//    }
//}


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMS.API.Data;
using SMS.API.DTOs;
using SMS.API.Models;
using System.Security.Claims;

namespace SMS.API.Controllers
{
    [ApiController]
    [Route("api/teacher/attendance")]
    [Authorize(Roles = "Teacher")]
    public class AttendanceController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        private int TeacherId =>
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        private int SchoolId =>
            int.Parse(User.FindFirst("SchoolId")!.Value);

        // ✅ Get classes assigned to teacher
        [HttpGet("classes")]
        public async Task<IActionResult> GetMyClasses()
        {
            var classes = await _context.Classes
                .Where(s => s.TeacherId == TeacherId)
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    s.Section
                })
                .Distinct()
                .ToListAsync();

            return Ok(classes);
        }

        // ✅ Get subjects for selected class
        [HttpGet("subjects/{classId}")]
        public async Task<IActionResult> GetSubjects(int classId)
        {
            var subjects = await _context.Subjects
                .Where(s => s.TeacherId == TeacherId && s.ClassId == classId)
                .Select(s => new { s.Id, s.Name })
                .ToListAsync();

            return Ok(subjects);
        }

        // ✅ Get students of class
        [HttpGet("students/{classId}")]
        public async Task<IActionResult> GetStudents(int classId)
        {
            var students = await _context.Students
                .Where(s => s.ClassId == classId)
                .Select(s => new { s.Id, s.FullName, s.RollNumber })
                .ToListAsync();

            return Ok(students);
        }

        // ✅ Mark attendance
        [HttpPost]
        public async Task<IActionResult> MarkAttendance(MarkAttendanceDto dto)
        {
            // 🔐 Validate subject ownership
            var subject = await _context.Subjects.FirstOrDefaultAsync(s =>
                s.Id == dto.SubjectId &&
                s.ClassId == dto.ClassId &&
                s.TeacherId == TeacherId);

            if (subject == null)
                return Forbid("You are not assigned to this subject");

            // ❌ Prevent duplicate attendance
            var alreadyMarked = await _context.Attendances.AnyAsync(a =>
                a.ClassId == dto.ClassId &&
                a.SubjectId == dto.SubjectId &&
                a.Date.Date == dto.Date.Date);

            if (alreadyMarked)
                return BadRequest("Attendance already marked");

            foreach (var s in dto.Students)
            {
                _context.Attendances.Add(new Attendance
                {
                    SchoolId = SchoolId,
                    ClassId = dto.ClassId,
                    SubjectId = dto.SubjectId,
                    StudentId = s.StudentId,
                    Date = dto.Date.Date,
                    IsPresent = s.IsPresent
                });
            }

            await _context.SaveChangesAsync();
            return Ok("Attendance saved");
        }
    }
}