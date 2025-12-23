namespace SMS.API.Models
{
    public class School
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string Address { get; set; } = null!;
        public bool IsActive { get; set; } = true;

        public int SubscriptionId { get; set; }
        public Subscription Subscription { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
