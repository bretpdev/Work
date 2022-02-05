CREATE TABLE [dashcache].[DashboardItems] (
    [DashboardItemId]      INT          IDENTITY (1, 1) NOT NULL,
    [ItemName]             VARCHAR (50) NOT NULL,
    [UheaaSprocName]       VARCHAR (50) NULL,
    [UheaaDatabase]        VARCHAR (50) NULL,
    [CornerstoneSprocName] VARCHAR (50) NULL,
    [CornerstoneDatabase]  VARCHAR (50) NULL,
    [Retired]              BIT          DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([DashboardItemId] ASC) WITH (FILLFACTOR = 95),
    UNIQUE NONCLUSTERED ([ItemName] ASC) WITH (FILLFACTOR = 95)
);

