CREATE TABLE [dashcache].[DashboardItems]
(
	[DashboardItemId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ItemName] VARCHAR(50) NOT NULL UNIQUE, 
    [UheaaSprocName] VARCHAR(50) NULL, 
    [UheaaDatabase] VARCHAR(50) NULL, 
	[CornerstoneSprocName] VARCHAR(50) NULL, 
    [CornerstoneDatabase] VARCHAR(50) NULL, 
    [Retired] BIT NOT NULL DEFAULT 0
)
