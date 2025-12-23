using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMS.API.Data;
using SMS.API.Helpers;

namespace SMS.API.Controllers
{
    [ApiController]
    [Route("api/parent")]
    [Authorize(Roles = "Parent")]
    public class ParentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly CurrentUserService _currentUser;

        public ParentController(
            ApplicationDbContext context,
            CurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        // 👨‍👩‍👧 Get My Children
        [HttpGet("children")]
        public async Task<IActionResult> GetMyChildren()
        {
            int parentId = _currentUser.GetUserId();

            var children = await _context.ParentStudents
                .Where(ps => ps.ParentUserId == parentId)
                .Select(ps => new
                {
                    ps.Student.Id,
                    ps.Student.FullName,
                    ps.Student.Class,
                    //ps.Student.Section
                })
                .ToListAsync();

            return Ok(children);
        }

        // 📅 Attendance
        [HttpGet("attendance/{studentId}")]
        public async Task<IActionResult> GetAttendance(int studentId)
        {
            int parentId = _currentUser.GetUserId();

            bool allowed = await _context.ParentStudents
                .AnyAsync(p => p.ParentUserId == parentId && p.StudentId == studentId);

            if (!allowed) return Forbid();

            var attendance = await _context.Attendances
                .Where(a => a.StudentId == studentId)
                .Select(a => new
                {
                    a.Date,
                    a.IsPresent
                })
                .ToListAsync();

            return Ok(attendance);
        }

        // 📝 Marks
        [HttpGet("marks/{studentId}")]
        public async Task<IActionResult> GetMarks(int studentId)
        {
            int parentId = _currentUser.GetUserId();

            bool allowed = await _context.ParentStudents
                .AnyAsync(p => p.ParentUserId == parentId && p.StudentId == studentId);

            if (!allowed) return Forbid();

            var marks = await _context.Marks
                .Where(m => m.StudentId == studentId)
                .Select(m => new
                {
                    Exam = m.Exam.Name,
                    m.Subject,
                    m.Score,
                    m.MaxScore
                })
                .ToListAsync();

            return Ok(marks);
        }
    }
}
