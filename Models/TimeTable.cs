namespace SMS.API.Models
{
    public class Timetable
    {
        public int Id { get; set; }

        public int SchoolId { get; set; }
        public int ClassId { get; set; }
        public int SubjectId { get; set; }

        public DayOfWeek Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public Class Class { get; set; }
        public Subject Subject { get; set; }
    }
}
