namespace SMS.API.Models
{
    public class Subject
    {
        public int Id { get; set; }

        public int ClassId { get; set; }
        public string Name { get; set; } = null!;   // Maths, Science
        public int TeacherId { get; set; }          // UserId (Teacher)

        public Class Class { get; set; }
        public User Teacher { get; set; }
    }
}
