create database LoanProcedure;
GO

USE LoanProcedure
GO

CREATE TABLE Customers(
    CustomerId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    NID NVARCHAR(50) UNIQUE NOT NULL,
    Mobile NVARCHAR(20),
    Income DECIMAL(18,2)
);
GO

CREATE TABLE Loans(
    LoanId INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    LoanType NVARCHAR(50) NOT NULL,
    Tenor INT NOT NULL,
    Purpose NVARCHAR(200),
    Status TINYINT NOT NULL DEFAULT 0,
    CreatedDate DATETIME DEFAULT GETDATE(),
    CONSTRAINT CHK_Loan_Status CHECK (Status IN (0,1,2,3)),
);
GO