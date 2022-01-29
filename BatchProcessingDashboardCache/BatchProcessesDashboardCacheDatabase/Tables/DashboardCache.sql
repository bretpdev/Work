CREATE TABLE [dashcache].[DashboardCache]
(
	[DashboardCacheId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [DashboardItemId] INT NOT NULL, 
    [UheaaProblemCount] INT NULL, 
	[UheaaElapsedTimeInMilliseconds] INT,
	[CornerstoneProblemCount] INT NULL,
	[CornerstoneElapsedTimeInMilliseconds] INT,
    [AddedAt] DATETIME NOT NULL DEFAULT getdate(), 
    [AddedBy] VARCHAR(MAX) NOT NULL DEFAULT SYSTEM_USER, 
    CONSTRAINT [FK_DashboardCache_DashboardItems] FOREIGN KEY ([DashboardItemId]) REFERENCES dashcache.DashboardItems([DashboardItemId])
)
