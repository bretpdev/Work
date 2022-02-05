DECLARE @PFACT VARCHAR(5) = 'K1APP'
DECLARE @WF_QUE VARCHAR(2) = 'K8'
DECLARE @WRKGRP VARCHAR(8) = 'KLOANAPP'
DECLARE @ATYPRF DATE = '2001-06-01'
DECLARE @NDT DATE = '1900-01-01'
DECLARE @NOW DATETIME = GETDATE()
DECLARE @TODAY DATE = GETDATE()
DECLARE @LENDER VARCHAR(10) = '828476'
DECLARE @SCRIPTID VARCHAR(10) = 'UTLWK22'
DECLARE @COMMENT VARCHAR(200) = 'Review borrower application for any possible new reference records including employer information'
DROP TABLE IF EXISTS #Population;

SELECT DISTINCT 
	LN10.BF_SSN AS TargetId, --TargetID
	PD10.DF_SPE_ACC_ID AS AccountNumber,
	CASE WHEN PD01.DF_PRS_ID IS NULL THEN NULL ELSE @WRKGRP END AS QueueName, --QueueName
	CASE WHEN PD01.DF_PRS_ID IS NULL THEN @PFACT ELSE NULL END AS CompassArc,
	CASE WHEN PD01.DF_PRS_ID IS NULL THEN @WF_QUE ELSE NULL END AS CompassQueue,
	@COMMENT AS Comment, --Comment
	NULL AS SourceFile, --SourceFile
	@NOW AS AddedAt, --AddedAt
	@SCRIPTID AS AddedBy --AddedBy
INTO
	#Population
