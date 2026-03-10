using Loan_Procedure.DTOs.Loan;
using Loan_Procedure.Models;
using Loan_Procedure.Utils;

namespace Loan_Procedure.Repositories.Interfaces
{
    public interface ILoanRepository
    {
        Response CreateLoan(Loan loan);
        List<LoanResponseDto> GetLoans(int? status, int? customerId);
        Response UpdateLoan(Loan loan);
        Response UpdateStatus(int loanId, int status);
        Loan GetLoan(int id);
    }
}
