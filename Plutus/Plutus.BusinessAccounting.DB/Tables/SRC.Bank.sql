CREATE TABLE [SRC].[Bank] (
    [FITID]             BIGINT          NOT NULL,
    [PostedDate]        DATETIME        NOT NULL,
    [Amount]            DECIMAL (18, 2) NOT NULL,
    [PayeeFriendlyName] NVARCHAR (256)  NOT NULL,
    [Activity]          NVARCHAR (128)  NOT NULL,
    [TransferType]      NVARCHAR (10)   NOT NULL,
    CONSTRAINT [PK_BankEnriched] PRIMARY KEY CLUSTERED ([FITID] ASC)
);

