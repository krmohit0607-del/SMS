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
    [Route("api/schooladmin/attendance")]
    [Authorize(Roles = "SchoolAdmin")]
    public class AttendanceController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AttendanceController(ApplicationDbContext context)
        {
            _context = context;
        }

        private int SchoolId =>
            int.Parse(User.FindFirst("SchoolId")!.Value);

        // ✅ Mark attendance
        [HttpPost]
        public async Task<IActionResult> MarkAttendance(MarkAttendanceDto dto)
        {
            var exists = await _context.Attendances.AnyAsync(x =>
                x.StudentId == dto.StudentId &&
                x.Date.Date == dto.Date.Date);

            if (exists)
                return BadRequest("Attendance already marked");

            var attendance = new Attendance
            {
                SchoolId = SchoolId,
                StudentId = dto.StudentId,
                ClassId = dto.ClassId,
                Date = dto.Date,
                IsPresent = dto.IsPresent
            };

            _context.Attendances.Add(attendance);
            await _context.SaveChangesAsync();

            return Ok("Attendance marked");
        }

        // ✅ Daily summary (for charts)
        [HttpGet("summary")]
        public async Task<ActionResult<AttendanceSummaryDto>> GetSummary()
        {
            var today = DateTime.UtcNow.Date;

            var present = await _context.Attendances
                .CountAsync(x => x.SchoolId == SchoolId && x.Date == today && x.IsPresent);

            var absent = await _context.Attendances
                .CountAsync(x => x.SchoolId == SchoolId && x.Date == today && !x.IsPresent);

            return new AttendanceSummaryDto
            {
                Present = present,
                Absent = absent
            };
        }

        // ✅ Class-wise attendance
        [HttpGet("class/{classId}")]
        public async Task<IActionResult> GetClassAttendance(int classId)
        {
            var data = await _context.Attendances
                .Where(x => x.ClassId == classId && x.SchoolId == SchoolId)
                .OrderByDescending(x => x.Date)
                .Select(x => new
                {
                    x.StudentId,
                    x.Date,
                    x.IsPresent
                })
                .ToListAsync();

            return Ok(data);
        }
    }
}
