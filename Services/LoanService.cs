using Loan_Procedure.DTOs;
using Loan_Procedure.DTOs.Loan;
using Loan_Procedure.Models;
using Loan_Procedure.Repositories.Interfaces;
using Loan_Procedure.Utils;

namespace Loan_Procedure.Services
{
    public class LoanService
    {
        private readonly ILoanRepository _loanRepository;

        public LoanService(ILoanRepository repo)
        {
            _loanRepository = repo;
        }

        public Response CreateLoan(Loan loan)
        {
            if (loan.Amount <= 0)
                return Response.Fail("Loan amount must be greater than zero.");
            return _loanRepository.CreateLoan(loan);
        }
        public PagedResult<LoanResponseDto> GetLoans(int? status, int? customerId, int page = 1, int pageSize = 10)
        {
            return _loanRepository.GetLoans(status, customerId, page, pageSize);
        }

        public Response UpdateStatus(int loanId, int status)
        {
            return _loanRepository.UpdateStatus(loanId, status);
        }
        public Loan GetLoan(int id)
        {
            return _loanRepository.GetLoan(id);
        }
        public Response UpdateLoan(Loan loan)
        {
            return _loanRepository.UpdateLoan(loan);
        }
    }
}
