using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMS.API.Models
{
    public class Fee
    {
        [Key]
        public int Id { get; set; }

        public int SchoolId { get; set; }

        public int StudentId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public bool IsPaid { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime? PaidDate { get; set; }
    }
}
