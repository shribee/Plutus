CREATE TABLE [DIM].[Salary] (
    [SalaryDate]      DATETIME        NOT NULL,
    [TaxCode]         NVARCHAR (32)   NULL,
    [TotalPay]        DECIMAL (18, 2) NOT NULL,
    [TaxDeducted]     DECIMAL (18, 2) NOT NULL,
    [EmployeeNIC]     DECIMAL (18, 2) NOT NULL,
    [EmployeePension] DECIMAL (18, 2) NOT NULL,
    [SickPay]         DECIMAL (18, 2) NOT NULL,
    [ParentingPay]    DECIMAL (18, 2) NOT NULL,
    [StudentLoan]     DECIMAL (18, 2) NOT NULL,
    [NetPay]          DECIMAL (18, 2) NOT NULL,
    [EmployerNIC]     DECIMAL (18, 2) NOT NULL,
    [EmployerPension] DECIMAL (18, 2) NOT NULL,
    CONSTRAINT [PK_Salary] PRIMARY KEY CLUSTERED ([SalaryDate] ASC)
);

