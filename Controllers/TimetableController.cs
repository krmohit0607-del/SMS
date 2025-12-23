using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMS.API.Data;
using SMS.API.DTOs;
using SMS.API.Models;

namespace SMS.API.Controllers
{
    [Authorize(Roles = "SchoolAdmin")]
    [ApiController]
    [Route("api/schooladmin/timetable")]
    public class TimetableController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TimetableController(ApplicationDbContext context) => _context = context;

        private int SchoolId =>
            int.Parse(User.FindFirst("SchoolId")!.Value);

        // ➜ Add Slot
        [HttpPost]
        public async Task<IActionResult> Create(CreateTimetableDto dto)
        {
            var slot = new Timetable
            {
                SchoolId = SchoolId,
                ClassId = dto.ClassId,
                SubjectId = dto.SubjectId,
                Day = dto.Day,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime
            };

            _context.Timetables.Add(slot);
            await _context.SaveChangesAsync();

            return Ok(slot);
        }

        // ➜ Get Class Timetable
        [HttpGet("{classId}")]
        public async Task<IActionResult> Get(int classId)
        {
            var data = await _context.Timetables
                .Where(x => x.SchoolId == SchoolId && x.ClassId == classId)
                .Include(x => x.Subject)
                .ThenInclude(x => x.Teacher)
                .OrderBy(x => x.Day)
                .ThenBy(x => x.StartTime)
                .ToListAsync();

            return Ok(data);
        }
    }
}