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
		MS.AverageBacklogAge AS AvgBacklog
	FROM
		AllowedUsers AU
		INNER JOIN AllowedUserAccessGroupMapping AUGM
			ON AUGM.AllowedUserId = AU.AllowedUserId
		INNER JOIN AccessGroupMetricMapping AGMM
			ON AGMM.AccessGroupId = AUGM.AccessGroupId
		INNER JOIN ServicerMetrics SM
			ON SM.ServicerMetricsId = AGMM.ServicerMetricsId
		INNER JOIN MetricsSummary MS
			ON MS.ServicerMetricsId = SM.ServicerMetricsId
		INNER JOIN ServicerCategory SC
			ON SC.ServicerCategoryId = SM.ServicerCategoryId
	WHERE 
		AU.AllowedUser = @User
		






RETURN 0
