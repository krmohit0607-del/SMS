using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMS.API.Data;
using SMS.API.Models;

namespace SMS.API.Controllers
{
    [ApiController]
    [Route("api/superadmin/schools/subscription")]
    [Authorize(Roles = "SuperAdmin")]
    public class SchoolSubscriptionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SchoolSubscriptionController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AssignSubscription(
            int schoolId,
            int subscriptionId)
        {
            var school = await _context.Schools.FindAsync(schoolId);
            if (school == null)
                return NotFound("School not found");

            var subscription = await _context.Subscriptions.FindAsync(subscriptionId);
            if (subscription == null)
                return NotFound("Subscription not found");

            // ❌ Disable previous subscription
            var old = await _context.SchoolSubscriptions
                .Where(x => x.SchoolId == schoolId && x.IsActive)
                .ToListAsync();

            foreach (var item in old)
                item.IsActive = false;

            // ✅ Assign new subscription
            var start = DateTime.UtcNow;
            var end = start.AddMonths(subscription.DurationInMonths);

            var schoolSub = new SchoolSubscription
            {
                SchoolId = schoolId,
                SubscriptionId = subscriptionId,
                StartDate = start,
                EndDate = end,
                IsActive = true
            };

            school.IsActive = true;

            _context.SchoolSubscriptions.Add(schoolSub);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Subscription assigned successfully",
                StartDate = start,
                EndDate = end
            });
        }
    }
}
