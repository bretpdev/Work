DECLARE @TODAY DATE = CAST(GETDATE() AS DATE)
DECLARE @SOURCEFILE VARCHAR(25) = 'ULWM16.LWM16R2'
DECLARE @QUEUE VARCHAR(8) = 'DEMPCALL'
DECLARE @PF_ACT VARCHAR(5) = 'MEMPL'

INSERT INTO OLS.olqtskbldr.Queues(TargetId, QueueName, InstitutionId, InstitutionType, DateDue, TimeDue, Comment, SourceFilename, AddedAt, AddedBy)
SELECT DISTINCT
	DC01.BF_SSN, --TargetId
	@QUEUE, --QueueName
	NULL, --InstitutionId
	NULL, --InstitutionType
	NULL, --DateDue
	NULL, --TimeDue
	CONVERT(VARCHAR, AY01.BD_ATY_PRF, 101) + ',' + REPLACE(REPLACE(REPLACE(LTRIM(AY01.BX_CMT), ' ', '<>'), '><', ''), '<>', ' '), --Comment Removes any double spacing
	@SOURCEFILE, --SourceFilename
	@TODAY, --AddedAt
	USER_NAME() --AddedBy
FROM
	ODW..DC01_LON_CLM_INF DC01
	INNER JOIN ODW..AY01_BR_ATY AY01
		ON AY01.DF_PRS_ID = DC01.BF_SSN
	LEFT JOIN ODW..AY01_BR_ATY AY01_DEMPL --Gets any account that has had a DEMPL task in the past 10 days
		ON AY01.DF_PRS_ID = DC01.BF_SSN
		AND AY01.PF_ACT = 'DEMPL'
		AND AY01.BD_ATY_PRF >= DATEADD(D, -10, @TODAY)
	LEFT JOIN
	(--Orders the borrower tasks by BD_ATY_PRF date and the PF_ACT to check if the newest Queue is MEMPL
		SELECT
			DF_PRS_ID,
			PF_ACT,
			BD_ATY_PRF,
			BX_CMT, BF_CRT_DTS_AY01,
			DENSE_RANK() OVER (PARTITION BY DF_PRS_ID ORDER BY BD_ATY_PRF DESC, PF_ACT DESC, BF_CRT_DTS_AY01 DESC) QueueRank
		FROM
			ODW..AY01_BR_ATY
		WHERE
			PF_ACT IN ('MEMPL','MEMP0','DEMP1')
	) Ranked
		ON Ranked.DF_PRS_ID = AY01.DF_PRS_ID
		AND Ranked.PF_ACT = AY01.PF_ACT
		AND Ranked.BD_ATY_PRF = AY01.BD_ATY_PRF
		AND Ranked.BF_CRT_DTS_AY01 = AY01.BF_CRT_DTS_AY01
	LEFT JOIN ODW..CT30_CALL_QUE CT30
		ON CT30.DF_PRS_ID_BR = DC01.BF_SSN
		AND CT30.IF_WRK_GRP = @QUEUE
		AND CT30.IC_TSK_STA IN ('A','W')
	LEFT JOIN OLS.olqtskbldr.Queues Q
		ON Q.TargetId = DC01.BF_SSN
		AND Q.QueueName = @QUEUE
		AND 
		(
			CAST(Q.AddedAt AS DATE) = @TODAY
			OR Q.ProcessedAt IS NULL
		)
		AND Q.DeletedAt IS NULL
		AND Q.Comment = CONVERT(VARCHAR, AY01.BD_ATY_PRF, 101) + ',' + REPLACE(REPLACE(REPLACE(LTRIM(AY01.BX_CMT), ' ', '<>'), '><', ''), '<>', ' ')
WHERE
	DC01.LC_STA_DC10 = '03'
	AND DC01.LD_CLM_ASN_DOE IS NULL
	AND DC01.LC_AUX_STA = ''
	AND CT30.DF_PRS_ID_BR IS NULL --Checks for open task
	AND Q.TargetId IS NULL --Make sure there is not an open task in the table
	AND Ranked.QueueRank = 1
	AND Ranked.PF_ACT = @PF_ACT
	AND AY01.BX_CMT NOT IN ('# (LPXG0   )','N/A (LPXG0   )','NA (LPXG0   )','EMPLOYER (LPXG0   )','UNKNOWN (LPXG0   )','UKNOWN (LPXG0   )','YNKNOWN (LPXG0   )','* (LPXG0   )','OTHER (LPXG0   )','EMP (LPXG0   )','1 (LPXG0   )','## (LPXG0   )','. (LPXG0   )','E (LPXG0   )','***** (LPXG0   )','- (LPXG0   )',' # (LPXG0   )',' , (LPXG0   )','/ (LPXG0   )','/* (LPXG0   )','0 (LPXG0   )','1 (LPXG0   )','@ (LPXG0   )','A (LPXG0   )','? (LPXG0   )','¬ (LPXG0   )','WRK (LPXG0   )','WRK # (LPXG0   )','WK (LPXG0   )','WORK (LPXG0   )','WORK # (LPXG0   )','XXX (LPXG0   )','XX (LPXG0   )','X (LPXG0   )','### (LPXG0 )')
	AND AY01_DEMPL.DF_PRS_ID IS NULL
ORDER BY
	DC01.BF_SSN