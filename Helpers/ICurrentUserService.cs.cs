namespace SMS.API.Helpers
{
    public interface ICurrentUserService
    {
        string UserId { get; }
        string Role { get; }
        Guid? SchoolId { get; }
        int GetUserId();
    }
}
