namespace SMS.API.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        public string PlanName { get; set; } = null!;
        public decimal Price { get; set; }
        public int DurationInMonths { get; set; }
        public int MaxTeachers { get; set; }
        public int MaxStudents { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<School> Schools { get; set; } = new List<School>();
    }

}
