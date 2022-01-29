CREATE TABLE [print].[Arcs] (
    [ArcId]           INT         IDENTITY (1, 1) NOT NULL,
    [Arc]             VARCHAR (5) NOT NULL,
    [ActivityType]    VARCHAR (2) NULL,
    [ActivityContact] VARCHAR (2) NULL,
    PRIMARY KEY CLUSTERED ([ArcId] ASC)
);

