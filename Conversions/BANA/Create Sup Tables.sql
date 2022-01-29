USE [EA27_BANA]
GO



/****** Object:  Table [dbo].[CompassLoanMapping]    Script Date: 2/2/2016 5:09:29 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[CompassLoanMapping](
	[BorrowerSsn] [char](9) NOT NULL,
	[loan_number] [int] NOT NULL,
	[LN_SEQ] [int] NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

insert into CompassLoanMapping(BorrowerSsn,loan_number)
select distinct borrowerssn, loan_number from _07_08DisbClaimEnrollRecord




