CREATE VIEW DIM.vw_TransactionType
AS
SELECT 
[TT].[TransactionTypeID]
, [TT].[Activity]
, [TT].[TransactionCategory]
, [TT].[TransactionSubCategory]
FROM [DIM].[TransactionType] TT