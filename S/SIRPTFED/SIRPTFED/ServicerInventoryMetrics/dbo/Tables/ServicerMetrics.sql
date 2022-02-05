CREATE TABLE [dbo].[ServicerMetrics] (
    [ServicerMetricsId]  INT           IDENTITY (1, 1) NOT NULL,
    [ServicerCategoryId] INT           NOT NULL,
    [ServicerMetric]     VARCHAR (256) NOT NULL,
    [ServicerMetricGoal] VARCHAR (256) NOT NULL,
    [IsManualUpdate]     BIT           DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([ServicerMetricsId] ASC),
    CONSTRAINT [FK_ServicerMetrics_ToTable] FOREIGN KEY ([ServicerCategoryId]) REFERENCES [dbo].[ServicerCategory] ([ServicerCategoryId])
);

