namespace SMS.API.Models
{
    public class Class
    {
        public int Id { get; set; }

        public int SchoolId { get; set; }
        public string Name { get; set; } = null!;   // e.g. Class 1, Class 2
        public string Section { get; set; } = null!; // A, B, C
        public bool IsActive { get; set; } = true;

        public School School { get; set; }
        public ICollection<Subject> Subjects { get; set; }
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
