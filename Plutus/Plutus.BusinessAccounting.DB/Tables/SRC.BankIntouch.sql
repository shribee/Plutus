CREATE TABLE [SRC].[BankIntouch] (
    [ID]              INT             IDENTITY (1, 1) NOT NULL,
    [Date]            DATETIME        NOT NULL,
    [BankDescription] NVARCHAR (1024) NOT NULL,
    [AccountNumber]   NVARCHAR (128)  NULL,
    [EmployeeName]    NVARCHAR (128)  NULL,
    [Description]     NVARCHAR (1024) NULL,
    [UserComments]    NVARCHAR (1024) NULL,
    [Amount]          DECIMAL (18, 2) NOT NULL,
    [Net]             DECIMAL (18, 2) NULL,
    [VAT]             DECIMAL (18, 2) NULL,
    CONSTRAINT [PK_BankIntouch] PRIMARY KEY CLUSTERED ([ID] ASC)
);

