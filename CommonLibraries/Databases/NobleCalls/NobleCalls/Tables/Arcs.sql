﻿CREATE TABLE [dbo].[Arcs]
(
	[ArcId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Arc] VARCHAR(5) NOT NULL, 
    CONSTRAINT [AK_Arcs_Arc] UNIQUE (Arc)
)
