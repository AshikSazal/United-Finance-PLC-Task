# Mini Loan Approval System

## Project Overview

This is a small web application developed using **ASP.NET Core MVC** and **SQL Server** to manage loan requests and approval workflow.

The system allows users to create customers, submit loan requests, and process loan approvals following a defined workflow.

---

# Technology Stack

* ASP.NET Core MVC
* SQL Server
* ADO.NET
* Bootstrap (for UI)

---

# Features

## Customer Management

* Create new customers
* Fields:

  * Name
  * NID (Unique)
  * Mobile
  * Monthly Income

## Loan Application

Users can create loan requests with:

* Customer
* Loan Amount
* Loan Type
* Tenor (months)
* Purpose

Default Status: **Draft**

---

## Loan Workflow

Loan status follows this process:

Draft → Submitted → Approved / Rejected

Rules:

* Relationship Manager (RM) creates and submits loan
* Approver can **Approve** or **Reject**
* Once Approved/Rejected the loan **cannot be edited**

---

## Loan List Page

Displays:

* Customer Name
* Loan Amount
* Loan Type
* Status
* Created Date

Filters available:

* Filter by Status
* Filter by Customer

---

# Database Setup

## 1. Create Database

Open SQL Server and run:

```sql
CREATE DATABASE LoanProcedure;
```

---

## 2. Create Tables

```sql
CREATE TABLE Customers (
    CustomerId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    NID NVARCHAR(50) UNIQUE NOT NULL,
    Mobile NVARCHAR(20),
    Income DECIMAL(18,2)
);

CREATE TABLE Loans (
    LoanId INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT,
    Amount DECIMAL(18,2),
    LoanType NVARCHAR(50),
    Tenor INT,
    Purpose NVARCHAR(200),
    Status INT DEFAULT 0,
    CreatedDate DATETIME DEFAULT GETDATE(),

    FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId)
);
```

---

# Application Setup

## 1. Clone the Repository

```bash
git clone https://github.com/AshikSazal/United-Finance-PLC-Task.git
```

---

## 2. Open the Project

Open the solution in **Visual Studio**.

---

## 3. Configure Database Connection

Update `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=LoanProcedure;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True"
}
```

---

# Project Structure

```
LoanProcedure
│
├── Controllers
│   ├── CustomerController.cs
│   └── LoanController.cs
│
├── Models
│   ├── Customer.cs
│   └── Loan.cs
│
├── DTOs
│   ├── Customer
│   │    └── CustomerResponseDto.cs
│   │
│   └── Loan
│        └── LoanResponseDto.cs
│
├── DbHelper
│   └── DbConnection.cs
│
├── Repositories
│   ├── Interfaces
│   │    ├── ICustomerRepository.cs
│   │    └── ILoanRepository.cs
│   │
│   ├── CustomerRepository.cs
│   └── LoanRepository.cs
│
├── Services
│   ├── CustomerService.cs
│   └── LoanService.cs
│
├── Views
│   ├── Customer
│   │    ├── Create.cshtml
│   │    ├── Edit.cshtml
│   │    └── Index.cshtml
│   │
│   └── Loan
│        ├── Create.cshtml
│        ├── Edit.cshtml
│        └── Index.cshtml
│
├── wwwroot
│
├── Utils
│   ├── Constants
│   │    └── LoanStatus.cs
│   └── Response.cs
│
├── appsettings.json
└── Program.cs

```

```
Controllers     → Handle HTTP requests
Services        → Business logic
Repositories    → Database operations
DTOs            → Data transfer objects
Models          → Domain models
Views           → Razor UI pages
```

---

# Validation & Security

* Parameterized SQL queries used to prevent SQL injection
* Basic model validation implemented
* Error handling added in repository/service layer

---

# Author

Candidate Assignment – Mini Loan Approval System
