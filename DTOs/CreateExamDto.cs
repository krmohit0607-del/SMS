namespace SMS.API.DTOs
{
    public class CreateExamDto
    {
        public string Name { get; set; } = null!;
        public DateTime ExamDate { get; set; }
        public int SchoolId { get; set; }
    }
}
