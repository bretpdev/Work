SELECT
	SC.ServicerCategory,
	SM.ServicerMetric,
	MS.CompliantRecords,
	MS.TotalRecords,
	MS.MetricMonth,
	MS.MetricYear,
	MS.AverageBacklogAge
FROM
	ServicerInventoryMetrics..MetricsSummary MS
	INNER JOIN ServicerInventoryMetrics..ServicerMetrics SM
		ON SM.ServicerMetricsId = MS.ServicerMetricsId
	INNER JOIN ServicerInventoryMetrics..ServicerCategory SC
		ON SC.ServicerCategoryId = SM.ServicerCategoryId
ORDER BY
	MS.MetricYear,
	MS.MetricMonth,
	SC.ServicerCategory,
	SM.ServicerMetric