CREATE TABLE [ODS].[ExpenseEdit] (
    [ID]                   INT             IDENTITY (1, 1) NOT NULL,
    [Date]                 DATETIME        NOT NULL,
    [Description]          NVARCHAR (1024) NOT NULL,
    [Detail]               NVARCHAR (1024) NOT NULL,
    [Total]                DECIMAL (18, 2) NOT NULL,
    [EffectiveDate]        DATETIME        NOT NULL,
    [EffectiveDescription] NVARCHAR (1024) NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

