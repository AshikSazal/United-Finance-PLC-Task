namespace Loan_Procedure.DTOs.Customer
{
    public class CustomerResponseDto
    {
        public int CustomerId { get; set; }
        public string? Name { get; set; }
        public string? NID { get; set; }
        public string? Mobile { get; set; }
        public decimal Income { get; set; }
    }
}
