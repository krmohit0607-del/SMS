namespace SMS.API.Models
{
    public class SchoolAdmin
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int SchoolId { get; set; }
        public School School { get; set; } = null!;
    }
}
