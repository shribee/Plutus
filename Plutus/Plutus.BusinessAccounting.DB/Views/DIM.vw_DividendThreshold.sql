CREATE VIEW DIM.vw_DividendThreshold
AS
SELECT 
[DT].[ThresholdID]
, [DT].[FinYear]
, [DT].[MinAmount]
, [DT].[MaxAmount]
, [DT].[TaxPct]
, [DT].[IsOptimumBand]
, [DT].[NetAmountCarryOver]
FROM DIM.DividendThreshold DT