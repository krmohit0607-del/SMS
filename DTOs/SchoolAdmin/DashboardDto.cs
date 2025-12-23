namespace SMS.API.DTOs.SchoolAdmin
{
    public class DashboardDto
    {
        public int TotalTeachers { get; set; }
        public int TotalStudents { get; set; }
        public int TotalClasses { get; set; }

        public int FeesPaid { get; set; }
        public int FeesPending { get; set; }

        public List<int> WeeklyAttendance { get; set; }
    }
}
