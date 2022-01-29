CREATE TABLE [dbo].[Regions] (
    [RegionId]   INT          IDENTITY (1, 1) NOT NULL,
    [RegionName] VARCHAR (25) NULL,
    CONSTRAINT [PK_Regions] PRIMARY KEY CLUSTERED ([RegionId] ASC) WITH (FILLFACTOR = 95)
);

