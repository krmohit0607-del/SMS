using System.Security.Claims;

namespace SMS.API.Models
{
    public class Attendance
    {
        public int Id { get; set; }

        public int SchoolId { get; set; }
        public int StudentId { get; set; }
        public int ClassId { get; set; }
        public int SubjectId { get; set; }
        public int TeacherId { get; set; }

        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }

        public School School { get; set; }
        public Student Student { get; set; }
        public Class Class { get; set; }
    }
}