FROM 
	UDW..LN10_LON LN10
	INNER JOIN
	(
		SELECT 
			DF_PRS_ID
		FROM 
			UDW..PD30_PRS_ADR 
		WHERE
			DC_ADR = 'L'
			AND DI_VLD_ADR = 'N'

		UNION 

		SELECT 
			DF_PRS_ID
		FROM 
			UDW..PD42_PRS_PHN
		WHERE 
			DC_PHN = 'H'
			AND DI_PHN_VLD = 'N'
	) PD3042
		ON PD3042.DF_PRS_ID = LN10.BF_SSN
	INNER JOIN UDW..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = LN10.BF_SSN
	LEFT JOIN ODW..PD01_PDM_INF PD01
		ON PD01.DF_PRS_ID = LN10.BF_SSN
	LEFT JOIN
	(
		SELECT
			AY10.BF_SSN,
			MX.LD_ATY_REQ_RCV
		FROM 
			UDW..AY10_BR_LON_ATY AY10
			LEFT JOIN
			(
				SELECT
					AY10.BF_SSN,
					MAX(AY10.LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
				FROM
					UDW..AY10_BR_LON_ATY AY10
				WHERE 
					AY10.PF_REQ_ACT = @PFACT
					AND AY10.LC_STA_ACTY10 = 'A'
				GROUP BY 
					AY10.BF_SSN
			) MX
				ON MX.BF_SSN = AY10.BF_SSN
	) AY10 
		ON AY10.BF_SSN = LN10.BF_SSN
	LEFT JOIN ODW..CT30_CALL_QUE CT30
		ON CT30.DF_PRS_ID_BR = LN10.BF_SSN
		AND CT30.IF_WRK_GRP = @WRKGRP
		AND CT30.IC_TSK_STA IN ('A','W')
	LEFT JOIN UDW..WQ20_TSK_QUE WQ20
		ON WQ20.BF_SSN = LN10.BF_SSN
		AND WQ20.WF_QUE = @WF_QUE
		AND WQ20.WC_STA_WQUE20 NOT IN('X','C')
	LEFT JOIN 
	(
		SELECT 
			DF_PRS_ID,
			MAX(BD_ATY_PRF) AS BD_ATY_PRF 
		FROM 
			ODW..AY01_BR_ATY 
		WHERE 
			PF_ACT = @PFACT
		GROUP BY 
			DF_PRS_ID
	) AY01
		ON AY01.DF_PRS_ID = LN10.BF_SSN
	LEFT JOIN 
	(
		SELECT 
			DF_PRS_ID,
			'Y' AS K1_IND
		FROM 
			ODW..AY01_BR_ATY 
		WHERE 
			PF_ACT = @PFACT
			AND BD_ATY_PRF > @ATYPRF
		GROUP BY 
			DF_PRS_ID
	) AY012
		ON AY012.DF_PRS_ID = LN10.BF_SSN
WHERE
	LN10.LA_CUR_PRI > 0.00
	AND LN10.LC_STA_LON10 = 'R'
	AND LN10.IC_LON_PGM != 'TILP'
	AND LN10.LF_LON_CUR_OWN = @LENDER
	AND LN10.LD_LON_1_DSB > ISNULL(AY10.LD_ATY_REQ_RCV, @NDT)
	AND 
	(
		LN10.LD_LON_1_DSB > ISNULL(AY01.BD_ATY_PRF,@NDT) 
		OR AY012.K1_IND != 'Y'
	)
	AND CT30.DF_PRS_ID_BR IS NULL
	AND WQ20.BF_SSN IS NULL
ORDER BY
	LN10.BF_SSN


INSERT INTO OLS.olqtskbldr.Queues(TargetId,	QueueName, InstitutionId, InstitutionType, DateDue, TimeDue, Comment, SourceFilename, AddedAt, AddedBy)
SELECT DISTINCT 
	P.TargetId,
	P.QueueName, 
	NULL, --InstitutionId
	NULL, --InstitutionType
	NULL, --DateDue
	NULL, --TimeDue
	P.Comment,
	P.SourceFile,
	P.AddedAt,
	P.AddedBy
FROM 
	#Population P
--check for existing record to add queue task for the current date
	LEFT JOIN OLS.olqtskbldr.Queues ExistingData
		ON ExistingData.TargetId = P.TargetId
		AND ExistingData.QueueName = @WRKGRP
		AND
		(
			CAST(ExistingData.AddedAt AS DATE) = @TODAY
			OR ExistingData.ProcessedAt IS NULL
		)
		AND ExistingData.DeletedAt IS NULL
		AND ExistingData.DeletedBy IS NULL
WHERE
	ExistingData.TargetId IS NULL --Doesnt have a pending record in the queues table
	AND P.QueueName IS NOT NULL --Makes sure that the borrower exist on onelink

INSERT INTO ULS..ArcAddProcessing(ArcTypeId,ArcResponseCodeId,AccountNumber,RecipientId,ARC,ScriptId,Comment,IsReference,IsEndorser,CreatedAt,CreatedBy)
SELECT
	1 AS ArcTypeId,
	NULL AS ArcResponseCodeId,
	P.AccountNumber AS AccountNumber,
	NULL AS RecipientId,
	P.CompassArc AS ARC,
	@SCRIPTID AS ScriptId,
	P.Comment AS Comment,
	0 AS IsReference,
	0 AS IsEndorser,
	P.AddedAt AS CreatedAt,
	P.AddedBy AS CreatedBy
FROM
	#Population P
	LEFT JOIN ULS.dbo.ArcAddProcessing ExistingData
		ON ExistingData.AccountNumber = P.AccountNumber
		AND ExistingData.ArcTypeId = 1 --all loans
		AND ExistingData.ARC = P.CompassArc
		AND ExistingData.Comment = P.Comment
		AND 
		(
			CAST(ExistingData.ProcessedAt AS DATE) = @TODAY
			OR ExistingData.ProcessedAt IS NULL
		)
WHERE
	ExistingData.AccountNumber IS NULL --Doesnt have a pending ArcAdd record
	AND P.CompassArc IS NOT NULL --Exists on Compass and is Compass Only