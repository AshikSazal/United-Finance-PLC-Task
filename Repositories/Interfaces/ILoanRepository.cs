using Loan_Procedure.DTOs;
using Loan_Procedure.DTOs.Loan;
using Loan_Procedure.Models;
using Loan_Procedure.Utils;

namespace Loan_Procedure.Repositories.Interfaces
{
    public interface ILoanRepository
    {
        Response CreateLoan(Loan loan);
        PagedResult<LoanResponseDto> GetLoans(int? status, int? customerId, int page = 1, int pageSize = 10);
        Response UpdateLoan(Loan loan);
        Response UpdateStatus(int loanId, int status);
        Loan? GetLoan(int id);
    }
}
