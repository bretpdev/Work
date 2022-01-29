CREATE TABLE [fp].[Arcs] (
    [ArcId]   INT            IDENTITY (1, 1) NOT NULL,
    [Arc]     VARCHAR (5)    NOT NULL,
    [Comment] VARCHAR (1200) NULL,
    PRIMARY KEY CLUSTERED ([ArcId] ASC)
);

