USE ServicerInventoryMetrics
GO

DECLARE @CurrentDate DATETIME
SET @CurrentDate = GETDATE()

DELETE 
	MS 
FROM 
	[ServicerInventoryMetrics].[dbo].[MetricsSummary] MS 
	INNER JOIN [ServicerInventoryMetrics].[dbo].[ServicerMetrics] SM
	ON SM.ServicerMetricsId = MS.ServicerMetricsId
WHERE 
	SM.IsManualUpdate = 0 
	AND MS.MetricMonth = MONTH(@CurrentDate)
	AND MS.MetricYear = YEAR(@CurrentDate)

INSERT INTO MetricsSummary(ServicerMetricsId, CompliantRecords, TotalRecords, AverageBacklogAge, MetricMonth, MetricYear)
(--Forbearances
SELECT 
	ServicerMetricsId,
	(SELECT COUNT(*) FROM Forbearance WHERE CentralData.dbo.BusinessDaysDiff(WD_ACT_REQ,@CurrentDate) <= 10) AS CompliantRecords,
	(SELECT COUNT(*) FROM Forbearance) AS TotalRecords,
	(SELECT AVG(CentralData.dbo.BusinessDaysDiff(WD_ACT_REQ,@CurrentDate) - 10) FROM Forbearance WHERE CentralData.dbo.BusinessDaysDiff(WD_ACT_REQ,@CurrentDate) > 10) AS AverageBacklogAge,
	MONTH(@CurrentDate) AS MetricMonth,
	YEAR(@CurrentDate) AS MetricYear
FROM 
	[ServicerInventoryMetrics].[dbo].[ServicerMetrics]
WHERE 
	ServicerMetric = 'Forbearance'

UNION ALL
--Deferments
SELECT 
	ServicerMetricsId,
	(SELECT COUNT(*) FROM Deferment WHERE CentralData.dbo.BusinessDaysDiff(WD_ACT_REQ,@CurrentDate) <= 10) AS CompliantRecords,
	(SELECT COUNT(*) FROM Deferment) AS TotalRecords,
	(SELECT AVG(CentralData.dbo.BusinessDaysDiff(WD_ACT_REQ,@CurrentDate) - 10) FROM Deferment WHERE CentralData.dbo.BusinessDaysDiff(WD_ACT_REQ,@CurrentDate) > 10) AS AverageBacklogAge,
	MONTH(@CurrentDate) AS MetricMonth,
	YEAR(@CurrentDate) AS MetricYear
FROM 
	[ServicerInventoryMetrics].[dbo].[ServicerMetrics]
WHERE 
	ServicerMetric = 'Deferment'

UNION ALL
--IDR
SELECT 
	ServicerMetricsId,
	(SELECT COUNT(*) FROM IDR WHERE	CentralData.dbo.BusinessDaysDiff(WD_ACT_REQ,@CurrentDate) <= 15) AS CompliantRecords,
	(SELECT COUNT(*) FROM IDR) AS TotalRecords,
	(SELECT AVG(CentralData.dbo.BusinessDaysDiff(WD_ACT_REQ,@CurrentDate) - 15) FROM IDR WHERE CentralData.dbo.BusinessDaysDiff(WD_ACT_REQ,@CurrentDate) > 15) AS AverageBacklogAge,
	MONTH(@CurrentDate) AS MetricMonth,
	YEAR(@CurrentDate) AS MetricYear
FROM 
	[ServicerInventoryMetrics].[dbo].[ServicerMetrics]
WHERE 
	ServicerMetric = 'IDR'

UNION ALL
--BankruptcyNotices
SELECT
	ServicerMetricsId,
	(SELECT COUNT(*) FROM BankruptcyNotice WHERE CentralData.dbo.BusinessDaysDiff(WD_ACT_REQ,@CurrentDate) <= 30) AS CompliantRecords,
	(SELECT COUNT(*) FROM BankruptcyNotice) AS TotalRecords,
	(SELECT AVG(CentralData.dbo.BusinessDaysDiff(WD_ACT_REQ,@CurrentDate) - 30) FROM BankruptcyNotice WHERE CentralData.dbo.BusinessDaysDiff(WD_ACT_REQ,@CurrentDate) > 30) AS AverageBacklogAge,
	MONTH(@CurrentDate) AS MetricMonth,
	YEAR(@CurrentDate) AS MetricYear
FROM 
	[ServicerInventoryMetrics].[dbo].[ServicerMetrics]
WHERE 
	ServicerMetric = 'Bankruptcy - Notices'

UNION ALL
--BankruptcyProofOfClaim

SELECT
	ServicerMetricsId,
	(SELECT COUNT(*) FROM BankruptcyProofOfClaim WHERE DATEDIFF(DAY,@CurrentDate, DEADLINE) >= 0) AS CompliantRecords,
	(SELECT COUNT(*) FROM BankruptcyProofOfClaim) AS TotalRecords,
	(SELECT AVG(ABS(DATEDIFF(DAY,@CurrentDate, DEADLINE))) FROM BankruptcyProofOfClaim WHERE DATEDIFF(DAY,@CurrentDate, DEADLINE) < 0) AS AverageBacklogAge,
	MONTH(@CurrentDate) AS MetricMonth,
	YEAR(@CurrentDate) AS MetricYear
FROM 
	[ServicerInventoryMetrics].[dbo].[ServicerMetrics]
WHERE 
	ServicerMetric = 'Bankruptcy - Proof of Claim'

UNION ALL
--Aging 360 servicer
SELECT
	ServicerMetricsId,
	(SELECT COUNT(*) FROM Aging360AtServicer WHERE DATEDIFF(DAY, AGING_DATE, @CurrentDate) <= 60) AS CompliantRecords, 
	(SELECT COUNT(*) FROM Aging360AtServicer) AS TotalRecords,
	(SELECT AVG(DATEDIFF(DAY, AGING_DATE, @CurrentDate) - 60) FROM Aging360AtServicer WHERE DATEDIFF(DAY, AGING_DATE, @CurrentDate) > 60) AS AverageBacklogAge,
	MONTH(@CurrentDate) AS MetricMonth,
	YEAR(@CurrentDate) AS MetricYear
FROM 
	[ServicerInventoryMetrics].[dbo].[ServicerMetrics]
WHERE 
	ServicerMetric = 'Aging +360 & Still at Servicer'

UNION ALL
--Aging 360 Sent to DMCS
SELECT
	ServicerMetricsId,
	(SELECT COUNT(*) FROM Aging360SentToDMCS WHERE DATEDIFF(DAY, AGING_DATE, @CurrentDate) <= 7) AS CompliantRecords, 
	(SELECT COUNT(*) FROM Aging360SentToDMCS) AS TotalRecords,
	(SELECT AVG(DATEDIFF(DAY, AGING_DATE, @CurrentDate) - 7) FROM Aging360SentToDMCS WHERE DATEDIFF(DAY, AGING_DATE, @CurrentDate) > 7) AS AverageBacklogAge,
	MONTH(@CurrentDate) AS MetricMonth,
	YEAR(@CurrentDate) AS MetricYear
FROM 
	[ServicerInventoryMetrics].[dbo].[ServicerMetrics]
WHERE 
	ServicerMetric = 'Aging +360 & Sent to DMCS to Process'

UNION ALL
--Aging 360 Not accepted by DMCS
SELECT
	ServicerMetricsId,
	(SELECT COUNT(*) FROM Aging360NotAcceptedByDMCS WHERE DATEDIFF(DAY, AGING_DATE, @CurrentDate) <= 90) AS CompliantRecords, 
	(SELECT COUNT(*) FROM Aging360NotAcceptedByDMCS) AS TotalRecords,
	(SELECT AVG(DATEDIFF(DAY, AGING_DATE, @CurrentDate) - 90) FROM Aging360NotAcceptedByDMCS WHERE DATEDIFF(DAY, AGING_DATE, @CurrentDate) > 90) AS AverageBacklogAge,
	MONTH(@CurrentDate) AS MetricMonth,
	YEAR(@CurrentDate) AS MetricYear
FROM 
	[ServicerInventoryMetrics].[dbo].[ServicerMetrics]
WHERE 
	ServicerMetric = 'Aging +360 & Not accepted by DMCS'

UNION ALL
--Death Discharge Servicer Aging
SELECT 
	ServicerMetricsId,
	(SELECT COUNT(*) FROM DeathDischarge WHERE DATEDIFF(DAY, WD_ACT_REQ, @CurrentDate) <= 30) AS CompliantRecords, 
	(SELECT COUNT(*) FROM DeathDischarge) AS TotalRecords,
	(SELECT AVG(DATEDIFF(DAY, WD_ACT_REQ, @CurrentDate) - 30) FROM DeathDischarge WHERE DATEDIFF(DAY, WD_ACT_REQ, @CurrentDate) > 30) AS AverageBacklogAge,
	MONTH(@CurrentDate) AS MetricMonth,
	YEAR(@CurrentDate) AS MetricYear
FROM 
	[ServicerInventoryMetrics].[dbo].[ServicerMetrics]
WHERE 
	ServicerMetric = 'Death Discharge Servicer Aging'

UNION ALL
--Death Discharge FSA Aging
SELECT
	ServicerMetricsId,
	(SELECT COUNT(*) FROM DeathDischargeFSA WHERE DATEDIFF(DAY, LD_ATY_REQ_RCV, @CurrentDate) <= 30) AS CompliantRecords, 
	(SELECT COUNT(*) FROM DeathDischargeFSA) AS TotalRecords,
	(SELECT AVG(DATEDIFF(DAY, LD_ATY_REQ_RCV, @CurrentDate) - 30) FROM DeathDischargeFSA WHERE DATEDIFF(DAY, LD_ATY_REQ_RCV, @CurrentDate) > 30) AS AverageBacklogAge,
	MONTH(@CurrentDate) AS MetricMonth,
	YEAR(@CurrentDate) AS MetricYear
FROM 
	[ServicerInventoryMetrics].[dbo].[ServicerMetrics]
WHERE 
	ServicerMetric = 'Death Discharge FSA Aging'

UNION ALL
--Closed School Applications Processed
SELECT
	ServicerMetricsId,
	(SELECT COUNT(*) FROM ClosedSchoolApp WHERE DATEDIFF(DAY, WD_ACT_REQ, @CurrentDate) <= 30) AS CompliantRecords, 
	(SELECT COUNT(*) FROM ClosedSchoolApp) AS TotalRecords,
	(SELECT AVG(DATEDIFF(DAY, WD_ACT_REQ, @CurrentDate) - 30) FROM ClosedSchoolApp WHERE DATEDIFF(DAY, WD_ACT_REQ, @CurrentDate) > 30) AS AverageBacklogAge,
	MONTH(@CurrentDate) AS MetricMonth,
	YEAR(@CurrentDate) AS MetricYear
FROM 
	[ServicerInventoryMetrics].[dbo].[ServicerMetrics]
WHERE 
	ServicerMetric = 'Closed School Applications Processed'

UNION ALL
--Closed School FSA Aging
SELECT
	ServicerMetricsId,
	(SELECT COUNT(*) FROM ClosedSchoolFSA WHERE DATEDIFF(DAY, LD_ATY_REQ_RCV, @CurrentDate) <= 30) AS CompliantRecords, 
	(SELECT COUNT(*) FROM ClosedSchoolFSA) AS TotalRecords,
	(SELECT AVG(DATEDIFF(DAY, LD_ATY_REQ_RCV, @CurrentDate) - 30) FROM ClosedSchoolFSA WHERE DATEDIFF(DAY, LD_ATY_REQ_RCV, @CurrentDate) > 30) AS AverageBacklogAge,
	MONTH(@CurrentDate) AS MetricMonth,
	YEAR(@CurrentDate) AS MetricYear
FROM 
	[ServicerInventoryMetrics].[dbo].[ServicerMetrics]
WHERE
	ServicerMetric = 'Closed School FSA Aging'

UNION ALL
--Borrower Mail
SELECT
	ServicerMetricsId,
	(SELECT COUNT(*) FROM BorrowerMail WHERE CentralData.dbo.BusinessDaysDiff(WD_ACT_REQ,@CurrentDate) <= 10) AS CompliantRecords, 
	(SELECT COUNT(*) FROM BorrowerMail) AS TotalRecords,
	(SELECT AVG(CentralData.dbo.BusinessDaysDiff(WD_ACT_REQ,@CurrentDate) - 10) FROM BorrowerMail WHERE CentralData.dbo.BusinessDaysDiff(WD_ACT_REQ,@CurrentDate) > 10) AS AverageBacklogAge,
	MONTH(@CurrentDate) AS MetricMonth,
	YEAR(@CurrentDate) AS MetricYear
FROM 
	[ServicerInventoryMetrics].[dbo].[ServicerMetrics]
WHERE 
	ServicerMetric = 'Borrower Mail - Letters'

UNION ALL
--Credit Balances
SELECT
	ServicerMetricsId,
	(SELECT COUNT(*) FROM CreditBalance WHERE DATEDIFF(DAY, LD_FAT_APL, @CurrentDate) <= 45)  AS CompliantRecords, 
	(SELECT COUNT(*) FROM CreditBalance) AS TotalRecords,
	(SELECT AVG(DATEDIFF(DAY, LD_FAT_APL, @CurrentDate) - 45) FROM CreditBalance WHERE DATEDIFF(DAY, LD_FAT_APL, @CurrentDate) > 45) AS AverageBacklogAge,
	MONTH(@CurrentDate) AS MetricMonth,
	YEAR(@CurrentDate) AS MetricYear
FROM [ServicerInventoryMetrics].[dbo].[ServicerMetrics]
WHERE ServicerMetric = 'Credit Balances'
)
