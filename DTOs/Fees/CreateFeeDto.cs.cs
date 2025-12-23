using System.ComponentModel.DataAnnotations;

namespace SMS.API.DTOs.Fees
{
    public class CreateFeeDto
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime DueDate { get; set; }
    }
}
