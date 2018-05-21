CREATE TABLE [SRC].[Invoice] (
    [ID]                INT             IDENTITY (1, 1) NOT NULL,
    [Date]              DATETIME        NOT NULL,
    [Description]       NVARCHAR (1024) NOT NULL,
    [ContractReference] NVARCHAR (1024) NULL,
    [ClientName]        NVARCHAR (1024) NULL,
    [InvoiceReference]  NVARCHAR (1024) NULL,
    [Total]             DECIMAL (18, 2) NOT NULL,
    [Net]               DECIMAL (18, 2) NULL,
    [VAT]               DECIMAL (18, 2) NULL,
    CONSTRAINT [PK_Invoice] PRIMARY KEY CLUSTERED ([ID] ASC)
);

