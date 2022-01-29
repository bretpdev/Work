﻿CREATE TABLE [dbo].[Regions]
(
	[RegionId] [int] IDENTITY(1,1) NOT NULL,
	[Region] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RegionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]