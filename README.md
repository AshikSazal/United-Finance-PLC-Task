# 1. Professional Project Structure

```
LoanProcedure
│
├── Controllers
│    ├── CustomerController.cs
│    └── LoanController.cs
│
├── Models
│    ├── Customer.cs
│    └── Loan.cs
│
├── DTOs
│    ├── Customer
│    │     ├── CustomerCreateDto.cs
│    │     ├── CustomerUpdateDto.cs
│    │     └── CustomerResponseDto.cs
│    │
│    └── Loan
│          ├── LoanCreateDto.cs
│          ├── LoanUpdateDto.cs
│          └── LoanResponseDto.cs
│
├── Data
│    └── DbHelper.cs
│
├── Repositories
│    ├── Interfaces
│    │     ├── ICustomerRepository.cs
│    │     └── ILoanRepository.cs
│    │
│    ├── CustomerRepository.cs
│    └── LoanRepository.cs
│
├── Services
│    ├── CustomerService.cs
│    └── LoanService.cs
│
├── Views
│    ├── Customer
│    ├── Loan
│    └── Shared
│
├── wwwroot
│
├── appsettings.json
└── Program.cs

```

This architecture separates:

```
Controller → Service → Repository → Database
```