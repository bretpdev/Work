CREATE TABLE [dbo].[MonthlyMetricsOrder] (
    [MonthlyMetricsOrderId] INT           IDENTITY (1, 1) NOT NULL,
    [ServicerMetric]        VARCHAR (256) NOT NULL,
    CONSTRAINT [PK_MonthlyMetricsOrder] PRIMARY KEY CLUSTERED ([MonthlyMetricsOrderId] ASC) WITH (FILLFACTOR = 95)
);

