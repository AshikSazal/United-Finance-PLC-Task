using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Loan_Procedure.Models
{
    public class Loan
    {
        [Key]
        public int LoanId { get; set; }

        [Required(ErrorMessage = "Customer is required")]
        public int? CustomerId { get; set; }

        [Required(ErrorMessage = "Loan amount is required")]
        [Range(1, 1000000000, ErrorMessage = "Amount must be greater than 0")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; }

        [Required(ErrorMessage = "Loan type is required")]
        [StringLength(50, ErrorMessage = "Loan type cannot exceed 50 characters")]
        public string? LoanType { get; set; }

        [Required(ErrorMessage = "Tenor is required")]
        [Range(1, 360, ErrorMessage = "Tenor must be between 1 and 360 months")]
        public int? Tenor { get; set; }

        [Required(ErrorMessage = "Purpose is required")]
        [StringLength(200, ErrorMessage = "Purpose cannot exceed 200 characters")]
        public string? Purpose { get; set; }
        public byte? Status { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
