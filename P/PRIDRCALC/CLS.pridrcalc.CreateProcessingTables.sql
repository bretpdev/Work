USE CLS
GO

CREATE SCHEMA [pridrcalc]
GRANT EXECUTE ON SCHEMA::[pridrcalc] TO db_executor

GO

CREATE TABLE [pridrcrp].[PreQualifyingPayments](
	[PreQualifyingPaymentsId] [int] IDENTITY(1,1) NOT NULL,
	[BF_SSN] [char](9) NOT NULL,
	[LN_SEQ] [int] NOT NULL,
	[ScheduleCode] [varchar](10) NOT NULL,
	[PaymentsQualifyingLevelPrevious] [int] NOT NULL,
	[PaymentsQualifyingIDRPrevious] [int] NOT NULL,
	[PaymentsQualifyingPermanentStandardPrevious] [int] NOT NULL,
	[PaymentsCoveredByEHDPre] [int] NOT NULL,
	[Total] [int] NOT NULL,
	[DeletedAt] DATETIME NULL,
	[DeletedBy] VARCHAR(50) NULL
 CONSTRAINT [PK_pridrcrp.PreQualifyingPayments] PRIMARY KEY CLUSTERED 
(
	[PreQualifyingPaymentsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY]