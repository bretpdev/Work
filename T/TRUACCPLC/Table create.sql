USE [OLS]
GO
/****** Object:  Table [dbo].[trueaccord.Placements]    Script Date: 12/8/2020 11:12:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE SCHEMA [trueaccord];
GO
GRANT EXECUTE ON SCHEMA :: [trueaccord] to db_executor;

CREATE TABLE [trueaccord].[Placements](
	[TrueAccordPlacementId] [int] IDENTITY(1,1) NOT NULL,
	[AccountNumber] [varchar](10) NOT NULL,
	[AF_APL_ID] [varchar](17) NOT NULL,
	[AF_APL_ID_SFX] [varchar](2) NOT NULL,
	[AccountNamespace] [varchar](10) NOT NULL,
	[AccountRelationship] [varchar](10) NOT NULL,
	[AccountOpenDate] [date] NOT NULL,
	[BrandId] [varchar](10) NOT NULL,
	[ProductType] [varchar](50) NOT NULL,
	[DateAssigned] [date] NOT NULL,
	[ExpectedRetractionDate] [date] NOT NULL,
	[TransactionDate] [date] NOT NULL,
	[FirstName] [varchar](12) NOT NULL,
	[LastName] [varchar](35) NOT NULL,
	[MiddleName] [char](1) NOT NULL,
	[Prefix] [varchar](20) NOT NULL,
	[Suffix] [varchar](20) NOT NULL,
	[EmailAddress1] [varchar](56) NOT NULL,
	[EmailAddress2] [varchar](56) NOT NULL,
	[EmailAddress3] [varchar](56) NOT NULL,
	[Telephone1] [varchar](17) NOT NULL,
	[Telephone2] [varchar](17) NOT NULL,
	[Telephone3] [varchar](17) NOT NULL,
	[AddressLine1] [varchar](35) NOT NULL,
	[AddressLine2] [varchar](35) NOT NULL,
	[AddressLine3] [varchar](35) NOT NULL,
	[AddressType] [varchar](4) NOT NULL,
	[City] [varchar](30) NOT NULL,
	[State] [varchar](2) NOT NULL,
	[ZipCode] [varchar](14) NOT NULL,
	[TotalAmountDue] [numeric](11, 2) NOT NULL,
	[CurrentAmountDue] [numeric](11, 2) NOT NULL,
	[CurrentBalance] [numeric](11, 2) NOT NULL,
	[TotalDelinquentAmount] [numeric](11, 2) NOT NULL,
	[DelinquencyDate] [date] NOT NULL,
	[CyclesDelinquent] [int] NULL,
	[LastPaymentAmount] [numeric](9, 2) NULL,
	[LastPaymentDate] [date] NULL,
	[MonthToDateFeesPaid] [numeric](11, 2) NOT NULL,
	[MonthToDateInterestPaid] [numeric](11, 2) NOT NULL,
	[MonthToDatePrincipalPaid] [numeric](11, 2) NOT NULL,
	[PlacementNumber] [int] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[RetractedAt] [datetime] NULL,
	[RetractedBy] [varchar](50) NULL,
	[DeletedAt] [datetime] NULL,
	[DeletedBy] [varchar](50) NULL,
 CONSTRAINT [PK_Placements] PRIMARY KEY CLUSTERED 
(
	[TrueAccordPlacementId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY]

GO


