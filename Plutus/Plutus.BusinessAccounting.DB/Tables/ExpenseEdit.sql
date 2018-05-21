CREATE TABLE [ODS].[ExpenseEdit]
(
[ID] [int] NOT NULL IDENTITY(1, 1),
[Date] [datetime] NOT NULL,
[Description] [nvarchar] (1024) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Detail] [nvarchar] (1024) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Total] [decimal] (18, 2) NOT NULL,
[EffectiveDate] [datetime] NOT NULL,
[EffectiveDescription] [nvarchar] (1024) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [ODS].[ExpenseEdit] ADD CONSTRAINT [PK__ExpenseE__3214EC273963BFFB] PRIMARY KEY CLUSTERED  ([ID]) ON [PRIMARY]
GO
;

