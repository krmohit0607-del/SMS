using Microsoft.EntityFrameworkCore;
using SMS.API.Data;
using System.Security.Claims;

namespace SMS.API.Middleware
{
    public class SubscriptionValidationMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context, ApplicationDbContext db)
        {
            var path = context.Request.Path.Value?.ToLower();

            // Skip swagger & auth
            if (path!.Contains("swagger") || path.Contains("auth"))
            {
                await _next(context);
                return;
            }

            if (!context.User.Identity?.IsAuthenticated ?? true)
            {
                await _next(context);
                return;
            }

            var role = context.User.FindFirst(ClaimTypes.Role)?.Value;

            // SuperAdmin is never blocked
            if (role == "SuperAdmin")
            {
                await _next(context);
                return;
            }

            var schoolIdClaim = context.User.FindFirst("SchoolId")?.Value;

            if (!int.TryParse(schoolIdClaim, out int schoolId))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid School");
                return;
            }

            var school = await db.Schools
                .Include(x => x.Subscription)
                .FirstOrDefaultAsync(x => x.Id == schoolId && x.IsActive);

            if (school == null)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("School inactive or not found");
                return;
            }

            if (!school.Subscription.IsActive)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Subscription inactive");
                return;
            }

            await _next(context);
        }
    }
}
