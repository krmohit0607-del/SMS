using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMS.API.Data;
using SMS.API.Models;

[ApiController]
[Route("api/teacher")]
[Authorize(Roles = "Teacher")]
public class TeacherController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TeacherController(ApplicationDbContext context)
    {
        _context = context;
    }

    private int TeacherId =>
        int.Parse(User.FindFirst("TeacherId")!.Value);

    private int SchoolId =>
        int.Parse(User.FindFirst("SchoolId")!.Value);

    // ✅ Dashboard
    [HttpGet("dashboard")]
    public async Task<IActionResult> Dashboard()
    {
        var classes = await _context.TeacherClasses
            .Where(x => x.TeacherId == TeacherId)
            .CountAsync();

        var subjects = await _context.TeacherClasses
            .Where(x => x.TeacherId == TeacherId)
            .Select(x => x.Subject)
            .Distinct()
            .CountAsync();

        var today = DateTime.UtcNow.Date;

        var attendanceCount = await _context.Attendances
            .Where(x => x.TeacherId == TeacherId && x.Date == today)
            .CountAsync();

        return Ok(new
        {
            classes,
            subjects,
            todayAttendance = attendanceCount
        });
    }

    // ✅ My classes
    [HttpGet("classes")]
    public async Task<IActionResult> MyClasses()
    {
        var data = await _context.TeacherClasses
            .Where(x => x.TeacherId == TeacherId)
            .Select(x => new
            {
                x.ClassId,
                ClassName = x.Class.Name,
                x.Subject
            })
            .ToListAsync();

        return Ok(data);
    }

    // ✅ Students of class
    [HttpGet("students/{classId}")]
    public async Task<IActionResult> Students(int classId)
    {
        var allowed = await _context.TeacherClasses
            .AnyAsync(x => x.TeacherId == TeacherId && x.ClassId == classId);

        if (!allowed)
            return Forbid();

        var students = await _context.Students
            .Where(x => x.ClassId == classId && x.IsActive)
            .Select(x => new
            {
                x.Id,
                x.RollNumber,
                x.FullName
            })
            .ToListAsync();

        return Ok(students);
    }
    [HttpGet("timetable")]
    public async Task<IActionResult> Timetable()
    {
        var data = await _context.TeacherClasses
            .Where(x => x.TeacherId == TeacherId)
            .Select(x => new
            {
                //Day = x.Class.Day,
                Class = x.Class.Name,
                x.Subject,
                //x.Class.StartTime,
                //x.Class.EndTime
            })
            .ToListAsync();

        return Ok(data);
    }

}
