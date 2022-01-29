CREATE VIEW [dashcache].[DashboardView]
	AS 
	
	SELECT
		DI.ItemName [BatchProcess],
		ISNULL(DC.CornerstoneProblemCount, CASE WHEN DI.CornerstoneSprocName IS NOT NULL THEN 1 ELSE NULL END) CornerstoneProblemCount,
		ISNULL(DC.UheaaProblemCount, CASE WHEN DI.UheaaSprocName IS NOT NULL THEN 1 ELSE NULL END) UheaaProblemCount,
		DC.AddedAt [LastRefresh],
		DC.UheaaElapsedTimeInMilliseconds,
		DC.CornerstoneElapsedTimeInMilliseconds
	FROM 
		dashcache.DashboardItems DI 
		LEFT JOIN
		(
			SELECT
				MAX(DashboardCacheId) DashboardCacheId,
				DashboardItemId
			FROM
				dashcache.DashboardCache
			GROUP BY
				DashboardItemId
		) MostRecent ON MostRecent.DashboardItemId = DI.DashboardItemId
		LEFT JOIN
		dashcache.DashboardCache DC ON DC.DashboardCacheId = MostRecent.DashboardCacheId
	WHERE
		DI.Retired = 0
