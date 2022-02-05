CREATE TABLE [dbo].[Settings](
	[SettingId] [int] NOT NULL,
	[Name] [nvarchar](64) NOT NULL,
	[Value] [sql_variant] NOT NULL,
	[DisplayOrdinal] [int] NOT NULL DEFAULT ((0)),
PRIMARY KEY CLUSTERED 
(
	[SettingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO