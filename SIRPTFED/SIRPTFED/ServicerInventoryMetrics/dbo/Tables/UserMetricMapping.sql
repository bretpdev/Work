CREATE TABLE [dbo].[UserMetricMapping] (
    [UserMetricMappingId] INT IDENTITY (1, 1) NOT NULL,
    [AllowedUserId]       INT NOT NULL,
    [CategoryId]          INT NOT NULL,
    [ServicerMetricId]    INT NOT NULL,
    CONSTRAINT [FK_AllowedUserId] FOREIGN KEY ([AllowedUserId]) REFERENCES [dbo].[AllowedUsers] ([AllowedUserId]),
    CONSTRAINT [FK_ServicerMetricId] FOREIGN KEY ([ServicerMetricId]) REFERENCES [dbo].[ServicerMetrics] ([ServicerMetricsId])
);

