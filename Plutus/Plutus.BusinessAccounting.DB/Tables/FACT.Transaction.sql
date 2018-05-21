CREATE TABLE [FACT].[Transaction] (
    [TransactionID]     INT             IDENTITY (1, 1) NOT NULL,
    [SourceID]          NVARCHAR (50)   NOT NULL,
    [PostedDate]        DATETIME        NOT NULL,
    [EffectiveDate]     DATETIME        NOT NULL,
    [TransactionTypeID] INT             NOT NULL,
    [PayeeID]           INT             NOT NULL,
    [TransferType]      NVARCHAR (6)    NOT NULL,
    [Amount]            DECIMAL (18, 2) NOT NULL,
    [EffectiveAmount]   DECIMAL (18, 2) NOT NULL,
    [TransactionSource] NVARCHAR (10)   NOT NULL,
    CONSTRAINT [PK__Transact__55433A4B42E0188F] PRIMARY KEY CLUSTERED ([TransactionID] ASC)
);

