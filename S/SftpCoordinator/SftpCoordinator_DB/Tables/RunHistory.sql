CREATE TABLE [dbo].[RunHistory](
	[RunHistoryId] [int] IDENTITY(1,1) NOT NULL,
	[StartedOn] [datetime] NOT NULL DEFAULT (getdate()),
	[EndedOn] [datetime] NULL,
	[RunBy] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RunHistoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO