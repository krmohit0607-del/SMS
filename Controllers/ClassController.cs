using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMS.API.Data;
using SMS.API.DTOs;
using SMS.API.Models;

namespace SMS.API.Controllers
{
    [ApiController]
    [Route("api/classes")]
    [Authorize(Roles = "SchoolAdmin")]
    public class ClassController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClassController(ApplicationDbContext context)
        {
            _context = context;
        }

        private int SchoolId =>
            int.Parse(User.FindFirst("SchoolId")!.Value);

        // ✅ Create Class
        [HttpPost]
        public async Task<IActionResult> CreateClass(CreateClassDto dto)
        {
            var exists = await _context.Classes.AnyAsync(x =>
                x.SchoolId == SchoolId &&
                x.Name == dto.Name &&
                x.Section == dto.Section);

            if (exists)
                return BadRequest("Class already exists");

            var cls = new Class
            {
                SchoolId = SchoolId,
                Name = dto.Name,
                Section = dto.Section
            };

            _context.Classes.Add(cls);
            await _context.SaveChangesAsync();

            return Ok(cls);
        }

        // ✅ Get Classes
        [HttpGet]
        public async Task<IActionResult> GetClasses()
        {
            var classes = await _context.Classes
                .Where(x => x.SchoolId == SchoolId && x.IsActive)
                .Include(x => x.Subjects)
                .ThenInclude(x => x.Teacher)
                .ToListAsync();

            return Ok(classes);
        }

        // ✅ Add Subject
        [HttpPost("subjects")]
        public async Task<IActionResult> AddSubject(CreateSubjectDto dto)
        {
            var cls = await _context.Classes
                .FirstOrDefaultAsync(x => x.Id == dto.ClassId && x.SchoolId == SchoolId);

            if (cls == null)
                return BadRequest("Invalid class");

            var subject = new Subject
            {
                ClassId = dto.ClassId,
                Name = dto.Name,
                TeacherId = dto.TeacherId
            };

            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();

            return Ok(subject);
        }

        // ✅ Delete Class (Soft delete)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClass(int id)
        {
            var cls = await _context.Classes
                .FirstOrDefaultAsync(x => x.Id == id && x.SchoolId == SchoolId);

            if (cls == null)
                return NotFound();

            cls.IsActive = false;
            await _context.SaveChangesAsync();

            return Ok("Class deleted");
        }
    }
}
