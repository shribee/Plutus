CREATE TABLE [SRC].[Expense] (
    [ID]           INT             IDENTITY (1, 1) NOT NULL,
    [Date]         DATETIME        NOT NULL,
    [EmployeeName] NVARCHAR (128)  NULL,
    [Description]  NVARCHAR (1024) NOT NULL,
    [Detail]       NVARCHAR (1024) NULL,
    [Total]        DECIMAL (18, 2) NOT NULL,
    [Net]          DECIMAL (18, 2) NULL,
    [VAT]          DECIMAL (18, 2) NULL,
    CONSTRAINT [PK_Expense] PRIMARY KEY CLUSTERED ([ID] ASC)
);

