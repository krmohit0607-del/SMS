namespace SMS.API.Models
{
    public class Exam
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;   // Mid Term, Final
        public DateTime ExamDate { get; set; }

        public int SchoolId { get; set; }
    }
}
