CREATE TABLE [dashcache].[DashboardCache] (
    [DashboardCacheId]                     INT           IDENTITY (1, 1) NOT NULL,
    [DashboardItemId]                      INT           NOT NULL,
    [UheaaProblemCount]                    INT           NULL,
    [UheaaElapsedTimeInMilliseconds]       INT           NULL,
    [CornerstoneProblemCount]              INT           NULL,
    [CornerstoneElapsedTimeInMilliseconds] INT           NULL,
    [AddedAt]                              DATETIME      DEFAULT (getdate()) NOT NULL,
    [AddedBy]                              VARCHAR (MAX) DEFAULT (suser_sname()) NOT NULL,
    PRIMARY KEY CLUSTERED ([DashboardCacheId] ASC) WITH (FILLFACTOR = 95),
    CONSTRAINT [FK_DashboardCache_DashboardItems] FOREIGN KEY ([DashboardItemId]) REFERENCES [dashcache].[DashboardItems] ([DashboardItemId])
);

