CREATE PROCEDURE [dbo].[InsertMetricSummaryData]
	@ServicerMetricId INT,
	@ServicerCategoryId INT,
	@MetricMonth INT,
	@MetricYear INT,
	@ComplaintRecords INT,
	@TotalRecords INT,
	@AvgBacklog INT,
	@SuspenseAmount DECIMAL(14,2) NULL,
	@SuspenseTotal DECIMAL(14,2) NULL
AS
	
	IF EXISTS (SELECT MS.MetricsSummaryId FROM MetricsSummary MS INNER JOIN ServicerMetrics SM ON SM.ServicerMetricsId = MS.ServicerMetricsId WHERE SM.ServicerCategoryId = @ServicerCategoryId AND MS.ServicerMetricsId = @ServicerMetricId AND MS.MetricYear = @MetricYear AND MS.MetricMonth = @MetricMonth)
	BEGIN
		RAISERROR('THE DATA YOU ARE TRYING TO ENTER ALREADY EXISTS',11,-1)
	END
	ELSE
	BEGIN
		INSERT INTO MetricsSummary(ServicerMetricsId, MetricMonth, MetricYear, CompliantRecords, TotalRecords, AverageBacklogAge, SuspenseAmount, SuspenseTotal)
		VALUES(@ServicerMetricId, @MetricMonth, @MetricYear, @ComplaintRecords, @TotalRecords, @AvgBacklog, @SuspenseAmount, @SuspenseTotal)
	END
RETURN 0