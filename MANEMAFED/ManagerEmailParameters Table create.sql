USE [CLS]
GO

/****** Object:  Table [dbo].[ManagerEmailParameters]    Script Date: 11/13/2018 2:43:10 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ManagerEmailParameters](
	[ManagerEmailParameterId] [int] IDENTITY(1,1) NOT NULL,
	[DelinquencyLowerLimit] [int] NOT NULL CONSTRAINT [DF_ManagerEmailParameters_DelinquencyLowerLimit]  DEFAULT ((15)),
	[DelinquencyUpperLimit] [int] NOT NULL CONSTRAINT [DF_ManagerEmailParameters_DelinquencyUpperLimit]  DEFAULT ((40)),
	[MaxEmails] [int] NOT NULL CONSTRAINT [DF_ManagerEmailParameters_MaxEmails]  DEFAULT ((20000)),
	[AddedAt] [datetime] NOT NULL CONSTRAINT [DF_ManagerEmailParameters_AddedAt]  DEFAULT (getdate()),
	[AddedBy] [varchar](100) NOT NULL CONSTRAINT [DF_ManagerEmailParameters_AddedBy]  DEFAULT (suser_name()),
 CONSTRAINT [PK_ManagerEmailParameters] PRIMARY KEY CLUSTERED 
(
	[ManagerEmailParameterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING ON
GO


