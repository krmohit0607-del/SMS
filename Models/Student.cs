namespace SMS.API.Models
{
    public class Student
    {
        public int Id { get; set; }

        public int SchoolId { get; set; }
        public int ClassId { get; set; }

        public string FullName { get; set; } = null!;
        public string RollNumber { get; set; } = null!;
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }

        public bool IsActive { get; set; } = true;

        public School School { get; set; }
        public Class Class { get; set; }
    }
}
