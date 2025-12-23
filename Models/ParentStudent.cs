namespace SMS.API.Models
{
    public class ParentStudent
    {
        public int Id { get; set; }

        public int ParentUserId { get; set; }   // AspNetUsers.Id
        public int StudentId { get; set; }

        public Student Student { get; set; } = null!;
    }
}
