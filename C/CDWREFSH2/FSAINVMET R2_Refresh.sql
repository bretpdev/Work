USE [CDW]
GO

INSERT INTO [FsaInvMet].[R2]
           ([ReportDate]
           ,[Metric]
           ,[Segment1BorrowerCount]
           ,[Segment1TotalCount]
           ,[Segment1Percentage]
           ,[Segment1MetricPercentage]
           ,[Segment2BorrowerCount]
           ,[Segment2TotalCount]
           ,[Segment2Percentage]
           ,[Segment2MetricPercentage]
           ,[Segment3BorrowerCount]
           ,[Segment3TotalCount]
           ,[Segment3Percentage]
           ,[Segment3MetricPercentage]
           ,[Segment4BorrowerCount]
           ,[Segment4TotalCount]
           ,[Segment4Percentage]
           ,[Segment4MetricPercentage]
           ,[Segment5BorrowerCount]
           ,[Segment5TotalCount]
           ,[Segment5Percentage]
           ,[Segment5MetricPercentage]
           ,[Segment6BorrowerCount]
           ,[Segment6TotalCount]
           ,[Segment6Percentage]
           ,[Segment6MetricPercentage]
           ,[MetricTotal]
           ,[CreatedAt]
           ,[CreatedBy])
SELECT
	POP.ReportDate,
	POP.Metric,
	MAX
	(
		CASE 
			WHEN POP.Metric = 'Metric1' AND POP.Segment = 1 THEN POP.BorrowerCount 
			WHEN POP.Metric = 'Metric2' AND POP.Segment = 1 THEN POP.BorrowerCount  
			WHEN POP.Metric = 'Metric3' AND POP.Segment = 1 THEN POP.BorrowerCount 
			WHEN POP.Metric = 'Other' AND POP.Segment = 1 THEN POP.BorrowerCount 
		END
	) AS Segment1BorrowerCount,
	MAX
	(
		CASE 
			WHEN POP.Metric = 'Metric1' AND POP.Segment = 1 THEN POP.SegmentTotal 
			WHEN POP.Metric = 'Metric2' AND POP.Segment = 1 THEN POP.SegmentTotal 
			WHEN POP.Metric = 'Metric3' AND POP.Segment = 1 THEN POP.SegmentTotal  
			WHEN POP.Metric = 'Other' AND POP.Segment = 1 THEN POP.SegmentTotal 
		END
	) AS Segment1TotalCount,
	MAX
	(
		CASE 
			WHEN POP.Metric = 'Metric1' AND POP.Segment = 1 THEN POP.SegmentPercentage 
			WHEN POP.Metric = 'Metric2' AND POP.Segment = 1 THEN POP.SegmentPercentage  
			WHEN POP.Metric = 'Metric3' AND POP.Segment = 1 THEN POP.SegmentPercentage 
			WHEN POP.Metric = 'Other' AND POP.Segment = 1 THEN POP.SegmentPercentage 
		END
	) AS Segment1Percentage,
	MAX
	(
		CASE 
			WHEN POP.Metric = 'Metric1' AND POP.Segment = 1 THEN POP.MetricPercentage 
			WHEN POP.Metric = 'Metric2' AND POP.Segment = 1 THEN POP.MetricPercentage  
			WHEN POP.Metric = 'Metric3' AND POP.Segment = 1 THEN POP.MetricPercentage 
			WHEN POP.Metric = 'Other' AND POP.Segment = 1 THEN POP.MetricPercentage 
		END
	) AS Segment1MetricPercentage,
	MAX
	(
		CASE 
			WHEN POP.Metric = 'Metric1' AND POP.Segment = 2 THEN POP.BorrowerCount 
			WHEN POP.Metric = 'Metric2' AND POP.Segment = 2 THEN POP.BorrowerCount  
			WHEN POP.Metric = 'Metric3' AND POP.Segment = 2 THEN POP.BorrowerCount 
			WHEN POP.Metric = 'Other' AND POP.Segment = 2 THEN POP.BorrowerCount 
		END
	) AS Segment2BorrowerCount,
	MAX
	(
		CASE 
			WHEN POP.Metric = 'Metric1' AND POP.Segment = 2 THEN POP.SegmentTotal 
			WHEN POP.Metric = 'Metric2' AND POP.Segment = 2 THEN POP.SegmentTotal 
			WHEN POP.Metric = 'Metric3' AND POP.Segment = 2 THEN POP.SegmentTotal  
			WHEN POP.Metric = 'Other' AND POP.Segment = 2 THEN POP.SegmentTotal 
		END
	) AS Segment2TotalCount,
	MAX
	(
		CASE 
			WHEN POP.Metric = 'Metric1' AND POP.Segment = 2 THEN POP.SegmentPercentage 
			WHEN POP.Metric = 'Metric2' AND POP.Segment = 2 THEN POP.SegmentPercentage  
			WHEN POP.Metric = 'Metric3' AND POP.Segment = 2 THEN POP.SegmentPercentage 
			WHEN POP.Metric = 'Other' AND POP.Segment = 2 THEN POP.SegmentPercentage 
		END
	) AS Segment2Percentage,
	MAX
	(
		CASE 
			WHEN POP.Metric = 'Metric1' AND POP.Segment = 2 THEN POP.MetricPercentage 
			WHEN POP.Metric = 'Metric2' AND POP.Segment = 2 THEN POP.MetricPercentage  
			WHEN POP.Metric = 'Metric3' AND POP.Segment = 2 THEN POP.MetricPercentage 
			WHEN POP.Metric = 'Other' AND POP.Segment = 2 THEN POP.MetricPercentage 
		END
	) AS Segment2MetricPercentage,
	MAX
	(
		CASE 
			WHEN POP.Metric = 'Metric1' AND POP.Segment = 3 THEN POP.BorrowerCount 
			WHEN POP.Metric = 'Metric2' AND POP.Segment = 3 THEN POP.BorrowerCount  
			WHEN POP.Metric = 'Metric3' AND POP.Segment = 3 THEN POP.BorrowerCount 
			WHEN POP.Metric = 'Other' AND POP.Segment = 3 THEN POP.BorrowerCount 
		END
	) AS Segment3BorrowerCount,
	MAX
	(
		CASE 
			WHEN POP.Metric = 'Metric1' AND POP.Segment = 3 THEN POP.SegmentTotal 
			WHEN POP.Metric = 'Metric2' AND POP.Segment = 3 THEN POP.SegmentTotal 
			WHEN POP.Metric = 'Metric3' AND POP.Segment = 3 THEN POP.SegmentTotal  
			WHEN POP.Metric = 'Other' AND POP.Segment = 3 THEN POP.SegmentTotal 
		END
	) AS Segment3TotalCount,
	MAX
	(
		CASE 
			WHEN POP.Metric = 'Metric1' AND POP.Segment = 3 THEN POP.SegmentPercentage 
			WHEN POP.Metric = 'Metric2' AND POP.Segment = 3 THEN POP.SegmentPercentage  
			WHEN POP.Metric = 'Metric3' AND POP.Segment = 3 THEN POP.SegmentPercentage 
			WHEN POP.Metric = 'Other' AND POP.Segment = 3 THEN POP.SegmentPercentage 
		END
	) AS Segment3Percentage,
	MAX
	(
		CASE 
			WHEN POP.Metric = 'Metric1' AND POP.Segment = 3 THEN POP.MetricPercentage 
			WHEN POP.Metric = 'Metric2' AND POP.Segment = 3 THEN POP.MetricPercentage  
			WHEN POP.Metric = 'Metric3' AND POP.Segment = 3 THEN POP.MetricPercentage 
			WHEN POP.Metric = 'Other' AND POP.Segment = 3 THEN POP.MetricPercentage 
		END
	) AS Segment3MetricPercentage,
	MAX
	(
		CASE 
			WHEN POP.Metric = 'Metric1' AND POP.Segment = 4 THEN POP.BorrowerCount 
			WHEN POP.Metric = 'Metric2' AND POP.Segment = 4 THEN POP.BorrowerCount  
			WHEN POP.Metric = 'Metric3' AND POP.Segment = 4 THEN POP.BorrowerCount 
			WHEN POP.Metric = 'Other' AND POP.Segment = 4 THEN POP.BorrowerCount 
		END
	) AS Segment4BorrowerCount,
	MAX
	(
		CASE 
			WHEN POP.Metric = 'Metric1' AND Segment = 4 THEN POP.SegmentTotal 
			WHEN POP.Metric = 'Metric2' AND Segment = 4 THEN POP.SegmentTotal 
			WHEN POP.Metric = 'Metric3' AND Segment = 4 THEN POP.SegmentTotal  
			WHEN POP.Metric = 'Other' AND Segment = 4 THEN POP.SegmentTotal 
		END
	) AS Segment4TotalCount,
	MAX
	(
		CASE 
			WHEN POP.Metric = 'Metric1' AND POP.Segment = 4 THEN POP.SegmentPercentage 
			WHEN POP.Metric = 'Metric2' AND POP.Segment = 4 THEN POP.SegmentPercentage  
			WHEN POP.Metric = 'Metric3' AND POP.Segment = 4 THEN POP.SegmentPercentage 
			WHEN POP.Metric = 'Other' AND POP.Segment = 4 THEN POP.SegmentPercentage 
		END
	) AS Segment4Percentage,
	MAX
	(
		CASE 
			WHEN POP.Metric = 'Metric1' AND POP.Segment = 4 THEN POP.MetricPercentage 
			WHEN POP.Metric = 'Metric2' AND POP.Segment = 4 THEN POP.MetricPercentage  
			WHEN POP.Metric = 'Metric3' AND POP.Segment = 4 THEN POP.MetricPercentage 
			WHEN POP.Metric = 'Other' AND POP.Segment = 4 THEN POP.MetricPercentage 
		END
	) AS Segment4MetricPercentage,
	MAX
	(
		CASE 
			WHEN POP.Metric = 'Metric1' AND POP.Segment = 5 THEN POP.BorrowerCount 
			WHEN POP.Metric = 'Metric2' AND POP.Segment = 5 THEN POP.BorrowerCount  
			WHEN POP.Metric = 'Metric3' AND POP.Segment = 5 THEN POP.BorrowerCount 
			WHEN POP.Metric = 'Other' AND POP.Segment = 5 THEN POP.BorrowerCount 
		END
	) AS Segment5BorrowerCount,
	MAX
	(
		CASE 
			WHEN POP.Metric = 'Metric1' AND POP.Segment = 5 THEN POP.SegmentTotal 
			WHEN POP.Metric = 'Metric2' AND POP.Segment = 5 THEN POP.SegmentTotal 
			WHEN POP.Metric = 'Metric3' AND POP.Segment = 5 THEN POP.SegmentTotal  
			WHEN POP.Metric = 'Other' AND POP.Segment = 5 THEN POP.SegmentTotal 
		END
	) AS Segment5TotalCount,
	MAX
	(
		CASE 
			WHEN POP.Metric = 'Metric1' AND POP.Segment = 5 THEN POP.SegmentPercentage 
			WHEN POP.Metric = 'Metric2' AND POP.Segment = 5 THEN POP.SegmentPercentage  
			WHEN POP.Metric = 'Metric3' AND POP.Segment = 5 THEN POP.SegmentPercentage 
			WHEN POP.Metric = 'Other' AND POP.Segment = 5 THEN POP.SegmentPercentage 
		END
	) AS Segment5Percentage,
	MAX
	(
		CASE 
			WHEN POP.Metric = 'Metric1' AND POP.Segment = 5 THEN POP.MetricPercentage 
			WHEN POP.Metric = 'Metric2' AND POP.Segment = 5 THEN POP.MetricPercentage  
			WHEN POP.Metric = 'Metric3' AND POP.Segment = 5 THEN POP.MetricPercentage 
			WHEN POP.Metric = 'Other' AND POP.Segment = 5 THEN POP.MetricPercentage 
		END
	) AS Segment5MetricPercentage,
	MAX
	(
		CASE 
			WHEN POP.Metric = 'Metric1' AND POP.Segment = 6 THEN POP.BorrowerCount 
			WHEN POP.Metric = 'Metric2' AND POP.Segment = 6 THEN POP.BorrowerCount  
			WHEN POP.Metric = 'Metric3' AND POP.Segment = 6 THEN POP.BorrowerCount 
			WHEN POP.Metric = 'Other' AND POP.Segment = 6 THEN POP.BorrowerCount 
		END
	) AS Segment6BorrowerCount,
	MAX
	(
		CASE 
			WHEN POP.Metric = 'Metric1' AND POP.Segment = 6 THEN POP.SegmentTotal 
			WHEN POP.Metric = 'Metric2' AND POP.Segment = 6 THEN POP.SegmentTotal 
			WHEN POP.Metric = 'Metric3' AND POP.Segment = 6 THEN POP.SegmentTotal  
			WHEN POP.Metric = 'Other' AND POP.Segment = 6 THEN POP.SegmentTotal 
		END
	) AS Segment6TotalCount,
	MAX
	(
		CASE 
			WHEN POP.Metric = 'Metric1' AND POP.Segment = 6 THEN POP.SegmentPercentage 
			WHEN POP.Metric = 'Metric2' AND POP.Segment = 6 THEN POP.SegmentPercentage  
			WHEN POP.Metric = 'Metric3' AND POP.Segment = 6 THEN POP.SegmentPercentage 
			WHEN POP.Metric = 'Other' AND POP.Segment = 6 THEN POP.SegmentPercentage 
		END
	) AS Segment6Percentage,
	MAX
	(
		CASE 
			WHEN POP.Metric = 'Metric1' AND POP.Segment = 6 THEN POP.MetricPercentage 
			WHEN POP.Metric = 'Metric2' AND POP.Segment = 6 THEN POP.MetricPercentage  
			WHEN POP.Metric = 'Metric3' AND POP.Segment = 6 THEN POP.MetricPercentage 
			WHEN POP.Metric = 'Other' AND POP.Segment = 6 THEN POP.MetricPercentage 
		END
	) AS Segment6MetricPercentage,
	MAX
	(
		CASE 
			WHEN POP.Metric = 'Metric1' AND POP.Segment = 1 THEN POP.MetricTotal 
			WHEN POP.Metric = 'Metric2' AND POP.Segment = 1 THEN POP.MetricTotal  
			WHEN POP.Metric = 'Metric3' AND POP.Segment = 1 THEN POP.MetricTotal  
			WHEN POP.Metric = 'Other' AND POP.Segment = 1 THEN POP.MetricTotal 
		END
	) AS MetricTotal,
	GETDATE() AS CreatedAt,
	SYSTEM_USER AS AddedBy
