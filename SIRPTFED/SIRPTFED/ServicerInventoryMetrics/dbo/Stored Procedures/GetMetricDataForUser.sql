CREATE PROCEDURE [dbo].[GetMetricDataForUser]
	@User VARCHAR(50)
AS
	SELECT
		MS.MetricsSummaryId,
		SM.ServicerCategoryId,
		SC.ServicerCategory AS Category,
		MS.ServicerMetricsId,
		SM.ServicerMetric as Metric,
		MS.CompliantRecords,
		MS.TotalRecords,
		MS.MetricMonth,
		MS.MetricYear,
		MS.AverageBacklogAge AS AvgBacklog,
		MS.SuspenseAmount,
		MS.SuspenseTotal
	FROM
		AllowedUsers AU
		INNER JOIN UserMetricMapping UM
			ON UM.AllowedUserId = AU.AllowedUserId
		INNER JOIN ServicerMetrics SM
			ON SM.ServicerMetricsId = UM.ServicerMetricId
		INNER JOIN MetricsSummary MS
			ON MS.ServicerMetricsId = SM.ServicerMetricsId
		INNER JOIN ServicerCategory SC
			ON SC.ServicerCategoryId = SM.ServicerCategoryId
	WHERE 
		AU.AllowedUser = @User
		






RETURN 0