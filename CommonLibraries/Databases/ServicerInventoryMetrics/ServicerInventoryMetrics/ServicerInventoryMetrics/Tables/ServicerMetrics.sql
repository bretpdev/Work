CREATE TABLE [dbo].[ServicerMetrics]
(
	[ServicerMetricsId] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[ServicerCategoryId] INT NOT NULL,
    [ServicerMetric] VARCHAR(256) NOT NULL, 
    [ServicerMetricGoal] VARCHAR(256) NOT NULL, 
    [IsManualUpdate] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_ServicerMetrics_ToTable] FOREIGN KEY ([ServicerCategoryId]) REFERENCES [ServicerCategory]([ServicerCategoryId])
)
