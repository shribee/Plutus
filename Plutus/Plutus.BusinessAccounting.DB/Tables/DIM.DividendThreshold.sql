CREATE TABLE [DIM].[DividendThreshold] (
    [ThresholdID]        INT             NOT NULL,
    [FinYear]            NVARCHAR (128)  NOT NULL,
    [MinAmount]          DECIMAL (18, 2) NULL,
    [MaxAmount]          DECIMAL (18, 2) NULL,
    [TaxPct]             DECIMAL (18, 2) NULL,
    [IsOptimumBand]      SMALLINT        NULL,
    [NetAmountCarryOver] DECIMAL (18, 2) NULL,
    CONSTRAINT [PK_DividendThreshold] PRIMARY KEY CLUSTERED ([ThresholdID] ASC)
);

