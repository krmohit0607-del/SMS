using System.Security.Claims;
namespace SMS.API.Helpers
{
    public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContext;

    public CurrentUserService(IHttpContextAccessor httpContext)
    {
        _httpContext = httpContext;
    }

    public string UserId =>
        _httpContext.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    public string Role =>
        _httpContext.HttpContext?.User.FindFirstValue(ClaimTypes.Role)!;

    public Guid? SchoolId
    {
        get
        {
            var value = _httpContext.HttpContext?.User.FindFirst("SchoolId")?.Value;
            return string.IsNullOrEmpty(value) ? null : Guid.Parse(value);
        }
    }
        public int GetUserId()
        {
            return int.Parse(
                _httpContext.HttpContext!
                .User.FindFirst("id")!.Value
            );
        }
    }

}
