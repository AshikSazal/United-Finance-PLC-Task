namespace Loan_Procedure.DTOs.Loan
{
    public class LoanResponseDto
    {
        public int LoanId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public decimal Amount { get; set; }
        public string LoanType { get; set; }
        public int Tenor { get; set; }
        public byte Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string StatusText
        {
            get
            {
                return Status switch
                {
                    0 => "Draft",
                    1 => "Submitted",
                    2 => "Approved",
                    3 => "Rejected"
                };
            }
        }
    }
}
