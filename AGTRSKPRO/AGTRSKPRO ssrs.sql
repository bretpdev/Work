USE [CentralData]
GO
/****** Object:  StoredProcedure [agtrskpro].[GetTasksSSRS]    Script Date: 7/23/2020 12:48:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [agtrskpro].GetTasksSSRS
	
AS
DECLARE @Yesterday DATE = DATEADD(DAY,-1,GETDATE());
DROP TABLE IF EXISTS #TasksBase;

SELECT DISTINCT
	BUTM.BusinessUnitName,
	WQ21.WF_QUE AS [QUEUE],
	WQ21.WF_SUB_QUE AS SUB,
	WQ21.WN_CTL_TSK,
	CASE WHEN WQ21.WC_STA_WQUE20 = 'X' THEN 'Cancelled'
		 WHEN WQ21.WC_STA_WQUE20 = 'C' THEN 'Closed'
		 ELSE ''
	END AS [CURRENT STATUS],
	COALESCE(NULLIF(ISNULL(NULLIF(RTRIM(ISNULL(GX25.XM_USR_1,'')) + ' ' + RTRIM(ISNULL(GX25.XM_USR_LST,'')),' '), RTRIM(WQ21.WF_USR_ASN_TSK)) + ' - ' + RTRIM(WQ21.WF_USR_ASN_TSK), ' - '),'System') AS [USER],
	PD10.DF_SPE_ACC_ID AS ACCOUNT,
	WQ21.LN_ATY_SEQ AS ACTIVITY,
	WQ21.PF_REQ_ACT AS ARC,
	WQ21.WF_USR_ASN_TSK,
	WQ21.WF_CRT_DTS_WQ21,
	CAST(WQ21.WD_ACT_REQ AS DATE) AS [REQUESTED DATE],
	CAST(WQ21.WF_LST_DTS_WQ20 AS DATE) AS [CLOSED DATE],
	CAST(WQ21.WF_LST_DTS_WQ20 AS TIME(0)) AS [CLOSED TIME]
INTO
	#TasksBase
FROM
	CentralData.agtrskpro.BusinessUnitTaskMapping BUTM
	INNER JOIN UDW..WQ21_TSK_QUE_HST WQ21
		ON WQ21.WF_QUE = BUTM.[Queue]
		AND WQ21.WC_STA_WQUE20 IN('X','C')
		AND
		(
			WQ21.WF_SUB_QUE = BUTM.Subqueue
			OR BUTM.Subqueue IS NULL
		)
	INNER JOIN UDW..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = WQ21.BF_SSN
	LEFT JOIN UDW..GX25_USR GX25
		ON GX25.XF_USR = WQ21.WF_USR_ASN_TSK	
WHERE
	BUTM.DeletedAt IS NULL --Active task
	AND BUTM.BusinessUnitName NOT LIKE '%FED'
	AND BUTM.BusinessUnitName NOT IN('Repayment Services','Account Services')
	AND CAST(WQ21.WF_LST_DTS_WQ20 AS DATE) = @Yesterday --Task completed yesterday

UNION ALL

SELECT DISTINCT
	BUTM.BusinessUnitName,
	WQ21.WF_QUE AS [QUEUE],
	WQ21.WF_SUB_QUE AS SUB,
	WQ21.WN_CTL_TSK,
	CASE WHEN WQ21.WC_STA_WQUE20 = 'X' THEN 'Cancelled'
		 WHEN WQ21.WC_STA_WQUE20 = 'C' THEN 'Closed'
		 ELSE ''
	END AS [CURRENT STATUS],
	COALESCE(NULLIF(ISNULL(NULLIF(RTRIM(ISNULL(GX25.XM_USR_1,'')) + ' ' + RTRIM(ISNULL(GX25.XM_USR_LST,'')),' '), RTRIM(WQ21.WF_USR_ASN_TSK)) + ' - ' + RTRIM(WQ21.WF_USR_ASN_TSK), ' - '),'System') AS [USER],
	PD10.DF_SPE_ACC_ID AS ACCOUNT,
	WQ21.LN_ATY_SEQ AS ACTIVITY,
	WQ21.PF_REQ_ACT AS ARC,
	WQ21.WF_USR_ASN_TSK,
	WQ21.WF_CRT_DTS_WQ21,
	CAST(WQ21.WD_ACT_REQ AS DATE) AS [REQUESTED DATE],
	CAST(WQ21.WF_LST_DTS_WQ20 AS DATE) AS [CLOSED DATE],
	CAST(WQ21.WF_LST_DTS_WQ20 AS TIME(0)) AS [CLOSED TIME]
FROM
	CentralData.agtrskpro.BusinessUnitTaskMapping BUTM
	INNER JOIN CDW..WQ21_TSK_QUE_HST WQ21
		ON WQ21.WF_QUE = BUTM.[Queue]
		AND WQ21.WC_STA_WQUE20 IN('X','C')
		AND
		(
			WQ21.WF_SUB_QUE = BUTM.Subqueue
			OR BUTM.Subqueue IS NULL
		)
	INNER JOIN CDW..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = WQ21.BF_SSN
	LEFT JOIN CDW..GX25_USR GX25
		ON GX25.XF_USR = WQ21.WF_USR_ASN_TSK	
WHERE
	BUTM.DeletedAt IS NULL --Active task
	AND BUTM.BusinessUnitName NOT LIKE '%UHEAA'
	AND BUTM.BusinessUnitName != 'Loan Services'
	AND CAST(WQ21.WF_LST_DTS_WQ20 AS DATE) = @Yesterday --Task completed yesterday

SELECT
	TB.*,
	COALESCE(MaxPrevU.WD_INI_TSK,MaxPrevC.WD_INI_TSK) AS WD_INI_TSK,
	COALESCE(MaxPrevU.WT_INI_TSK,MaxPrevC.WT_INI_TSK) AS WT_INI_TSK
FROM
	#TasksBase TB
	LEFT JOIN
	(
		SELECT DISTINCT
			Prev.*,
			WQ21.WD_INI_TSK,
			WQ21.WT_INI_TSK
		FROM
		(
			SELECT DISTINCT
				WQ21.WF_QUE,
				WQ21.WF_SUB_QUE,
				WQ21.WN_CTL_TSK,
				WQ21.PF_REQ_ACT,
				WQ21.WF_USR_ASN_TSK,
				MAX(WQ21.WF_CRT_DTS_WQ21) AS Max_WF_CRT_DTS_WQ21
			FROM
				CentralData.agtrskpro.BusinessUnitTaskMapping BUTM
				INNER JOIN UDW..WQ21_TSK_QUE_HST WQ21
					ON WQ21.WF_QUE = BUTM.[Queue]
					AND WQ21.WC_STA_WQUE20 IN('A','H','P','U','W')
					AND
					(
						WQ21.WF_SUB_QUE = BUTM.Subqueue
						OR BUTM.Subqueue IS NULL
					)
					AND WQ21.WD_ACT_REQ != CAST(GETDATE() AS DATE)
			GROUP BY
				WQ21.WF_QUE,
				WQ21.WF_SUB_QUE,
				WQ21.WN_CTL_TSK,
				WQ21.PF_REQ_ACT,
				WQ21.WF_USR_ASN_TSK
		) Prev
		INNER JOIN UDW..WQ21_TSK_QUE_HST WQ21
			ON WQ21.WF_QUE = Prev.WF_QUE
			AND WQ21.WF_SUB_QUE = Prev.WF_SUB_QUE
			AND WQ21.WN_CTL_TSK = Prev.WN_CTL_TSK
			AND WQ21.PF_REQ_ACT = Prev.PF_REQ_ACT
			AND WQ21.WF_USR_ASN_TSK = Prev.WF_USR_ASN_TSK
			AND WQ21.WF_CRT_DTS_WQ21 = Prev.Max_WF_CRT_DTS_WQ21
	) MaxPrevU
		ON MaxPrevU.WF_QUE = TB.[QUEUE]
		AND MaxPrevU.WF_SUB_QUE = TB.SUB
		AND MaxPrevU.WN_CTL_TSK = TB.WN_CTL_TSK
		AND MaxPrevU.PF_REQ_ACT = TB.ARC
		AND MaxPrevU.WF_USR_ASN_TSK = TB.WF_USR_ASN_TSK
		AND MaxPrevU.Max_WF_CRT_DTS_WQ21 < TB.WF_CRT_DTS_WQ21
	LEFT JOIN
	(
		SELECT DISTINCT
			Prev.*,
			WQ21.WD_INI_TSK,
			WQ21.WT_INI_TSK
		FROM
		(
			SELECT DISTINCT
				WQ21.WF_QUE,
				WQ21.WF_SUB_QUE,
				WQ21.WN_CTL_TSK,
				WQ21.PF_REQ_ACT,
				WQ21.WF_USR_ASN_TSK,
				MAX(WQ21.WF_CRT_DTS_WQ21) AS Max_WF_CRT_DTS_WQ21
			FROM
				CentralData.agtrskpro.BusinessUnitTaskMapping BUTM
				INNER JOIN CDW..WQ21_TSK_QUE_HST WQ21
					ON WQ21.WF_QUE = BUTM.[Queue]
					AND WQ21.WC_STA_WQUE20 IN('A','H','P','U','W')
					AND
					(
						WQ21.WF_SUB_QUE = BUTM.Subqueue
						OR BUTM.Subqueue IS NULL
					)
					AND WQ21.WD_ACT_REQ != CAST(GETDATE() AS DATE)
			GROUP BY
				WQ21.WF_QUE,
				WQ21.WF_SUB_QUE,
				WQ21.WN_CTL_TSK,
				WQ21.PF_REQ_ACT,
				WQ21.WF_USR_ASN_TSK
		) Prev
		INNER JOIN CDW..WQ21_TSK_QUE_HST WQ21
			ON WQ21.WF_QUE = Prev.WF_QUE
			AND WQ21.WF_SUB_QUE = Prev.WF_SUB_QUE
			AND WQ21.WN_CTL_TSK = Prev.WN_CTL_TSK
			AND WQ21.PF_REQ_ACT = Prev.PF_REQ_ACT
			AND WQ21.WF_USR_ASN_TSK = Prev.WF_USR_ASN_TSK
			AND WQ21.WF_CRT_DTS_WQ21 = Prev.Max_WF_CRT_DTS_WQ21
	) MaxPrevC
		ON MaxPrevC.WF_QUE = TB.[QUEUE]
		AND MaxPrevC.WF_SUB_QUE = TB.SUB
		AND MaxPrevC.WN_CTL_TSK = TB.WN_CTL_TSK
		AND MaxPrevC.PF_REQ_ACT = TB.ARC
		AND MaxPrevC.WF_USR_ASN_TSK = TB.WF_USR_ASN_TSK
		AND MaxPrevC.Max_WF_CRT_DTS_WQ21 < TB.WF_CRT_DTS_WQ21 
GO
CREATE PROCEDURE [agtrskpro].GetTasksSummarySSRS

AS
DECLARE @Yesterday DATE = DATEADD(DAY,-1,GETDATE());
SELECT DISTINCT
	BUTM.BusinessUnitName,
	COALESCE(NULLIF(ISNULL(NULLIF(RTRIM(ISNULL(GX25.XM_USR_1,'')) + ' ' + RTRIM(ISNULL(GX25.XM_USR_LST,'')),' '), RTRIM(WQ21.WF_USR_ASN_TSK)) + ' - ' + RTRIM(WQ21.WF_USR_ASN_TSK), ' - '),'System') AS [USER],
	WQ21.PF_REQ_ACT AS ARC,
	WQ21.WF_SUB_QUE AS SUB,
	COUNT(PD10.DF_SPE_ACC_ID) OVER(PARTITION BY BUTM.BusinessUnitName, WQ21.WF_USR_ASN_TSK, WQ21.PF_REQ_ACT, WQ21.WF_SUB_QUE) AS [ARC TOTAL],
	COUNT(PD10.DF_SPE_ACC_ID) OVER(PARTITION BY BUTM.BusinessUnitName, WQ21.WF_USR_ASN_TSK) AS [GRAND TOTAL]
FROM
	CentralData.agtrskpro.BusinessUnitTaskMapping BUTM
	INNER JOIN UDW..WQ21_TSK_QUE_HST WQ21
		ON WQ21.WF_QUE = BUTM.[Queue]
		AND
		(
			WQ21.WF_SUB_QUE = BUTM.Subqueue
			OR BUTM.Subqueue IS NULL
		)
	INNER JOIN UDW..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = WQ21.BF_SSN
	LEFT JOIN UDW..GX25_USR GX25
		ON GX25.XF_USR = WQ21.WF_USR_ASN_TSK
WHERE
	BUTM.DeletedAt IS NULL --Active task
	AND BUTM.BusinessUnitName NOT LIKE '%FED'
	AND BUTM.BusinessUnitName NOT IN('Repayment Services','Account Services')
	AND WQ21.WC_STA_WQUE20 IN('X','C')
	AND CAST(WQ21.WF_LST_DTS_WQ20 AS DATE) = @Yesterday --Completed yesterday

UNION ALL

SELECT DISTINCT
	BUTM.BusinessUnitName,
	COALESCE(NULLIF(ISNULL(NULLIF(RTRIM(ISNULL(GX25.XM_USR_1,'')) + ' ' + RTRIM(ISNULL(GX25.XM_USR_LST,'')),' '), RTRIM(WQ21.WF_USR_ASN_TSK)) + ' - ' + RTRIM(WQ21.WF_USR_ASN_TSK), ' - '),'System') AS [USER],
	WQ21.PF_REQ_ACT AS ARC,
	WQ21.WF_SUB_QUE AS SUB,
	COUNT(PD10.DF_SPE_ACC_ID) OVER(PARTITION BY BUTM.BusinessUnitName, WQ21.WF_USR_ASN_TSK, WQ21.PF_REQ_ACT, WQ21.WF_SUB_QUE) AS [ARC TOTAL],
	COUNT(PD10.DF_SPE_ACC_ID) OVER(PARTITION BY BUTM.BusinessUnitName, WQ21.WF_USR_ASN_TSK) AS [GRAND TOTAL]
FROM
	CentralData.agtrskpro.BusinessUnitTaskMapping BUTM
	INNER JOIN CDW..WQ21_TSK_QUE_HST WQ21
		ON WQ21.WF_QUE = BUTM.[Queue]
		AND
		(
			WQ21.WF_SUB_QUE = BUTM.Subqueue
			OR BUTM.Subqueue IS NULL
		)
	INNER JOIN CDW..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = WQ21.BF_SSN
	LEFT JOIN CDW..GX25_USR GX25
		ON GX25.XF_USR = WQ21.WF_USR_ASN_TSK
WHERE
	BUTM.DeletedAt IS NULL --Active task
	AND BUTM.BusinessUnitName NOT LIKE '%UHEAA'
	AND BUTM.BusinessUnitName != 'Loan Services'
	AND WQ21.WC_STA_WQUE20 IN('X','C')
	AND CAST(WQ21.WF_LST_DTS_WQ20 AS DATE) = @Yesterday --Completed yesterday
