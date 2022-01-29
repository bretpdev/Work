CREATE PROCEDURE [dbo].[GetMeticsForCategory]
		@User VARCHAR(50),
		@CategoryId INT
AS
	SELECT DISTINCT
		SM.ServicerMetricsId,
		SM.ServicerMetric
	FROM
		AllowedUsers AU
		INNER JOIN AllowedUserAccessGroupMapping AUGM
			ON AUGM.AllowedUserId = AU.AllowedUserId
		INNER JOIN AccessGroupMetricMapping AGMM
			ON AGMM.AccessGroupId = AUGM.AccessGroupId
		INNER JOIN ServicerMetrics SM
			ON SM.ServicerMetricsId = AGMM.ServicerMetricsId
			AND SM.ServicerCategoryId = @CategoryId
		WHERE 
			AU.AllowedUser = @User
		
RETURN 0
