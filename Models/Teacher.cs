namespace SMS.API.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public string Subject { get; set; } = string.Empty;

        public int SchoolId { get; set; }
        public School School { get; set; } = null!;

        public bool IsActive { get; set; } = true;
    }
}
