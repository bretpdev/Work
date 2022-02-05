CREATE PROCEDURE [dbo].[GetServicerCategories]
	@User VARCHAR(50)
AS
	SELECT DISTINCT
		SC.ServicerCategoryId,
		SC.ServicerCategory
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
