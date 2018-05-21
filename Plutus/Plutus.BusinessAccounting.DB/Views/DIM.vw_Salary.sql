CREATE VIEW [DIM].[vw_Salary]
AS
SELECT
[S].[SalaryDate]
, [S].[TotalPay]
, [S].[TaxDeducted]
, [S].[EmployeeNIC]
, [S].[EmployeePension]
, [S].[NetPay]
, [S].[EmployerNIC]
, [S].[EmployerPension]
FROM [DIM].[Salary] S