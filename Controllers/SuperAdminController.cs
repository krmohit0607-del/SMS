using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMS.API.Data;
using SMS.API.DTOs.SuperAdmin;
using SMS.API.Models;

namespace SMS.API.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    [ApiController]
    [Route("api/superadmin")]
    public class SuperAdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SuperAdminController(ApplicationDbContext context) => _context = context;

        // ➜ Create Subscription
        [HttpPost("subscriptions")]
        public async Task<IActionResult> CreateSubscription(CreateSubscriptionDto dto)
        {
            var subscription = new Subscription
            {
                //Id = Guid.NewGuid(),
                PlanName = dto.PlanName,
                Price = dto.Price,
                DurationInMonths = dto.DurationInMonths,
                MaxTeachers = dto.MaxTeachers,
                MaxStudents = dto.MaxStudents
            };

            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();

            return Ok(subscription);
        }

        // ➜ Get Subscriptions
        [HttpGet("subscriptions")]
        public async Task<IActionResult> GetSubscriptions()
        {
            return Ok(await _context.Subscriptions
                .Where(x => x.IsActive)
                .ToListAsync());
        }

        // ➜ Create School
        [HttpPost("schools")]
        public async Task<IActionResult> CreateSchool(CreateSchoolDto dto)
        {
            var subscription = await _context.Subscriptions
                .FirstOrDefaultAsync(x => x.Id == dto.SubscriptionId && x.IsActive);

            if (subscription == null)
                return BadRequest("Invalid subscription");

            var school = new School
            {
                //Id = Guid.NewGuid(),
                Name = dto.Name,
                Code = dto.Code,
                Address = dto.Address,
                SubscriptionId = dto.SubscriptionId
            };

            _context.Schools.Add(school);
            await _context.SaveChangesAsync();

            return Ok(school);
        }

        // ➜ Get Schools
        [HttpGet("schools")]
        public async Task<IActionResult> GetSchools()
        {
            return Ok(await _context.Schools
                .Include(x => x.Subscription)
                .Where(x => x.IsActive)
                .ToListAsync());
        }
        [HttpPost("school-admin")]
        public async Task<IActionResult> CreateSchoolAdmin(CreateSchoolAdminDto dto)
        {
            var school = await _context.Schools
                .FirstOrDefaultAsync(x => x.Id == dto.SchoolId && x.IsActive);

            if (school == null)
                return BadRequest("Invalid school");

            if (await _context.Users.AnyAsync(x => x.Email == dto.Email))
                return BadRequest("Email already exists");

            var user = new User
            {
                //Id = Guid.NewGuid(),
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = Models.Enums.UserRole.SchoolAdmin,
                SchoolId = dto.SchoolId
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "School Admin created successfully",
                user.Email,
                School = school.Name
            });
        }

    }
}