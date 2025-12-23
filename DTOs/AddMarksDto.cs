namespace SMS.API.DTOs
{
    public class AddMarksDto
    {
        public int StudentId { get; set; }
        public int ExamId { get; set; }
        public string Subject { get; set; } = null!;
        public decimal Score { get; set; }
        public decimal MaxScore { get; set; }
    }
}
