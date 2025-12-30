namespace SMS.API.DTOs
{
    public class MarkAttendanceDto
    {
        public int ClassId { get; set; }
        public int SubjectId { get; set; }
        public DateTime Date { get; set; }

        public List<StudentAttendanceDto> Students { get; set; }
    }

    public class StudentAttendanceDto
    {
        public int StudentId { get; set; }
        public bool IsPresent { get; set; }
    }

}
