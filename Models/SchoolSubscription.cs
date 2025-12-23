namespace SMS.API.Models
{
    public class SchoolSubscription
    {
        public int Id { get; set; }

        public int SchoolId { get; set; }
        public School School { get; set; } = null!;

        public int SubscriptionId { get; set; }
        public Subscription Subscription { get; set; } = null!;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
