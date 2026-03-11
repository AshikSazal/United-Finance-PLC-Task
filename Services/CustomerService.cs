using Loan_Procedure.DTOs;
using Loan_Procedure.DTOs.Customer;
using Loan_Procedure.Models;
using Loan_Procedure.Repositories.Interfaces;
using Loan_Procedure.Utils;

namespace Loan_Procedure.Services
{
    public class CustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public Response CreateCustomer(Customer customer)
        {
            return _customerRepository.AddCustomer(customer);
        }
        public Customer GetCustomer(int Id)
        {
            return _customerRepository.GetCustomer(Id);
        }

        public List<CustomerResponseDto> GetCustomers()
        {
            return _customerRepository.GetCustomers();
        }
        public PagedResponse<CustomerResponseDto> GetCustomers(int page = 1, int pageSize = 10)
        {
            return _customerRepository.GetCustomers(page, pageSize);
        }
        public Response UpdateCustomer(Customer customer)
        {
            return _customerRepository.UpdateCustomer(customer);
        }
    }
}
