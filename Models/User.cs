using SMS.API.Models.Enums;

namespace SMS.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public UserRole Role { get; set; }

        public int? SchoolId { get; set; }
        public School? School { get; set; }
        public Teacher? Teacher { get; set; }

    }
}
