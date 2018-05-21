CREATE TABLE [DIM].[TransactionType] (
    [TransactionTypeID]      INT            NOT NULL,
    [Activity]               NVARCHAR (128) NOT NULL,
    [TransactionCategory]    NVARCHAR (128) NOT NULL,
    [TransactionSubCategory] NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_TransactionType] PRIMARY KEY CLUSTERED ([TransactionTypeID] ASC)
);

