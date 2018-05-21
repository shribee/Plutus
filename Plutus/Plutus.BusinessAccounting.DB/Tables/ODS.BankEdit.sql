CREATE TABLE [ODS].[BankEdit] (
    [FITID]           BIGINT          NOT NULL,
    [EffectiveDate]   DATETIME        NOT NULL,
    [EffectiveAmount] DECIMAL (18, 2) NOT NULL,
    CONSTRAINT [PK__BankEdit__694380B0D9B8DBAD] PRIMARY KEY CLUSTERED ([FITID] ASC)
);

