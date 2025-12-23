namespace SMS.API.Models
{
    public class Mark
    {
        public int Id { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public int ExamId { get; set; }
        public Exam Exam { get; set; } = null!;

        public string Subject { get; set; } = null!;
        public decimal Score { get; set; }
        public decimal MaxScore { get; set; }
    }
}
