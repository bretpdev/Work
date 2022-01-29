﻿CREATE TABLE [dbo].[Arcs] (
    [ArcId] INT         IDENTITY (1, 1) NOT NULL,
    [Arc]   VARCHAR (5) NOT NULL,
    PRIMARY KEY CLUSTERED ([ArcId] ASC),
    CONSTRAINT [AK_Arcs_Arc] UNIQUE NONCLUSTERED ([Arc] ASC)
);

