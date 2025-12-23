namespace SMS.API.DTOs.SuperAdmin
{
    public class CreateSchoolAdminDto
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int SchoolId { get; set; }
    }

}
