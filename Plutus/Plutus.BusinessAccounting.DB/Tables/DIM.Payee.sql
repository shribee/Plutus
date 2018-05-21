CREATE TABLE [DIM].[Payee] (
    [PayeeID]           INT            NOT NULL,
    [PayeeFriendlyName] NVARCHAR (256) NOT NULL,
    CONSTRAINT [PK_Payee] PRIMARY KEY CLUSTERED ([PayeeID] ASC)
);