FROM
(
SELECT
	Metric,
	SEGMENT,
	CAST(GETDATE() AS DATE) AS ReportDate,
	SUM(COALESCE(BorrowerCount,0)) OVER(PARTITION BY Metric, SEGMENT) AS BorrowerCount,
	SUM(COALESCE(BorrowerCount,0)) OVER(PARTITION BY SEGMENT) AS SegmentTotal,
	SUM(COALESCE(BorrowerCount,0)) OVER(PARTITION BY Metric) AS MetricTotal,
	CAST(SUM(COALESCE(BorrowerCount,0)) OVER(PARTITION BY Metric, SEGMENT) AS DECIMAL(14,2)) / CAST(SUM(COALESCE(BorrowerCount,0)) OVER(PARTITION BY SEGMENT) AS DECIMAL(14,2)) AS SegmentPercentage,
	CAST(SUM(COALESCE(BorrowerCount,0)) OVER(PARTITION BY Metric, SEGMENT) AS DECIMAL(14,2)) / CAST(SUM(COALESCE(BorrowerCount,0)) OVER(PARTITION BY Metric) AS DECIMAL(14,2)) AS MetricPercentage

FROM
(
	SELECT
		CASE 
			WHEN BL.PerformanceCategory = '03' THEN 'Metric1'
			 WHEN BL.PerformanceCategory IN ('09','10') THEN 'Metric2'
			 WHEN BL.PerformanceCategory = '11' THEN 'Metric3'
			 ELSE 'Other'
		END AS Metric,
		BL.SEGMENT,
		COUNT(DISTINCT BL.BF_SSN) AS BorrowerCount
	FROM
		CDW.FsaInvMet.Daily_BorrowerLevel BL
	WHERE
		BL.PerformanceCategory IN('03','07','08','09','10','11')
	GROUP BY
		CASE WHEN BL.PerformanceCategory = '03' THEN 'Metric1'
			 WHEN BL.PerformanceCategory IN('09','10') THEN 'Metric2'
			 WHEN BL.PerformanceCategory = '11' THEN 'Metric3'
			 ELSE 'Other'
		END,
		BL.SEGMENT
) MetricTracking

WHERE
	SEGMENT != 7

) POP
LEFT JOIN CDW.FsaInvMet.R2 Existing
	ON Existing.ReportDate = POP.ReportDate
WHERE
	Existing.ReportDate IS NULL
GROUP BY
	POP.ReportDate,
	POP.Metric
ORDER BY
	Metric