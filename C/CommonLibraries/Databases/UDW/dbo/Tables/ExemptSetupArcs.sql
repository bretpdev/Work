CREATE TABLE [dbo].[ExemptSetupArcs] (
    [ExemptSetupArcId] INT      IDENTITY (1, 1) NOT NULL,
    [ARC]              CHAR (5) NOT NULL,
    [Queue]            CHAR (2) NULL,
    PRIMARY KEY CLUSTERED ([ExemptSetupArcId] ASC)
);

