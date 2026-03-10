using Loan_Procedure.DTOs;
using Loan_Procedure.DTOs.Customer;
using Loan_Procedure.Models;
using Loan_Procedure.Utils;

namespace Loan_Procedure.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        Response AddCustomer(Customer customer);
        List<CustomerResponseDto> GetCustomers();
        PagedResult<CustomerResponseDto> GetCustomers(int page = 1, int pageSize = 10);
        Response UpdateCustomer(Customer customer);
        Customer? GetCustomer(int Id);
    }
}
