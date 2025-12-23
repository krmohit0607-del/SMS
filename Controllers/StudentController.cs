using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMS.API.Data;
using SMS.API.DTOs;
using SMS.API.Models;

[Authorize(Roles = "SchoolAdmin")]
[ApiController]
[Route("api/schooladmin/students")]
public class StudentController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public StudentController(ApplicationDbContext context)
    {
        _context = context;
    }

    private int SchoolId =>
        int.Parse(User.FindFirst("SchoolId")!.Value);

    // ➜ Add Student
    [HttpPost]
    public async Task<IActionResult> Create(CreateStudentDto dto)
    {
        if (await _context.Students.AnyAsync(x =>
            x.RollNumber == dto.RollNumber &&
            x.ClassId == dto.ClassId))
            return BadRequest("Roll number already exists");

        var student = new Student
        {
            SchoolId = SchoolId,
            ClassId = dto.ClassId,
            FullName = dto.FullName,
            RollNumber = dto.RollNumber,
            Email = dto.Email,
            DateOfBirth = dto.DateOfBirth
        };

        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        return Ok(student);
    }

    // ➜ Get Students
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _context.Students
            .Where(x => x.SchoolId == SchoolId && x.IsActive)
            .Include(x => x.Class)
            .ToListAsync());
    }

    // ➜ Delete Student
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var student = await _context.Students
            .FirstOrDefaultAsync(x => x.Id == id && x.SchoolId == SchoolId);

        if (student == null) return NotFound();

        student.IsActive = false;
        await _context.SaveChangesAsync();

        return Ok();
    }
}
