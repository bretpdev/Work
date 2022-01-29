CREATE TABLE [print].[Arcs] (
    [ArcId] INT         IDENTITY (1, 1) NOT NULL,
    [Arc]   VARCHAR (5) NOT NULL,
    PRIMARY KEY CLUSTERED ([ArcId] ASC) WITH (FILLFACTOR = 95)
);

