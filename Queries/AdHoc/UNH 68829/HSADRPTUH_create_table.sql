USE [ULS]
GO
CREATE SCHEMA [hsadrptuh] AUTHORIZATION dbo;
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [hsadrptuh].[DataComparison](
	[DataComparisonId] [int] IDENTITY(1,1) NOT NULL,
	[ActiveRow] [bit] NULL,
	[Loan] [smallint] NULL,
	[StatusDate] [date] NULL,
	[BorrSSN] [char](9) NULL,
	[BorrActive] [bit] NULL,
	[EndrSSN] [char](9) NULL,
	[EndrActive] [bit] NULL,
	[BeginBrwr] [date] NULL,
	[EIDB] [bit] NULL,
	[BeginEndr] [date] NULL,
	[EIDE] [bit] NULL,
	[EndBrwr] [date] NULL,
	[EndEndr] [date] NULL,
	[BorrIsReservist] [bit] NULL,
	[EndrIsReservist] [bit] NULL,
	[CreatedAt] [datetime] NULL,
	[LoanBalance] [numeric](8, 2) NULL,
	[ServiceComponent] [char](2) NULL,
	[EIDServiceComponent] [char](2) NULL,
	[EndorserServiceComponent] [char](2) NULL,
	[EndorserEIDServiceComponent] [char](2) NULL,
	[ActiveBeginBrwr] [date] NULL,
	[ActiveEndBrwr] [date] NULL,
	[ActiveBeginEndr] [date] NULL,
	[ActiveEndEndr] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[DataComparisonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [hsadrptuh].[ScraHistoricalFiles](
	[DODReturnFileId] [int] IDENTITY(1,1) NOT NULL,
	[SourceFile] [varchar](1000) NULL,
	[SSN] [char](9) NOT NULL,
	[DateOfBirth] [numeric](8, 0) NULL,
	[LastName] [varchar](26) NULL,
	[FirstName] [varchar](20) NULL,
	[CustomerRecordID] [varchar](28) NULL,
	[ActiveDutyStatusDate] [numeric](8, 0) NULL,
	[Blank] [varchar](1) NULL,
	[ActiveDutyOnActiveDutyStatusDt] [varchar](1) NULL,
	[LeftActiveDutyLE367DaysFromActiveDutyStatusDt] [varchar](1) NULL,
	[NotifiedOfActiveDutyRecallOnActiveDutyStatusDt] [varchar](1) NULL,
	[ActiveDutyEndDate] [numeric](8, 0) NULL,
	[MatchResultCode] [numeric](1, 0) NULL,
	[Error] [numeric](1, 0) NULL,
	[DateOfMatch] [numeric](8, 0) NULL,
	[ActiveDutyBeginDate] [numeric](8, 0) NULL,
	[EIDBeginDate] [numeric](8, 0) NULL,
	[EIDEndDate] [numeric](8, 0) NULL,
	[ServiceComponent] [varchar](2) NULL,
	[EIDServiceComponent] [varchar](2) NULL,
	[MiddleName] [varchar](20) NULL,
	[CertificateID] [varchar](15) NULL,
PRIMARY KEY CLUSTERED 
(
	[DODReturnFileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [hsadrptuh].[ScraHistoricalFilesErrors](
	[DODReturnFileErrorsId] [int] IDENTITY(1,1) NOT NULL,
	[SourceFile] [varchar](1000) NULL,
	[LineData] [varchar](255) NULL,
	[ErrorCode] [int] NULL,
	[ErrorColumn] [int] NULL,
	[ErrorDescription] [varchar](255) NULL,
	[DateRun] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[DODReturnFileErrorsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY]

GO


