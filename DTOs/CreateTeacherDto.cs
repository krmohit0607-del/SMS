namespace SMS.API.DTOs
{
    public class CreateTeacherDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Subject { get; set; } = null!;
    }
}
