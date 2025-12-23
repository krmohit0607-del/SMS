namespace SMS.API.DTOs
{
    public class CreateSchoolAdminDto
    {
        public int SchoolId { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FullName { get; set; } = null!;
    }
}
