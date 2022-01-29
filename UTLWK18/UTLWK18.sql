DECLARE 
	@Now DATETIME = GETDATE(),
	@Today DATE = GETDATE(),
	@ScriptId VARCHAR(20) = 'UTLWK18',
	@ExlusionDate DATE = DATEADD(DAY, -45, GETDATE()),
	@QueueName VARCHAR(20) = 'KSKEMAIL'

INSERT INTO OLS.olqtskbldr.Queues
(
	TargetId,
	QueueName,
	InstitutionId,
	InstitutionType,
	DateDue,
	TimeDue,
	Comment,
	SourceFilename,
	AddedAt,
	AddedBy
)
SELECT DISTINCT
	SKIP_POP.DF_PRS_ID AS TargetId,
	@QueueName AS QueueName,
	'' AS InstitutionId,
	'' AS InstitutionType,
	NULL AS DateDue,
	NULL AS TimeDue,
	CONCAT('OneLINK Email: ', RTRIM(LTRIM(ISNULL(SKIP_POP.DX_EML_ADR, ''))), ', Skip type: ', SKIP_POP.SKP) AS Comment,
	'' AS SourceFileName,
	@Now AS AddedAt,
	@ScriptId AS AddedBy
FROM
	(
		--ADDRESS SKIP
		--EML 1
		SELECT 
			PD03.DF_PRS_ID,
			PD03.DX_EML_ADR,
			CASE
				WHEN PD03.DI_VLD_ADR = 'N' AND (PD03.DI_PHN_VLD = 'Y' OR PD03.DI_FGN_PHN = 'Y') THEN 'A' --Address skip
				WHEN PD03.DI_VLD_ADR = 'Y' AND PD03.DI_PHN_VLD = 'N' AND PD03.DI_FGN_PHN = 'N' THEN 'P' --Phone skip
				WHEN PD03.DI_VLD_ADR = 'N' AND PD03.DI_PHN_VLD = 'N' AND PD03.DI_FGN_PHN = 'N' THEN 'B' --Both skip
				ELSE ''
			END AS SKP
		FROM 
			ODW..PD03_PRS_ADR_PHN PD03
			INNER JOIN ODW..GA01_APP GA01
				ON GA01.DF_PRS_ID_BR = PD03.DF_PRS_ID
			INNER JOIN ODW..GA14_LON_STA GA14
				ON GA14.AF_APL_ID = GA01.AF_APL_ID
		WHERE 
			PD03.DC_ADR = 'L'
			AND PD03.DI_EML_ADR_VAL = 'Y'
			AND GA14.AC_STA_GA14 = 'A'
			AND GA14.AC_LON_STA_TYP IN ('CR', 'DA', 'FB', 'IA', 'ID', 'IG', 'IM', 'RF', 'RP', 'UA', 'UB')
			AND
			(
				PD03.DI_VLD_ADR = 'N'
				OR PD03.DI_PHN_VLD = 'N'
			)
	) SKIP_POP
	LEFT JOIN ODW..AY01_BR_ATY AY01
		ON AY01.DF_PRS_ID = SKIP_POP.DF_PRS_ID
		AND PF_ACT = 'KEMAL'
		AND BD_ATY_PRF >= @ExlusionDate --Within the last 45 days
	LEFT JOIN ODW..CT30_CALL_QUE  CT30
		ON CT30.DF_PRS_ID_BR = SKIP_POP.DF_PRS_ID
		AND	IF_WRK_GRP = 'KSKEMAIL'
	LEFT JOIN OLS.olqtskbldr.Queues Q
		ON Q.TargetId = SKIP_POP.DF_PRS_ID
		AND Q.QueueName = @QueueName
		AND 
		(
			CAST(Q.AddedAt AS DATE) = @Today
			OR Q.ProcessedAt IS NULL
		)
WHERE
	--EXCLUSIONS
	AY01.DF_PRS_ID IS NULL
	AND CT30.DF_PRS_ID_BR IS NULL
	AND Q.TargetId IS NULL --Duplicate Exclusion for same day
