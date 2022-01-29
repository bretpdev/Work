CREATE TABLE [dbo].[MetricsSummary] (
    [MetricsSummaryId]  INT           IDENTITY (1, 1) NOT NULL,
    [ServicerMetricsId] INT           NOT NULL,
    [CompliantRecords]  INT           NOT NULL,
    [TotalRecords]      INT           NOT NULL,
    [MetricMonth]       TINYINT       NOT NULL,
    [MetricYear]        SMALLINT      NOT NULL,
    [AverageBacklogAge] INT           NULL,
	[SuspenseAmount]	DECIMAL(14,2) NULL,
	[SuspenseTotal]		DECIMAL(14,2) NULL,
    [UpdatedAt]         DATETIME      CONSTRAINT [DF__MetricsSu__Updat__5AEE82B9] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]         VARCHAR (100) CONSTRAINT [DF__MetricsSu__Updat__0D7A0286] DEFAULT (suser_sname()) NOT NULL,
    CONSTRAINT [PK__MetricsS__69A3878630F848ED] PRIMARY KEY CLUSTERED ([MetricsSummaryId] ASC),
    CONSTRAINT [FK_ServicerMetrics] FOREIGN KEY ([ServicerMetricsId]) REFERENCES [dbo].[ServicerMetrics] ([ServicerMetricsId])
);

