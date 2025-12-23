namespace SMS.API.DTOs
{
    public class CreateStudentDto1
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FullName { get; set; } = null!;

        public string RollNumber { get; set; } = null!;
        public string Class { get; set; } = null!;
        public string Section { get; set; } = null!;
    }
}
