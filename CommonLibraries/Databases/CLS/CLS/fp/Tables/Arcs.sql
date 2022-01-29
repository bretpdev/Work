CREATE TABLE [fp].[Arcs] (
    [ArcId]   INT            NOT NULL IDENTITY,
    [Arc]     VARCHAR (5)    NOT NULL,
    [Comment] VARCHAR (1200) NULL,
    PRIMARY KEY CLUSTERED ([ArcId] ASC)
);

