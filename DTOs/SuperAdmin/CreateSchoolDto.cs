namespace SMS.API.DTOs.SuperAdmin
{
    public class CreateSchoolDto
    {
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string Address { get; set; } = null!;
        public int SubscriptionId { get; set; }
    }
}
