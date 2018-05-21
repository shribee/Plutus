CREATE TABLE [SRC].[BankCaterAllen] (
    [FITID]      BIGINT          NOT NULL,
    [PostedDate] DATETIME        NOT NULL,
    [TxnType]    NVARCHAR (128)  NOT NULL,
    [Name]       NVARCHAR (1024) NOT NULL,
    [Amount]     DECIMAL (18, 2) NOT NULL,
    CONSTRAINT [PK_BankCaterAllen] PRIMARY KEY CLUSTERED ([FITID] ASC)
);

