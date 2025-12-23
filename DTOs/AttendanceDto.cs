namespace SMS.API.DTOs
{
    public class MarkAttendanceDto
    {
        public int StudentId { get; set; }
        public int ClassId { get; set; }
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }
    }

    public class AttendanceSummaryDto
    {
        public int Present { get; set; }
        public int Absent { get; set; }
    }
}
