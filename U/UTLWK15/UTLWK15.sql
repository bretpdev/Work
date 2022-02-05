DECLARE @QUEUE VARCHAR(8) = 'BKCP7RVW'
DECLARE @TODAY DATE = GETDATE()

INSERT INTO OLS.olqtskbldr.Queues (TargetId, QueueName, Comment, DateDue, TimeDue, AddedAt, AddedBy, InstitutionId, InstitutionType)
SELECT DISTINCT
	LN10.BF_SSN AS [TargetId]
	,@QUEUE AS [QueueName]
	,'"Compass BK Filed Date: ' + CONVERT(VARCHAR(10), CAST(PD24.DD_BKR_FIL AS DATE), 101)
	+ ',BK Status: ' + CONVERT(VARCHAR(10), CAST(PD24.DD_BKR_STA AS DATE), 101)
	+ ',BK Chapter: ' + RTRIM(PD24.DC_BKR_TYP)
	+ ',BK Court: ' + RTRIM(PD24.DF_COU_DKT)
	+ ',Last RVW: ' + ISNULL(CONVERT(VARCHAR(10), CAST(AY01_MAX.MAXREVIEW AS DATE), 101), ' ')
	+ ',Date Not: ' + CONVERT(VARCHAR(10), CAST(PD24.DD_BKR_NTF AS DATE), 101) + '"' AS [Comment]
	,NULL --DateDue
	,NULL --TimeDue
	,GETDATE() --AddedAt
	,USER_NAME() --AddedBy
	,NULL --InstitutionId
	,NULL --InstitutionType
FROM
	UDW..LN10_LON LN10
	INNER JOIN ODW..PD01_PDM_INF PD01 --Make sure to join with PD01 to remove any non Uheaa borrowers
		ON PD01.DF_PRS_ID = LN10.BF_SSN
	INNER JOIN UDW..PD24_PRS_BKR PD24
		ON PD24.DF_PRS_ID = LN10.BF_SSN
		AND PD24.DC_BKR_STA = '06'
	LEFT JOIN
	(
		SELECT DISTINCT
			AY01.DF_PRS_ID
			,'Y' AS EXCLD
		FROM
			ODW..AY01_BR_ATY AY01
			INNER JOIN UDW..PD24_PRS_BKR PD24
	 			ON AY01.DF_PRS_ID = PD24.DF_PRS_ID
		WHERE
			AY01.PF_ACT = 'DBKRW'
			AND AY01.BD_ATY_PRF > CAST(DATEADD(D, -30, GETDATE()) AS DATE)
			AND PD24.DC_BKR_TYP = '07'
	) AY01
		ON AY01.DF_PRS_ID = LN10.BF_SSN
	LEFT JOIN 
	(
		SELECT
			DF_PRS_ID
			,MAX(BD_ATY_PRF) AS MAXREVIEW
		FROM
			ODW..AY01_BR_ATY
		WHERE
			PF_ACT = 'DBKRW'
		GROUP BY
			DF_PRS_ID
	) AY01_MAX
		ON AY01_MAX.DF_PRS_ID = LN10.BF_SSN
	LEFT JOIN ODW..CT30_CALL_QUE CT30
		ON CT30.DF_PRS_ID_BR = LN10.BF_SSN
		AND CT30.IF_WRK_GRP = @QUEUE
		AND CT30.IC_TSK_STA IN ('A','W') --task is open
	LEFT JOIN  OLS.olqtskbldr.Queues ExistingData
		ON ExistingData.TargetId = LN10.BF_SSN
		AND ExistingData.QueueName = @QUEUE
		AND
		(
			CAST(ExistingData.AddedAt AS DATE) = @TODAY
			OR ExistingData.ProcessedAt IS NULL
		)
		AND ExistingData.DeletedAt IS NULL
		AND ExistingData.DeletedBy IS NULL
WHERE
	LN10.LA_CUR_PRI > 0.00
	AND LN10.IC_LON_PGM != 'TILP'
	AND LN10.LC_STA_LON10 = 'R'
	AND PD24.DD_BKR_FIL <= CAST(DATEADD(D, -120, GETDATE()) AS DATE)
	AND PD24.DC_BKR_TYP = '07'
	AND AY01.EXCLD IS NULL
	AND CT30.DF_PRS_ID_BR IS NULL --No open queue tasks
	AND ExistingData.TargetId IS NULL
ORDER BY
	TargetId