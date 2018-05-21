CREATE VIEW [FACT].[vw_Transaction] AS
SELECT 
[T].[TransactionID]
, [T].[SourceID]
, [T].[PostedDate]
, [T].[EffectiveDate]
, [T].[TransactionTypeID]
, [T].[PayeeID]
, [T].[TransferType]
, [T].[Amount]
, [T].[EffectiveAmount]
, [T].[TransactionSource]
FROM [FACT].[Transaction] T