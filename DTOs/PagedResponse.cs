namespace Loan_Procedure.DTOs
{
    public class PagedResponse<T>
    {
        public List<T> Items { get; set; } = [];
        public int TotalRecords { get; set; }
    }
}
