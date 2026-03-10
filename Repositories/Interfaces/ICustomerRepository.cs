using Loan_Procedure.DTOs.Customer;
using Loan_Procedure.Models;
using Loan_Procedure.Utils;

namespace Loan_Procedure.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        Response AddCustomer(Customer customer);
        List<CustomerResponseDto> GetCustomers();
        Response UpdateCustomer(Customer customer);
        Customer GetCustomer(int Id);
    }
}
