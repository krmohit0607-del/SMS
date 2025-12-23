namespace SMS.API.DTOs.SuperAdmin
{
    public class CreateSubscriptionDto
    {
        public string PlanName { get; set; } = null!;
        public decimal Price { get; set; }
        public int DurationInMonths { get; set; }
        public int MaxTeachers { get; set; }
        public int MaxStudents { get; set; }
    }
}
