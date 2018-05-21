CREATE VIEW DIM.vw_Payee
AS
SELECT
[DIM].[Payee].[PayeeID], [DIM].[Payee].[PayeeFriendlyName]
FROM DIM.Payee