CREATE TABLE [MGMT].[InputCatalog] (
    [ID]                INT            IDENTITY (1, 1) NOT NULL,
    [FileType]          NVARCHAR (128) NOT NULL,
    [FileName]          NVARCHAR (128) NOT NULL,
    [ProcessedDateTime] DATETIME       NOT NULL,
    [NumRows]           INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

