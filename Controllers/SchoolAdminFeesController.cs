using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMS.API.Data;
using SMS.API.DTOs.Fees;
using SMS.API.Models;

namespace SMS.API.Controllers
{
    [Authorize(Roles = "SchoolAdmin")]
    [ApiController]
    [Route("api/schooladmin/fees")]
    public class SchoolAdminFeesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SchoolAdminFeesController(ApplicationDbContext context) => _context = context;

        private int GetSchoolId()
        {
            return int.Parse(User.FindFirst("SchoolId")!.Value);
        }

        // 1️⃣ Add Fee
        [HttpPost]
        public async Task<IActionResult> CreateFee(CreateFeeDto dto)
        {
            var schoolId = GetSchoolId();

            var fee = new Fee
            {
                SchoolId = schoolId,
                StudentId = dto.StudentId,
                Amount = dto.Amount,
                DueDate = dto.DueDate,
                IsPaid = false
            };

            _context.Fees.Add(fee);
            await _context.SaveChangesAsync();

            return Ok(fee);
        }

        // 2️⃣ Get All Fees
        [HttpGet]
        public async Task<IActionResult> GetFees()
        {
            var schoolId = GetSchoolId();

            var fees = await _context.Fees
                .Where(x => x.SchoolId == schoolId)
                .OrderByDescending(x => x.DueDate)
                .ToListAsync();

            return Ok(fees);
        }

        // 3️⃣ Get Pending Fees
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingFees()
        {
            var schoolId = GetSchoolId();

            var fees = await _context.Fees
                .Where(x => x.SchoolId == schoolId && !x.IsPaid)
                .ToListAsync();

            return Ok(fees);
        }

        // 4️⃣ Mark Fee as Paid
        [HttpPut("pay/{id}")]
        public async Task<IActionResult> PayFee(int id)
        {
            var schoolId = GetSchoolId();

            var fee = await _context.Fees
                .FirstOrDefaultAsync(x => x.Id == id && x.SchoolId == schoolId);

            if (fee == null)
                return NotFound("Fee not found");

            fee.IsPaid = true;
            fee.PaidDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok("Fee marked as paid");
        }
    }
}