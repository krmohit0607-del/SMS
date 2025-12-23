namespace SMS.API.DTOs
{
    public class CreateTimetableDto
    {
        public int ClassId { get; set; }
        public int SubjectId { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
