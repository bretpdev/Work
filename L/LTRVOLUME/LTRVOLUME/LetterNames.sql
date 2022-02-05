USE [CentralData]
GO

/****** Object:  Table [dbo].[LetterNames]    Script Date: 4/16/2020 10:33:03 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LetterNames](
	[LetterId] [nvarchar](10) NULL,
	[LetterName] [nvarchar](50) NOT NULL,
	[Description] [ntext] NULL,
	[Status] [nvarchar](50) NOT NULL
) ON [PRIMARY]

GO


