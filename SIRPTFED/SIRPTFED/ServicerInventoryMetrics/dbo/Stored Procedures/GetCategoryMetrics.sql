CREATE PROCEDURE [dbo].[GetCategoryMetrics]
	@AllowedUserId INT,
	@CategoryId INT
AS
	SELECT DISTINCT
		SM.ServicerMetricsId,
		SM.ServicerMetric
	FROM
		ServicerMetrics SM
		INNER JOIN 
		UserMetricMapping UM
			ON  UM.AllowedUserId = @AllowedUserId
			AND SM.ServicerMetricsId = UM.ServicerMetricId
			AND UM.CategoryId = @CategoryId

RETURN 0