using System.ComponentModel.DataAnnotations;

namespace Loan_Procedure.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get;set; }

        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "NID is required")]
        [RegularExpression(@"^\d{10,17}$", ErrorMessage = "NID must be 10 to 17 digits")]
        public string? NID { get; set; }

        [Required(ErrorMessage = "Mobile number is required")]
        [Phone(ErrorMessage = "Invalid mobile number")]
        [RegularExpression(@"^01\d{9}$", ErrorMessage = "Mobile must be 11 digits and start with 01")]
        public string? Mobile { get; set; }

        [Required(ErrorMessage = "Income is required")]
        [Range(1, 100000000, ErrorMessage = "Income must be greater than 0")]
        public decimal? Income { get; set; }
    }
}
