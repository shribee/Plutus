USE [BusinessAccounting]
GO
/****** Object:  Schema [BA]    Script Date: 04/03/2018 12:17:59 ******/
CREATE SCHEMA [BA]
GO
/****** Object:  Schema [DIM]    Script Date: 04/03/2018 12:17:59 ******/
CREATE SCHEMA [DIM]
GO
/****** Object:  Schema [FACT]    Script Date: 04/03/2018 12:17:59 ******/
CREATE SCHEMA [FACT]
GO
/****** Object:  Schema [MGMT]    Script Date: 04/03/2018 12:17:59 ******/
CREATE SCHEMA [MGMT]
GO
/****** Object:  Schema [New]    Script Date: 04/03/2018 12:17:59 ******/
CREATE SCHEMA [New]
GO
/****** Object:  Schema [ODS]    Script Date: 04/03/2018 12:17:59 ******/
CREATE SCHEMA [ODS]
GO
/****** Object:  Schema [STG]    Script Date: 04/03/2018 12:17:59 ******/
CREATE SCHEMA [STG]
GO
/****** Object:  Table [DIM].[Date]    Script Date: 04/03/2018 12:17:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [DIM].[Date](
	[Date] [datetime] NOT NULL,
	[CalendarYear] [nvarchar](128) NULL,
	[CalendarQuarter] [nvarchar](128) NULL,
	[CalendarMonth] [nvarchar](128) NULL,
	[CalendarMonthNum] [int] NULL,
	[CalendarYearMonth] [nvarchar](128) NULL,
	[CalendarYearMonthDate] [int] NULL,
	[FinYear] [nvarchar](128) NULL,
	[FinQuarter] [nvarchar](128) NULL,
	[FinYearMonth] [nvarchar](128) NULL,
	[CompanyYear] [nvarchar](128) NULL,
	[CompanyQuarter] [nvarchar](128) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [DIM].[DividendThreshold]    Script Date: 04/03/2018 12:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [DIM].[DividendThreshold](
	[ThresholdID] [int] NOT NULL,
	[FinYear] [nvarchar](128) NOT NULL,
	[MinAmount] [decimal](18, 2) NULL,
	[MaxAmount] [decimal](18, 2) NULL,
	[TaxPct] [decimal](18, 2) NULL,
	[IsOptimumBand] [smallint] NULL,
	[NetAmountCarryOver] [decimal](18, 2) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [DIM].[Payee]    Script Date: 04/03/2018 12:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [DIM].[Payee](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PayeeID] [int] NOT NULL,
	[PayeeFriendlyName] [nvarchar](256) NOT NULL,
	[PayeeSourceName] [nvarchar](256) NOT NULL,
	[PayeeSource] [nvarchar](128) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [DIM].[PayeeMatchList]    Script Date: 04/03/2018 12:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [DIM].[PayeeMatchList](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PayeeMatchPattern] [nvarchar](256) NOT NULL,
	[PayeeFriendlyName] [nvarchar](256) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [DIM].[Salary]    Script Date: 04/03/2018 12:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [DIM].[Salary](
	[SalaryID] [int] NOT NULL,
	[Date] [datetime] NULL,
	[TaxCode] [nvarchar](32) NULL,
	[TotalPay] [decimal](18, 2) NOT NULL,
	[TaxDeducted] [decimal](18, 2) NOT NULL,
	[EmployeeNIC] [decimal](18, 2) NOT NULL,
	[EmployeePension] [decimal](18, 2) NOT NULL,
	[SickPay] [decimal](18, 2) NOT NULL,
	[ParentingPay] [decimal](18, 2) NOT NULL,
	[StudentLoan] [decimal](18, 2) NOT NULL,
	[NetPay] [decimal](18, 2) NOT NULL,
	[EmployerNIC] [decimal](18, 2) NOT NULL,
	[EmployerPension] [decimal](18, 2) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [DIM].[TransactionType]    Script Date: 04/03/2018 12:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [DIM].[TransactionType](
	[TransactionTypeID] [int] NOT NULL,
	[Activity] [nvarchar](128) NOT NULL,
	[TransactionCategory] [nvarchar](128) NOT NULL,
	[TransactionSubCategory] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_TransactionType] PRIMARY KEY CLUSTERED 
(
	[TransactionTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [FACT].[Transaction]    Script Date: 04/03/2018 12:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FACT].[Transaction](
	[FITID] [bigint] NOT NULL,
	[PostedDate] [datetime] NOT NULL,
	[TxnTypeID] [int] NOT NULL,
	[PayeeID] [int] NOT NULL,
	[TransactionTypeID] [int] NOT NULL,
	[SalaryID] [int] NOT NULL,
	[InvoiceID] [int] NOT NULL,
	[DividendID] [int] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[Net] [decimal](18, 2) NOT NULL,
	[VAT] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_FactBank] PRIMARY KEY CLUSTERED 
(
	[FITID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [MGMT].[InputCatalog]    Script Date: 04/03/2018 12:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [MGMT].[InputCatalog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FileType] [nvarchar](128) NOT NULL,
	[FileName] [nvarchar](128) NOT NULL,
	[ProcessedDateTime] [datetime] NOT NULL,
	[NumRows] [int] NOT NULL,
	[LastEOMDate] [datetime] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [ODS].[BankCaterAllen]    Script Date: 04/03/2018 12:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ODS].[BankCaterAllen](
	[FITID] [bigint] NOT NULL,
	[PostedDate] [datetime] NOT NULL,
	[TxnType] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](1024) NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_BankCaterAllen] PRIMARY KEY CLUSTERED 
(
	[FITID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [ODS].[BankIntouch]    Script Date: 04/03/2018 12:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ODS].[BankIntouch](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[BankDescription] [nvarchar](1024) NOT NULL,
	[AccountNumber] [nvarchar](128) NULL,
	[EmployeeName] [nvarchar](128) NULL,
	[Description] [nvarchar](1024) NULL,
	[UserComments] [nvarchar](1024) NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[Net] [decimal](18, 2) NULL,
	[VAT] [decimal](18, 2) NULL,
 CONSTRAINT [PK_BankIntouch] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [ODS].[Expense]    Script Date: 04/03/2018 12:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ODS].[Expense](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[EmployeeName] [nvarchar](128) NULL,
	[Description] [nvarchar](1024) NOT NULL,
	[Detail] [nvarchar](1024) NULL,
	[Total] [decimal](18, 2) NOT NULL,
	[Net] [decimal](18, 2) NULL,
	[VAT] [decimal](18, 2) NULL,
 CONSTRAINT [PK_Expense] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [ODS].[Invoice]    Script Date: 04/03/2018 12:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ODS].[Invoice](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Description] [nvarchar](1024) NOT NULL,
	[ContractReference] [nvarchar](1024) NULL,
	[ClientName] [nvarchar](1024) NULL,
	[InvoiceReference] [nvarchar](1024) NULL,
	[Total] [decimal](18, 2) NOT NULL,
	[Net] [decimal](18, 2) NULL,
	[VAT] [decimal](18, 2) NULL,
 CONSTRAINT [PK_Invoice] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
