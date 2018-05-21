CREATE TABLE [ODS].[PayeeMapping] (
    [ID]                INT            NOT NULL,
    [PayeeID]           INT            NOT NULL,
    [PayeeFriendlyName] NVARCHAR (256) NOT NULL,
    [PayeeSourceName]   NVARCHAR (256) NOT NULL,
    [PayeeSource]       NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_PayeeMapping] PRIMARY KEY CLUSTERED ([ID] ASC)
);

