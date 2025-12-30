namespace SMS.API.Models
{
    public class TeacherClass
    {
        public int Id { get; set; }

        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; } = null!;

        public int ClassId { get; set; }
        public Class Class { get; set; } = null!;

        public string Subject { get; set; } = string.Empty;
    }
}
